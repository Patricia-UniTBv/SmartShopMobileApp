using DTO;

namespace API.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<ICollection<TransactionDTO>> GetAllTransactions();
        Task<ICollection<TransactionDTO>> GetAllTransactionsWithDiscount(int userId, int supermarketId);
        Task<TransactionDTO> GetTransactionBySupermarketId(int shoppingCartId);
        Task AddTransaction(TransactionDTO transaction);
        void UpdateVoucherDiscountForTransaction(TransactionDTO transaction);
    }
}
