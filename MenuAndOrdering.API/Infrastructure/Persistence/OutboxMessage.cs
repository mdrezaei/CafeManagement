namespace MenuAndOrdering.API.Infrastructure.Persistence
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public bool IsProcessed { get; set; }
        public int RetryCount { get; set; }
    }
}
