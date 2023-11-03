using DTO;

namespace API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<ProductDTO>> GetAllProducts();
    }
}
