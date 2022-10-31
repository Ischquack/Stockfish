using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stockfish.DAL;
using Stockfish.Controllers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Serilog;
using System.Security.Cryptography;

namespace Stockfish.Model
{
    public class DBInit
    {
        private readonly StockController _controller;

        public DBInit(StockController controller)
        {
            _controller = controller;
        }

        public static async void init(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<StockContext>();
           
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
        
                var norwegian = new Stock ("Norwegian", 2, 250, -7000);
                var orkla = new Stock ("Orkla", 145, 3000, -340);
                var aaslieCasino = new Stock ("Åslie Casino", 355, 1600000, 16);
                var storefjellHotell = new Stock("Storefjell Hotell", 600, 400000, 8);
                var osloMet = new Stock("OsloMet", 40, 4000000, -23);
                var brann = new Stock("SK Brann", 750, 30000, 60);
                var ranheim = new Stock("Ranheim IL", 20, 2000, -40);
                var utdanningsdirektoret = new Stock("Utdanningsdirektoratet", 5000, 300000000, 44);
                var colorLine = new Stock("Color Line", 30000, 3600000, -33);
                var dde = new Stock("D.D.E", 5000, 4000000, 3);
                var tantrasenteret = new Stock("Tantrasenteret Oslo", 200, 4000, 1);
                var rettInnBar = new Stock("Rett Inn Bar", 1, 35000, -30);
                var kongsbergGruppen = new Stock("Kongsberg Gruppen", 9000, 1500000000000, 780);
                var russland = new Stock("Russland", 0.01, 20000, -3400);
                var sleepyJoe = new Stock("Joe Biden", 3700, 40000000, -370);

                var quackAttack = new User { Firstname = "Quacken", Surname = "Attacken", Address = "Gølle", 
                    PostalCode = "2918", PostalOffice = "Ulnes", Username = "Quacky", Password = "Ranheim123" };

                static byte[] CreateSalt()
                {
                    var csp = new RNGCryptoServiceProvider();
                    var salt = new byte[24];
                    csp.GetBytes(salt);
                    return salt;
                }

                byte[] salt = CreateSalt();

                static byte[] createHash(string password, byte[] salt)
                {
                    return KeyDerivation.Pbkdf2(
                                        password: password,
                                        salt: salt,
                                        prf: KeyDerivationPrf.HMACSHA512,
                                        iterationCount: 1000,
                    numBytesRequested: 32);
                }

                byte[] hash = createHash(quackAttack.Password, salt);

                Users newAdmin = new Users();
                var newPostOffice = new PostOffices();
                newAdmin.Address = quackAttack.Address;
                newAdmin.Firstname = quackAttack.Firstname;
                newAdmin.Surname = quackAttack.Surname;
                newPostOffice.PostalOffice = quackAttack.PostalOffice;
                newPostOffice.PostalCode = quackAttack.PostalCode;
                newAdmin.PostOffice = newPostOffice;
                newAdmin.Username = quackAttack.Username;
                newAdmin.Salt = salt;
                newAdmin.Password = hash;
                newAdmin.Admin = 1;
                context.Add(newAdmin);
                await context.SaveChangesAsync();
               




                context.Stocks.Add(norwegian);
                context.Stocks.Add(orkla);
                context.Stocks.Add(aaslieCasino);
                context.Stocks.Add(storefjellHotell);
                context.Stocks.Add(osloMet);
                context.Stocks.Add(brann);
                context.Stocks.Add(ranheim);
                context.Stocks.Add(utdanningsdirektoret);
                context.Stocks.Add(colorLine);
                context.Stocks.Add(dde);
                context.Stocks.Add(tantrasenteret);
                context.Stocks.Add(rettInnBar);
                context.Stocks.Add(kongsbergGruppen);
                context.Stocks.Add(russland);
                context.Stocks.Add(sleepyJoe);

                await context.SaveChangesAsync();
            }
        }
    }
}

