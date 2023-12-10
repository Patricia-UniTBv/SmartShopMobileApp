using DTO;

namespace API.Repository.Interfaces
{
    public interface IVoucherRepository
    {
        Task AddVoucherForSpecificUser(double earnedMoney);
    }
}
