using System;
using Stockfish.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

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
                _db.Add(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<bool> CheckUsername(User user)
        {
            try
            {
                if (await _db.Users.Where(u => u.Username == user.Username).ToListAsync() != null) { return true; }
                else { return false; }
            } catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

        public async Task<List<Order>> GetUserStocks(int userId)
        {
            try
            {
                List<Order> userOrders = await _db.Orders.Where(o => o.User.Id == userId).ToListAsync();
                return userOrders;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return null;
            }
        }

        public Task<bool> BuyStock(Order order)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }

        public Task<bool> SellStock(Order order)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }

        public Task<bool> AddStock(Stock stock)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }

        public Task<bool> UpdateStock(Stock stock)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }

        public Task<bool> DeleteStock(Stock stock)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }

        public Task<bool> GetStock(Stock stock)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
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

        public static byte[]  createSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }
        public async Task<bool> login(string username, string password)
        {
            try
            {
                int result = 0;
                if (await _db.Users.FindAsync(username) != null)
                {
                    Users user = await _db.Users.FindAsync(username);
                    byte[] hash = createHash(password, user.Salt);
                    bool ok = hash.SequenceEqual(user.Password);
                    if (ok)
                    {
                        _currentId = user.Id;
                        return true;
                    }
                    
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