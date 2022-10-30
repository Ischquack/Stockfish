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
        private readonly StockContext _db;
        private ILogger<StockRepo> _log;
        private int _currentId;

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

        public async Task<List<Orders>> GetUserStocks(int userId)
        {
            try
            {
                List<Orders> userOrders = await _db.Orders.Where(o => o.User.Id == userId).ToListAsync();
                return userOrders;
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
                Orders order = new Orders();
                Stock stock = await _db.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);
                Users user = await _db.Users.FirstOrDefaultAsync(u => u.Id == _currentId);
                order.Stock = stock;
                order.User = user;
                order.Quantity = quantity;
                order.Date = DateTime.Now;
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
                var newStock = new Stock(stock.Name, stock.Price, stock.Turnover, stock.Diff);
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

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                Users user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
                byte[] hash = createHash(password, user.Salt);
                bool ok = hash.SequenceEqual(user.Password);
                if (ok)
                {
                    _currentId = user.Id;
                    return true;
                }
                return false;
            } 
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
            
        }
    }
}