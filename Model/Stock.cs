using System;
namespace Stockfish.Model
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Turnover { get; set; }
        public double Diff { get; set; }

        public Stock (string name, double price, double turnover, double diff)
        {
            Name = name;
            Price = price;
            Turnover = turnover;
            Diff = diff;
        }
    }
}

