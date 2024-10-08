﻿using DTO;

namespace API.Repository.Interfaces
{
    public interface ISupermarketRepository
    {
        Task<ICollection<SupermarketDTO>> GetAllSupermarkets();
        Task<SupermarketDTO> GetSupermarketById(int supermarketId);
    }
}
