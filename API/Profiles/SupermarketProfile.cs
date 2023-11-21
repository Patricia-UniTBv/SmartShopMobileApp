using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class SupermarketProfile : Profile
    {
        public SupermarketProfile()
        {
            CreateMap<Supermarket, SupermarketDTO>();
            CreateMap<SupermarketDTO, Supermarket>();
        }
    }
}
