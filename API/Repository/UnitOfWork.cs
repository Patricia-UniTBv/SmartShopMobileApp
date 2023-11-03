using API.Models;
using API.Repository.Interfaces;
using AutoMapper;

namespace API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepository;
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context, _mapper);
      
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
