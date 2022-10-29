using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stockfish.Model;

namespace Stockfish.DAL
{
    public interface IStockRepo
    {
        Task<List<Stock>> GetAllStocks();
        Task<bool> CheckUsername(User user);
        Task<bool> RegisterUser(User user);
        Task<List<Stock>> GetUserStocks(int userId);
        Task<bool> BuyStock(Order order);
        Task<bool> SellStock(Order order);
        Task<bool> AddStock(Stock stock);
        Task<bool> UpdateStock(Stock stock);
        Task<bool> DeleteStock(Stock stock);
        Task<bool> GetStock(Stock stock);

    }


}

