using DTO;

namespace API.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddCartItem(CartItemDTO item);
    }
}
