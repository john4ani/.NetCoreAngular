using System.Collections.Generic;
using AgileTrackingSystem.Data.Entities;

namespace AgileTrackingSystem.Data
{
    public interface IDBRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUserName(string username, bool includeItems);
        Order GetAllOrderById(string name, int id);

        void AddEntity(object model);

        bool SaveAll();
        void AddOrder(Order newOrder);
    }
}