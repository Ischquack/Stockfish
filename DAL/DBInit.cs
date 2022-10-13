using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stockfish.DAL;

namespace Stockfish.Model
{
    public class DBInit
    {
        public static void init(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<StockContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var norwegian = new Stock { Name = "Norwegian", Price = 1000, Value = 100000 };
                var orkla = new Stock { Name = "Orkla", Price = 145, Value = 3000 };

                context.Stocks.Add(norwegian);
                context.Stocks.Add(orkla);

                context.SaveChanges();
            }
        }
    }
}

