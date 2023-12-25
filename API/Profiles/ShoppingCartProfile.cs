using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDTO>().ForMember(dest => dest.Supermarket, opt => opt.MapFrom(src => src.Supermarket));
            CreateMap<ShoppingCartDTO, ShoppingCart>();
        }
    }
}
