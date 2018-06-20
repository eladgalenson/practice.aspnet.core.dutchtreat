using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository   
    {
        IEnumerable<Product> GetAllProducts();

        IEnumerable<Order> GetAllOrders(bool includeItems);

        IEnumerable<Order> GetAllOrdersByName(string userName, bool includeItems);

        Order GetOrderById(string userName, int id);

        bool SaveAll();
        void AddEntity(object model);
        
    }
}