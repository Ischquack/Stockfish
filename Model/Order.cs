using System;
using System.Collections.Generic;

namespace Stockfish.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }

        public virtual User User { get; set; }

        public virtual Stock Stock { get; set; }

        /*public Order (string date, int quantity, User user, Stock stock)
        {
            Date = date;
            Quantity = quantity;
            User = user;
            Stock = stock;
        }*/
    }

}

