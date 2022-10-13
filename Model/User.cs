using System;
namespace Stockfish.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }

        public string PostCode { get; set; }
        public string PostArea { get; set; }
    }
}

