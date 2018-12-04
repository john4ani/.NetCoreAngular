using AgileTrackingSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileTrackingSystem.Data
{
    public class DBSeeder
    {
        private readonly DBContext _dBContext;
        private readonly UserManager<User> _userManager;

        public DBSeeder(DBContext dBContext, UserManager<User> userManager)
        {
            _dBContext = dBContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _dBContext.Database.EnsureCreated();

            User user = await _userManager.FindByEmailAsync("test@test.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "test",
                    LastName = "test",
                    Email = "test@test.com",
                    UserName = "test@test.com"
                };
                var result = await _userManager.CreateAsync(user,"P@ssw0rd!");
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Coulld not create user in the Seeder");
            }

            if (!_dBContext.Products.Any())
            {
                var orders = new List<Order>
                {
                    new Order{OrderNumber="12345",OrderDate=DateTime.Now}
                };
                _dBContext.AddRange(orders);
                var products = new List<Product> {
                    new Product{Price=2,Title="a"},
                    new Product{Price=3,Title="b"},
                    new Product{Price=4,Title="c"},
                };
                _dBContext.AddRange(products);
                var order = _dBContext.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }
                _dBContext.SaveChanges();
            }
        }
    }
}
