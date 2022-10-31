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
    [Route("[controller]/[action]")]    // stocks/*method* on client
    public class StockController : ControllerBase
    {
        private readonly IStockRepo _db;
        private ILogger<StockController> _log;
        private const string _loggedIn = "";


        public StockController(IStockRepo db, ILogger<StockController> log)
        {
            _db = db;
            _log = log;
        }

        // Returns all stocks from Stocks table in Stocks.db
        public async Task<ActionResult> GetAllStocks()
        {
            List<Stock> allStocks = await _db.GetAllStocks();
            return Ok(allStocks);
        }

        // Register a  user in Users table in Stocks.db
        public async Task<ActionResult> RegisterUser(User user)
        {
            if (await _db.CheckUsername(user))  // If username exists in db
            {
                if (await _db.RegisterUser(user))
                {
                    return Ok("User registered, you can now log in");
                }
                else
                {
                    return BadRequest("Oops, something went wrong! " +
                        "Please try again later");
                }

            }
            else
            {
                return BadRequest("Username already taken, " +
                    "please choose a different username");
            }
        }

        /* Login method. Returns 1 to client if admin, 0 if regular user and
        an error message if wrong password/username.
        It also changes the session state to "LoggedIn".*/ 
        public async Task<ActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)     // Server validation for User model
            {
                if (await _db.Login(username, password) == 1)
                {
                    HttpContext.Session.SetString(_loggedIn, "LoggedIn");
                    return Ok(1);
                }
                else if (await _db.Login(username, password) == 0) {
                    HttpContext.Session.SetString(_loggedIn, "LoggedIn");
                    return Ok(0);
                } else
                {
                    _log.LogInformation("Login failed for user: " + username);
                    HttpContext.Session.SetString(_loggedIn, "");
                    return Ok("Username and password does not match");
                }
            }
            _log.LogInformation("Inputvalidation unsuccesful");
            return BadRequest("Inputvalidation from server unsuccesful");   
        }

        //  Logout method changes session state from "LoggedIn" to "".
        public void LogOut()
        {
            HttpContext.Session.SetString(_loggedIn, "");
        }

        // Method for buying and selling stocks.
        public async Task<ActionResult> ExchangeStock(int StockId, int Quantity)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.ExchangeStock(StockId, Quantity))
            { return Ok("Exchange successfull"); }
            else { return BadRequest("Something went wrong during exchange"); }
        }

        /* An admin user can delete, uppdate and add stocks to Stocks table.
         The following three methods takes care  of  that.
        The DeleteStock-method deletes based on the Stock ID. The admin has all
        stocks listed in the html including the ID.
         */
        public async Task<ActionResult> DeleteStock(int StockId)
        {
            _log.LogInformation("Controller " + StockId.ToString());
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.DeleteStock(StockId)) { return Ok("Stock succesfully deleted from table"); }
            else
            {
                return BadRequest("Unable to delete stock");
            }
        }

        public async Task<ActionResult> UpdateStock(Stock stock)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.UpdateStock(stock)) { return Ok("Stock succesfully updated"); }
            else
            {
                return BadRequest("Unable to update stock");
            }
        }

        public async Task<ActionResult> AddStock(Stock stock)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.AddStock(stock)) { return Ok("Stock succesfully added"); }
            else
            {
                return BadRequest("Unable to add stock");
            }
        }

        // Returns a list containing all stocks for the spesific user logged in.
        public async Task<ActionResult> GetUserStocks()
        {
            List<MyStocks> myStocks = await _db.GetUserStocks();
            return Ok(myStocks);
        }
    }
}