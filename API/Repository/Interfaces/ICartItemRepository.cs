using DTO;

namespace API.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        Task<ICollection<ProductDTO>> GetProductsByShoppingCart(int shoppingCartId);
        Task<CartItemDTO> GetCartItemById(int id);
        Task<CartItemDTO> GetCartItemByProductIdAndShoppingCartId(int productId, int shoppingCartId, double quantity);
        Task AddCartItem(CartItemDTO item);
        void DeleteCartItem(CartItemDTO item);
    }
}
