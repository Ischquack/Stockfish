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
        private ILogger<StockController> _log;
        private const string _loggedIn = "";


        public StockController(IStockRepo db, ILogger<StockController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> GetAllStocks()
        {
            List<Stock> allStocks = await _db.GetAllStocks();
            return Ok(allStocks);
        }

        public async Task<ActionResult> RegisterUser(User user)
        {
            if (await _db.CheckUsername(user))
            {
                if (await _db.RegisterUser(user))
                {
                    return Ok("User registered, you can now log in");
                }
                else
                {
                    return BadRequest("Oops, something went wrong! Please try again later");
                }

            }
            else
            {
                return BadRequest("Username already taken, please choose a different username");
            }
        }

        public async Task<ActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid) 
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
            return BadRequest("Inputvalidation from server not succesful");   
        }

        public void LogOut()
        {
            HttpContext.Session.SetString(_loggedIn, "");
        }

        public async Task<ActionResult> BuyStock(int StockId, int Quantity)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.BuyStock(StockId, Quantity)) { return Ok("Stock succesfully purchased"); }
            else { return BadRequest("Something went wrong during purchase"); }
        }

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

        public async Task<ActionResult> GetUserStocks()
        {
            List<Orders> myStocks = await _db.GetUserStocks();
            return Ok(myStocks);
        }
    }
}