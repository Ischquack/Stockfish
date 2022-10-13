using System;
namespace Stockfish.Model
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantum { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual Order Order { get; set; }
    }
}

