using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stockfish.Model;

namespace Stockfish.DAL
{
    public interface IStockRepo
    {
        Task<List<Stock>> GetAllStocks();
    }
}

