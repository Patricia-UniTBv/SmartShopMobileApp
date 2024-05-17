using DTO;

namespace API.Repository.Interfaces
{
    public interface IVoucherRepository
    {
        void UpdateVoucherForSpecificUser(VoucherDTO voucher);
        Task<VoucherDTO> GetVoucherForUserAndSupermarket(int UserId, int SupermarketId);
        Task CreateVoucherForUserAndSupermarket(VoucherDTO newVoucher);
    }
}
