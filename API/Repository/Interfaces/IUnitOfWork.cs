namespace API.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IProductRepository ProductRepository { get; }

        Task CompleteAsync();
    }
}
