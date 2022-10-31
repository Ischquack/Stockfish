namespace Stockfish.Model
{
    /* Model class that matches  */
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
