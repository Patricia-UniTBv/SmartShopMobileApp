using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class OfferProfile:Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, OfferDTO>();
            CreateMap<OfferDTO, Offer>();
        }
    }
}
