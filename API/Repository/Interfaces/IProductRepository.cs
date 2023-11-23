using DTO;

namespace API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<ICollection<ProductDTO>> GetAllProductsForSupermarket(int supermarketID);
        Task<ProductDTO> GetProductByBarcode(string barcode);
    }
}
