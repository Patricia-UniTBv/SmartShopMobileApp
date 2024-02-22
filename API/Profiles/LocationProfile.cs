using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class LocationProfile: Profile
    {
        public 
            LocationProfile()
        {
            CreateMap<Location, LocationDTO>();
            CreateMap<LocationDTO, Location>();
        }
    }
}
