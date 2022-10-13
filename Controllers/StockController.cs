using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stockfish.DAL;
using Stockfish.Model;

namespace Stockfish.Controllers
{
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepo _db;

        public StockController(IStockRepo db)
        {
            _db = db;
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            return await _db.GetAllStocks();
        }
    }
}