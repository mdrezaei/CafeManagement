namespace CafeManagement.API.Application.Services
{
    public interface IOutboxService
    {
        Task AddMessageAsync<T>(T @event);
    }
}
