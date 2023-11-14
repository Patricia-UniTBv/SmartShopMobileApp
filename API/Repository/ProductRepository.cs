using AutoMapper;
using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SmartShopDBContext context, IMapper _mapper) : base(context, _mapper)
        {
        }
        public async Task<ICollection<ProductDTO>> GetAllProducts()
        {
            var product = await _dbSet.ToListAsync();
            return _mapper.Map<ICollection<Product>, ICollection<ProductDTO>>(product);
        }

        public async Task<ProductDTO> GetProductByBarcode(string barcode)
        {
            var product = await _dbSet.Where(p => p.Barcode == barcode).FirstAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }
    }
}
