using DTO;

namespace API.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionDTO> GetTransactionBySupermarketId(int shoppingCartId);
        Task AddTransaction(TransactionDTO transaction);
    }
}
