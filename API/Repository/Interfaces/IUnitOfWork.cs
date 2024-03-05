namespace API.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IProductRepository ProductRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IUserRepository UserRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ISupermarketRepository SupermarketRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ILocationRepository LocationRepository { get; }
        IOfferRepository OfferRepository { get; }
        Task CompleteAsync();
    }
}
