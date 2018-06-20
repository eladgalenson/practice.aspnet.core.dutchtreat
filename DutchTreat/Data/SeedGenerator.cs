using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class SeedGenerator
    {
        private DutchContext _context { get; set; }
        private IHostingEnvironment _hosting { get; set; }
        private UserManager<StoreUser> _userManager { get; set; }

        public SeedGenerator(DutchContext context, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            this._context = context;
            this._hosting = hosting;
            this._userManager = userManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            var user = await this._userManager.FindByEmailAsync("elad@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "elad",
                    LastName = "last",
                    UserName = "eladlast",
                    Email = "elad@gmail.com"
                };

                var result = await this._userManager.CreateAsync(user, "123456aB!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_context.Products.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "12345",
                    User = user,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product =  products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            
        }
    }
}
