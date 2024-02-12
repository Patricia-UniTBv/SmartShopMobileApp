using DTO;

namespace API.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ICollection<CategoryDTO>> GetAllCategories();

    }
}
