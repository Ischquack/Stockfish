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
        private const string _loggedIn = "loggedIn";


        public StockController(IStockRepo db, ILogger<StockController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> GetAllStocks()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            List<Stock> allStocks = await _db.GetAllStocks();
            return Ok(allStocks);
        }

        public async Task<ActionResult> RegisterUser(User user)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (await _db.CheckUsername(user))
            {
                if (await _db.RegisterUser(user))
                {
                    return Ok("User registered, you can now log in");
                } else
                {
                    return BadRequest("Oops, something went wrong! Please try again later");
                }
                
            } else
            {
                return BadRequest("Username already taken, please choose a different username");
            }
        }

        public async Task<ActionResult> Login(string username, string password)
        {
            await _db.Login(username, password);
        }
    }
}