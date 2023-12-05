using DTO;

namespace API.Repository.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCartDTO?> GetShoppingCartForSpecificUser(int userID);
        Task<ShoppingCartDTO> GetLatestShoppingCartForCurrentUser(int userID);
        Task<ShoppingCartDTO> GetShoppingCartById(int id);
        Task AddShoppingCart(ShoppingCartDTO shoppingCart);
        Task UpdateShoppingCart(ShoppingCartDTO shoppingCart);
    }
}
