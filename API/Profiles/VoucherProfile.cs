using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class VoucherProfile:Profile
    {
        public VoucherProfile()
        {
            CreateMap<Voucher, VoucherDTO>();
            CreateMap<VoucherDTO, Voucher>();
        }
    }
}
