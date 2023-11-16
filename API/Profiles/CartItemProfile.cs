using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CartItemDTO, CartItem>();
        }
    }
}
