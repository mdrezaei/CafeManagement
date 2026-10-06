using System.Text.Json;
using CafeManagement.Shared.Events;
using MenuAndOrdering.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuAndOrdering.API.Infrastructure.BackgroundServices;

public class OutboxProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OutboxProcessorService> _logger;
    private readonly IConfiguration _configuration;

    public OutboxProcessorService(
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory,
        ILogger<OutboxProcessorService> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MenuAndOrderingDbContext>();

                var pendingMessages = await dbContext.OutboxMessages
                    .Where(m => !m.IsProcessed)
                    .OrderBy(m => m.CreatedAt)
                    .Take(10)
                    .ToListAsync(stoppingToken);

                foreach (var message in pendingMessages)
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient();
                        var destinationUrl = GetDestinationUrl(message.EventType);

                        Type? eventType = message.EventType switch
                        {
                            nameof(OrderPlacedEvent) => typeof(OrderPlacedEvent),
                            nameof(CustomerCreatedEvent) => typeof(CustomerCreatedEvent),
                            nameof(CustomerUpdatedEvent) => typeof(CustomerUpdatedEvent),
                            nameof(CustomerDeletedEvent) => typeof(CustomerDeletedEvent),
                            _ => null
                        };

                        if (eventType == null)
                        {
                            _logger.LogWarning("unknown event: {EventType}", message.EventType);
                            continue;
                        }

                        var eventData = JsonSerializer.Deserialize(message.EventData, eventType);

                        var response = await client.PostAsJsonAsync(
                            destinationUrl,
                            eventData,
                            stoppingToken
                        );

                        if (response.IsSuccessStatusCode)
                        {
                            message.IsProcessed = true;
                            message.ProcessedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            message.RetryCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "error at sending {EventType}", message.EventType);
                        message.RetryCount++;
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error at processing Outbox");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private string GetDestinationUrl(string eventType)
    {
        // آدرس CafeManagement.API
        var baseUrl = _configuration["CafeManagementBaseUrl"] ?? "https://localhost:7256";

        return eventType switch
        {
            nameof(OrderPlacedEvent) => $"{baseUrl}/api/sync/order",
            nameof(CustomerCreatedEvent) => $"{baseUrl}/api/sync/customer",
            nameof(CustomerUpdatedEvent) => $"{baseUrl}/api/sync/customer",
            nameof(CustomerDeletedEvent) => $"{baseUrl}/api/sync/customer/delete",
            _ => ""
        };

        //return eventType switch
        //{
        //    nameof(OrderPlacedEvent) => $"{baseUrl}/order",
        //    nameof(CustomerCreatedEvent) => $"{baseUrl}/customer",
        //    nameof(CustomerUpdatedEvent) => $"{baseUrl}/customer",
        //    nameof(CustomerDeletedEvent) => $"{baseUrl}/customer/delete",
        //    _ => ""
        //};
    }
}