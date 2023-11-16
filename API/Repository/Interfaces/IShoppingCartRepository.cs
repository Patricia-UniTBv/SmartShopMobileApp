using DTO;

namespace API.Repository.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCartDTO> GetShoppingCartForSpecificUser(int userID);
        Task AddShoppingCart(ShoppingCartDTO shoppingCart);
    }
}
