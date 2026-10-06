using CafeManagement.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using CafeManagement.Shared.Events;

namespace CafeManagement.API.Infrastructure.BackgroundServices
{
    public class OutboxProcessorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OutboxProcessorService> _logger;
        private readonly IConfiguration _configuration;

        public OutboxProcessorService(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory, ILogger<OutboxProcessorService> logger, IConfiguration configuration)
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
                    var dbContext = scope.ServiceProvider.GetRequiredService<CafeManagementDbContext>();

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

                            // اینجا بر اساس EventType آدرس مقصد رو مشخص کن
                            var destinationUrl = GetDestinationUrl(message.EventType);
                            //-----
                            Type? eventType = message.EventType switch
                            {
                                nameof(CustomerCreatedEvent) => typeof(CustomerCreatedEvent),
                                nameof(CustomerUpdatedEvent) => typeof(CustomerUpdatedEvent),
                                nameof(CustomerDeletedEvent) => typeof(CustomerDeletedEvent),
                                nameof(OrderPlacedEvent) => typeof(OrderPlacedEvent),
                                nameof(OrderStatusChangedEvent) => typeof(OrderStatusChangedEvent),
                                nameof(TableCreatedEvent) => typeof(TableCreatedEvent),
                                nameof(TableUpdatedEvent) => typeof(TableUpdatedEvent),
                                nameof(TableDeletedEvent) => typeof(TableDeletedEvent),
                                nameof(MenuItemCreatedEvent) => typeof(MenuItemCreatedEvent),
                                nameof(MenuItemUpdatedEvent) => typeof(MenuItemUpdatedEvent),
                                nameof(MenuItemDeletedEvent) => typeof(MenuItemDeletedEvent),
                                _ => null
                            };

                            if (eventType == null)
                            {
                                Console.WriteLine($"invalid type: {message.EventType}");
                                continue;
                            }

                            var eventData = JsonSerializer.Deserialize(message.EventData, eventType);

                            var httpMethod = GetHttpMethod(message.EventType);

                            using var content = new StringContent(
                                message.EventData,
                                System.Text.Encoding.UTF8,
                                "application/json");

                            using var request = new HttpRequestMessage(httpMethod, destinationUrl)
                            {
                                Content = content
                            };

                            var response = await client.SendAsync(request, stoppingToken);

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
                    _logger.LogError(ex, "خطا در پردازش Outbox");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private string GetDestinationUrl(string eventType)
        {
            var baseUrl = _configuration["MenuAndOrderingBaseUrl"] ?? "https://localhost:7205"; // MenuAndOrdering.API

            return eventType switch
            {
                nameof(TableCreatedEvent) => $"{baseUrl}/api/sync/table",
                nameof(TableUpdatedEvent) => $"{baseUrl}/api/sync/table",
                nameof(TableDeletedEvent) => $"{baseUrl}/api/sync/table/delete",
                nameof(MenuItemCreatedEvent) => $"{baseUrl}/api/sync/menuitem",
                nameof(MenuItemUpdatedEvent) => $"{baseUrl}/api/sync/menuitem",
                nameof(MenuItemDeletedEvent) => $"{baseUrl}/api/sync/menuitem/delete",
                nameof(CustomerCreatedEvent) => $"{baseUrl}/api/sync/customer",
                nameof(CustomerUpdatedEvent) => $"{baseUrl}/api/sync/customer",
                nameof(CustomerDeletedEvent) => $"{baseUrl}/api/sync/customer/delete",
                nameof(OrderStatusChangedEvent) => $"{baseUrl}/api/sync/order/status",
                _ => ""
            };

            //return eventType switch
            //{
            //    "TableCreatedEvent" => $"{baseUrl}/table",
            //    "TableUpdatedEvent" => $"{baseUrl}/table",
            //    "TableDeletedEvent" => $"{baseUrl}/table/delete",
            //    "MenuItemCreatedEvent" => $"{baseUrl}/menuitem",
            //    "MenuItemUpdatedEvent" => $"{baseUrl}/menuitem",
            //    "MenuItemDeletedEvent" => $"{baseUrl}/menuitem/delete",
            //    "CustomerCreatedEvent" => $"{baseUrl}/customer",
            //    "CustomerUpdatedEvent" => $"{baseUrl}/customer",
            //    "CustomerDeletedEvent" => $"{baseUrl}/customer/delete",
            //    "OrderStatusChangedEvent" => $"{baseUrl}/order/status",
            //    _ => ""
            //};
        }

        private HttpMethod GetHttpMethod(string eventType)
        {
            return eventType switch
            {
                // ایجادها → POST
                nameof(TableCreatedEvent) => HttpMethod.Post,
                nameof(MenuItemCreatedEvent) => HttpMethod.Post,
                nameof(CustomerCreatedEvent) => HttpMethod.Post,
                nameof(OrderPlacedEvent) => HttpMethod.Post,

                // به‌روزرسانی‌ها → PUT
                nameof(TableUpdatedEvent) => HttpMethod.Put,
                nameof(MenuItemUpdatedEvent) => HttpMethod.Put,
                nameof(CustomerUpdatedEvent) => HttpMethod.Put,

                // تغییر وضعیت → PATCH
                nameof(OrderStatusChangedEvent) => HttpMethod.Patch,

                // حذف‌ها → DELETE
                nameof(TableDeletedEvent) => HttpMethod.Delete,
                nameof(MenuItemDeletedEvent) => HttpMethod.Delete,
                nameof(CustomerDeletedEvent) => HttpMethod.Delete,

                _ => HttpMethod.Post
            };
        }

    }
}
