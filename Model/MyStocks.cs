namespace Stockfish.Model
{
    /* Model class that gets returned to a logged in user in sellStocks.html
     The user gets information about which and how many stocks he owns. */
    public class MyStocks
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public double Turnover { get; set; }
        public double Diff { get; set; }
        public int Quantity { get; set; }
    }
}
