namespace Coordinator.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Guid> CreateTransactionAsync();
        Task PrepareServiceAsync(Guid transactionId);
        Task<bool>CheckReadyServiceAsync(Guid transactionId);
        Task CommitAsync(Guid transactionId);
        Task<bool>CheckTransactionStateServicesAsync(Guid transactionId);   
        Task RoolbackAsync(Guid transactionId); 
    }
}
