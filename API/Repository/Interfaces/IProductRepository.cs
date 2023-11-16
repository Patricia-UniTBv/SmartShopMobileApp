using DTO;

namespace API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductByBarcode(string barcode);
        Task<ProductDTO> AddProductToShoppingCart(ProductDTO product);
    }
}
