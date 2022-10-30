using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Stockfish.Model;

namespace Stockfish.DAL
{
    
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        virtual public PostOffices? PostOffice { get; set; }
        public string? Username { get; set; }
        public byte[]? Password { get; set; }
        public byte[]? Salt { get; set; }
    }

    public class PostOffices
    {
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? PostalCode { get; set; }
        public string? PostalOffice { get; set; }
    }

    public class Orders
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        public virtual Users User { get; set; }

        public virtual Stock Stock { get; set; }
    }




    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<PostOffices> PostOffices { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

