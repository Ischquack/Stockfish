using System;
using System.ComponentModel.DataAnnotations;

namespace Stockfish.Model
{
    public class User
    {
        
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string Firstname { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]    
        public string Surname { get; set; }
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Za-zÆØÅæøå])[0-9a-zA-ZæøåÆØÅ ]{2,30}$")]
        public string Address { get; set; }
        [RegularExpression(@"^[0-9]{4}$")]
        public string PostalCode { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string PostalOffice { get; set; }
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ]{2,15}$")]
        public string Username { get; set; }
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Za-zÆØÅæøå])[0-9a-zA-ZæøåÆØÅ. \-]{8,}$")]
        public string Password { get; set; }
    }
}

