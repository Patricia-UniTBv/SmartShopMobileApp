using DTO;

namespace API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<ProductDTO>> GetAllProducts();
        Task<ICollection<ProductDTO>> GetAllProductsForSupermarket(int supermarketID);
        Task<ProductDTO> GetProductByBarcode(string barcode);
    }
}
