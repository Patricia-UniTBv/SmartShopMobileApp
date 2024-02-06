using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class CreditCardProfile: Profile
    {
        public CreditCardProfile()
        {
            CreateMap<CreditCard, CreditCardDTO>();
            CreateMap<CreditCardDTO, CreditCard>();
        }
    }
}
