using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.DTOs.Stock;
using MyWebApi.Helpers;
using MyWebApi.Models;

namespace MyWebApi.Intefaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> GetBySymbolAsync(string symbol);

        Task<Stock?> UpdateAsync(int id , UpdateStockRequestDto stockDto);

        Task<Stock?> DeleteAsync(int id);

        Task<bool>  StockExist(int id);

    }
}