using CafeManagement.API.Infrastructure.Persistence;
using System.Text.Json;

namespace CafeManagement.API.Application.Services
{
    public class OutboxService : IOutboxService
    {
        private readonly CafeManagementDbContext _context;

        public OutboxService(CafeManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync<T>(T @event)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = typeof(T).Name,
                EventData = JsonSerializer.Serialize(@event),
                CreatedAt = DateTime.UtcNow,
                IsProcessed = false,
                RetryCount = 0
            };

            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();
        }
    }
}
