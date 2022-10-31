using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stockfish.Model;
using Stockfish.DAL;

namespace Stockfish.DAL
{
    public interface IStockRepo
    {
        Task<List<Stock>> GetAllStocks();
        Task<bool> CheckUsername(User user);
        Task<int> Login(string username, string password);
        Task<bool> RegisterUser(User user);
        Task<List<MyStocks>> GetUserStocks();
        Task<bool> BuyStock(int stockId, int quantity);
        //Task<bool> SellStock(Order order);
        Task<bool> AddStock(Stock stock);
        Task<bool> UpdateStock(Stock stock);
        Task<bool> DeleteStock(int stockId);
        //Task<bool> GetStock(Stock stock);
    }
}

