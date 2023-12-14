using API.Models;
using AutoMapper;
using DTO;

namespace API.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();
        }
    }
}
