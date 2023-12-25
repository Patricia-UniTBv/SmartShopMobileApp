using DTO;

namespace API.Repository.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<List<ShoppingCartDTO>> GetAllTransactedShoppingCartsByUserId(int userID);
        Task<List<ShoppingCartDTO>> GetAllTransactedShoppingCartsWithSupermarketByUserId(int userID);
        Task<ShoppingCartDTO?> GetShoppingCartForSpecificUser(int userID);
        Task<ShoppingCartDTO> GetLatestShoppingCartForCurrentUser(int userID);
        Task<ShoppingCartDTO> GetShoppingCartById(int id);
        Task AddShoppingCart(ShoppingCartDTO shoppingCart);
        Task UpdateShoppingCart(ShoppingCartDTO shoppingCart);
    }
}
