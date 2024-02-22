using API.Models;
using API.Repository.Interfaces;
using AutoMapper;

namespace API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepository;
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context, _mapper);
        private IShoppingCartRepository? _shoppingCartRepository;
        public IShoppingCartRepository ShoppingCartRepository => _shoppingCartRepository ??= new ShoppingCartRepository(_context, _mapper);
        private IUserRepository? _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _mapper);
        private ICartItemRepository? _cartItemRepository;
        public ICartItemRepository CartItemRepository => _cartItemRepository ??= new CartItemRepository(_context, _mapper);
        private ISupermarketRepository? _supermarketRepository;
        public ISupermarketRepository SupermarketRepository => _supermarketRepository ??= new SupermarketRepository(_context, _mapper);
        private IVoucherRepository? _voucherRepository;
        public IVoucherRepository VoucherRepository => _voucherRepository ??= new VoucherRepository(_context, _mapper);
        private ITransactionRepository? _transactionRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_context, _mapper);
        private ICategoryRepository? _categoryRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context, _mapper);

        private ILocationRepository? _locationRepository;
        public ILocationRepository LocationRepository => _locationRepository ??= new LocationRepository(_context, _mapper);

        private readonly SmartShopDBContext _context;

        private readonly IMapper _mapper;

        public UnitOfWork(SmartShopDBContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }

                _disposed = true;
            }
        }
    }
}
