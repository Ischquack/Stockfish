using System;
using Stockfish.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Stockfish.DAL
{
    public class StockRepo : IStockRepo
    {
        private readonly StockContext _db;

        public StockRepo(StockContext db)
        {
            _db = db;
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            try
            {
                List<Stock> allStocks = await _db.Stocks.Select(s => new Stock
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Value = s.Value
                }).ToListAsync();
                return allStocks;
            }
            catch
            {
                return null;
            }
        }
    }
}