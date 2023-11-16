namespace API.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IProductRepository ProductRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IUserRepository UserRepository { get; }
        ICartItemRepository CartItemRepository { get; }

        Task CompleteAsync();
    }
}
