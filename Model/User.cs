using System;
namespace Stockfish.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PostalOffice { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

