using AgileTrackingSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AgileTrackingSystem.Data
{
    public class DBRepository : IDBRepository
    {
        private readonly DBContext _dBContext;

        public DBRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public void AddEntity(object model)
        {
            _dBContext.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            foreach (var item in newOrder.Items)
            {
                item.Product = _dBContext.Products.Find(item.Product.Id);
            }
            AddEntity(newOrder);
        }

        public Order GetAllOrderById(string username, int id)
        {
            return _dBContext.Orders
                             .Include(o => o.Items)
                             .ThenInclude(i => i.Product)
                             .Where(o=>o.Id == id && o.User.UserName == username)
                             .FirstOrDefault(); 
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if(includeItems)
                return _dBContext.Orders
                                 .Include(o=>o.Items)
                                 .ThenInclude(i=>i.Product)
                                 .ToList();

            return _dBContext.Orders
                                 .ToList();
        }

        public IEnumerable<Order> GetAllOrdersByUserName(string username, bool includeItems)
        {
            if (includeItems)
                return _dBContext.Orders
                                 .Where(o=>o.User.UserName == username)
                                 .Include(o => o.Items)
                                 .ThenInclude(i => i.Product)
                                 .ToList();

            return _dBContext.Orders
                             .Where(o => o.User.UserName == username)
                             .ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _dBContext.Products
                .OrderBy(p => p.Title)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _dBContext.Products
                .OrderBy(p => p.Category == category)
                .ToList();
        }

        public bool SaveAll()
        {
            return _dBContext.SaveChanges() > 0;
        }
    }
}
