using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.DTOs.Stock;
using MyWebApi.Helpers;
using MyWebApi.Intefaces;
using MyWebApi.Models;

namespace MyWebApi.Repository
{
    public class StockRepository : IStockRepository
    {


        public readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
             var stockModel =  await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
             if(stockModel == null){
                return null;
             }
             _context.Stocks.Remove(stockModel);
             await _context.SaveChangesAsync();
             return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var Stocks =  _context.Stocks.Include(x => x.Comments).ThenInclude( a => a.AppUser).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                Stocks = Stocks.Where(s => s.CompanyName == query.CompanyName);
            }

            if(!string.IsNullOrWhiteSpace(query.Symbol)){
                Stocks = Stocks.Where(s => s.Symbol == query.Symbol);
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy)){
                if(query.SortBy.Equals("symbol" , StringComparison.OrdinalIgnoreCase)){
                    Stocks =  query.IsDecending ? Stocks.OrderByDescending(s => s.Symbol) : Stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipSize = (query.PageNumber  - 1) * query.PageSize ;

            return await Stocks.Skip(skipSize).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
           return   await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
             return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
           var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null) {
                return null;
            }

                stockModel.Symbol = stockDto.Symbol;
                stockModel.CompanyName = stockDto.CompanyName;
                stockModel.Purchase = stockDto.Purchase;
                stockModel.LastDiv = stockDto.LastDiv;
                stockModel.Industry = stockDto.Industry;
                stockModel.MarketCap = stockDto.MarketCap;

             await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}