using System;
using Stockfish.Model;
using Stockfish.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Net.WebSockets;
using System.Globalization;

namespace Stockfish.DAL
{
    public class StockRepo : IStockRepo
    {

        public static int _currentId;

        private readonly StockContext _db;
        private ILogger<StockRepo> _log;

        public StockRepo(StockContext db, ILogger<StockRepo> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            try
            {
                List<Stock> allStocks = await _db.Stocks.ToListAsync();
                return allStocks;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                Users newUser = new Users();
                var newPostOffice = new PostOffices(); 
                newUser.Address = user.Address;
                newUser.Firstname = user.Firstname;
                newUser.Surname = user.Surname;
                newPostOffice.PostalOffice = user.PostalOffice;
                newPostOffice.PostalCode = user.PostalCode;
                newUser.PostOffice = newPostOffice;
                newUser.Username = user.Username;
                byte[] salt = CreateSalt();
                newUser.Salt = salt;
                byte[] hash = createHash(user.Password, salt);
                newUser.Password = hash;
                _db.Add(newUser);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<List<Orders>> GetUserStocks()
        {
            try
            {
                _log.LogInformation("Inside of GetUserStocks");
                List<Orders> orderList = new List<Orders>();
                List<Orders> myFucks = new List<Orders>();
                orderList = await _db.Orders.Where(o => o.User.Id == _currentId).ToListAsync();
                myFucks = await orderList.DistinctBy(o => o.Stock.Id).ToListAsync();
                _log.LogInformation("Created orderList");
                foreach (Orders order in orderList)
                {
                    _log.LogInformation("Name" + order.Stock.Name);
                    _log.LogInformation("Quantity" + order.Quantity.ToString());
                }
                var myStocks = orderList.GroupBy(s => s.Stock.Id)
                    .Select(g => new
                    {   Stockname = g.Select(s => s.Stock.Name),
                        Quantity = g.Sum(c => c.Quantity),
                        Price = g.Select(p => p.Stock.Price), 
                        Turnover = g.Select(t => t.Stock.Turnover),
                        Diff = g.Select(d => d.Stock.Diff)
                    });
                _log.LogInformation("Created myStocks");
                foreach (var order in myStocks)
                {
                    _log.LogInformation("Name"+order.Stockname);
                    _log.LogInformation("Quantity"+order.Quantity.ToString());
                    _log.LogInformation("Price" + order.Price.ToString());
                    _log.LogInformation("Turnover" + order.Turnover.ToString());
                    _log.LogInformation("Diff" + order.Diff.ToString());
                }
                return orderList;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<bool> BuyStock(int stockId, int quantity)
        {
            try
            {
                int current = _currentId;
                Orders order = new Orders();
                Stock stock = await _db.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);
                Users user = await _db.Users.FirstOrDefaultAsync(u => u.Id == current);
                if (user != null && stock != null)
                {
                    order.Stock = stock;
                    order.User = user;
                }
                order.Quantity = quantity;
                order.Date = DateTime.Now;
                _log.LogInformation(stockId.ToString());
                _log.LogInformation(current.ToString());
                _log.LogInformation(order.Date.ToString());
                _log.LogInformation(order.Quantity.ToString());
                //_log.LogInformation(order.User.Id.ToString());
                //_log.LogInformation(order.Stock.Id.ToString());
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        /*public Task<bool> SellStock(Order order)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }*/

        public async Task<bool> AddStock(Stock stock)
        {
            try
            {
                var newStock = new Stock();
                newStock.Name = stock.Name;
                newStock.Price = stock.Price;
                newStock.Turnover = stock.Turnover;
                newStock.Diff = stock.Diff;
                _db.Add(newStock);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateStock(Stock stock)
        {
            try
            {
                var updateStock = await _db.Stocks.FindAsync(stock.Id);
                updateStock.Name = stock.Name;
                updateStock.Price = stock.Price;
                updateStock.Turnover = stock.Turnover;
                updateStock.Diff = stock.Diff;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteStock(int stockId)
        {
            try
            {
                _log.LogInformation(stockId.ToString());
                Stock stock = await _db.Stocks.FindAsync(stockId);
                _db.Stocks.Remove(stock);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        /*public Task<bool> GetStock(Stock stock)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }*/

        public static byte[] CreateSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public static byte[] createHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
            numBytesRequested: 32);
        }

        public async Task<bool> CheckUsername(User user)
        {
            try
            {
                if (await _db.Users.Where(u => u.Username == user.Username).ToListAsync() != null) { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<int> Login(string username, string password)
        {
            try
            {
                Users user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
                byte[] hash = createHash(password, user.Salt);
                bool ok = hash.SequenceEqual(user.Password);
                if (ok)
                {
                    _log.LogInformation(_currentId.ToString());
                    _currentId = user.Id;
                    _log.LogInformation("Rett etter currentId er satt for første gang" + _currentId.ToString());
                    if (user.Admin == 1)
                    {
                        return 1;
                    }
                    return 0;
                }
                return -1;
            } 
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return -1;
            }
            
        }
    }
}