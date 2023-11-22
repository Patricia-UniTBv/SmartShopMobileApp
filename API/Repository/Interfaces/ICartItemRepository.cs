using DTO;

namespace API.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        Task<ICollection<ProductDTO>> GetProductsByShoppingCart(int shoppingCartId);
        Task AddCartItem(CartItemDTO item);
    }
}
