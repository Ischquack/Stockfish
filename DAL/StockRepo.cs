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
    /* This class contains methods that gets called on in StockController.
     * It handles db queries, exception handling and overall logic for all
     * client actions. 
     * Every time a query to db is made, an exception is returned if the db
     * was not able to perform the desired action. All queries are async to
     * improve stability and performance.
     */
    public class StockRepo : IStockRepo
    {
        public static int _currentId;       /* Variable that holds the
                                             current User ID */

        private readonly StockContext _db;  /* Gets access to read/write in
                                             Stocks.db. */
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

        /* This method retrieves a User type (model) from the client and 
         * makes a new Users object for inserting into db. It  also creates 
         * a PostOffices instance to be inserted into the Users object to ensure
         * correct insertion in db. 
         * The password gets salted and hashed before inserting it in db.
         */
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

        /* This method returns a list containing all stocks a spesific user
         * owns. A spesific model class MyStocks.cs is created to represent this
         * list as it differs slightly from the regular Orders type.
         */
        public async Task<List<MyStocks>> GetUserStocks()
        {
            try
            {
                List<Orders> orderList = new List<Orders>();
                List<MyStocks> myStocks = new List<MyStocks>();
                // All stocks where Order.User.Id == the logged in user´s ID:
                orderList = await _db.Orders.
                    Where(o => o.User.Id == _currentId).ToListAsync();
                /* Creates a list from orderList containing the information
                 * about each different stock that the logged in user owns: */
                var stockAttributes = orderList.DistinctBy(o => o.Stock.Id)
                    .Select(a => new
                    {
                        Id = a.Stock.Id,
                        Name = a.Stock.Name,
                        Price = a.Stock.Price,
                        Turnover = a.Stock.Turnover,
                        Diff = a.Stock.Diff,
                        Quantity = 0
                    }).ToList();
                /* Creates a list containing the total amount of each stock
                 * that the logged in user owns: */ 
                var quantity = orderList.GroupBy(s => s.Stock.Id)
                    .Select(g => new
                    { Quantity = g.Sum(q => q.Quantity)}).ToList();

                /* This loop inserts a new MyStocks instance into myStocks for 
                 * each iteration and feeds the attributes that got stored in
                 * the stockAttributes and quantity lists: */
                for (int i = 0; i < quantity.Count(); i++)
                {   
                    MyStocks myStock = new MyStocks();
                    myStock.Id = stockAttributes[i].Id;
                    myStock.Name = stockAttributes[i].Name;
                    myStock.Price = stockAttributes[i].Price;
                    myStock.Turnover = stockAttributes[i].Turnover;
                    myStock.Diff = stockAttributes[i].Diff;
                    myStock.Quantity = quantity[i].Quantity;
                    myStocks.Add(myStock);                    
                }

                return myStocks;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return null;
            }
        }

        public async Task<bool> ExchangeStock(int stockId, int quantity)
        {
            try
            {
                int current = _currentId;       // Logged in users´s id
                Orders order = new Orders();
                // Gets the correct stock based on stockId from client:
                Stock stock = await _db.Stocks.
                    FirstOrDefaultAsync(s => s.Id == stockId);
                // Gets  the correct user based on current
                Users user = await _db.Users.
                    FirstOrDefaultAsync(u => u.Id == current);

                if (user != null && stock != null)
                {
                    order.Stock = stock;
                    order.User = user;
                }
                order.Quantity = quantity;
                order.Date = DateTime.Now;

                _db.Orders.Add(order);      /* Adds the spesific order to the
                                             * Orders table*/
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
                return false;
            }
        }

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

        // Creates password salt.
        public static byte[] CreateSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        // Creates password hash. Used by registerUser and login methods
        public static byte[] createHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
            numBytesRequested: 32);
        }

        // Checks if username exists. Gets called on from controller
        public async Task<bool> CheckUsername(User user)
        {
            try
            {
                if (await _db.Users.Where(u => u.Username == user.Username).
                    ToListAsync() != null) { return true; }
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
                Users user = await _db.Users.
                    FirstOrDefaultAsync(u => u.Username == username);
                byte[] hash = createHash(password, user.Salt);
                bool ok = hash.SequenceEqual(user.Password);    // Check pswd
                if (ok)
                {
                    _currentId = user.Id;  // Updates to the logged in user´s ID
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