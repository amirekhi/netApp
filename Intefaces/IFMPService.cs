using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Models;

namespace MyWebApi.Intefaces
{
    public interface IFMPService
    {
         
        Task<Stock> FindStockBySymbolAsync(string symbol);
    
    }
}