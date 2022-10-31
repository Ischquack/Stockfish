using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Stockfish.Model;

namespace Stockfish.DAL
{
    /* To configure foreign key for Postal Code we have to create two new 
     * classes Users and PostOffices:
     */
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
        public int? Admin { get; set; }
    }

    public class PostOffices
    {
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.
            DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? PostalCode { get; set; }
        public string? PostalOffice { get; set; }
    }

    /* An order needs to contain information about which user and which stock
     * that was involved. This is how we can get information about which stocks
     * each user has bought (Used in the GetUserStocks method). 
     */
    public class Orders
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        virtual public Users User { get; set; }

        virtual public Stock Stock { get; set; }
    }

    // This class handles the creation of the tables in Stocks.db. 
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext>
            options) : base(options)
        {
            Database.EnsureCreated();
        }
        // The tables gets created here:
        public DbSet<Users> Users { get; set; }
        public DbSet<PostOffices> PostOffices { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Orders> Orders { get; set; }

        // Activates Lazy Loading
        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

