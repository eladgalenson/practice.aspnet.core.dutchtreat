using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private DutchContext context { get; set; }
        private ILogger<DutchRepository> _logger { get; set; }

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this._logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            this._logger.LogInformation("Call to GetAllProducts");

            try
            {
                return this.context.Products.OrderBy(p => p.Title);
            }
            catch(Exception exc)
            {
                this._logger.LogError($"problem found with excption message: {exc.Message}");
                return null;
            }
            
            
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            this._logger.LogInformation($"Call to GetAllOrders");

            try
            {
                if(includeItems)
                {
                    return this.context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
                }
                else
                {
                    return this.context.Orders.ToList();
                }
                
            }
            catch (Exception exc)
            {
                this._logger.LogError($"problem found with excption message: {exc.Message}");
                
            }
            return null;
        }

        public IEnumerable<Order> GetAllOrdersByName(string userName, bool includeItems)
        {
            this._logger.LogInformation($"Call to GetAllOrdersByName");

            try
            {
                if (includeItems)
                {
                    return this.context.Orders.Where(o=>o.User.UserName == userName).Include(o => o.Items).ThenInclude(i => i.Product).ToList();
                }
                else
                {
                    return this.context.Orders.Where(o => o.User.UserName == userName).ToList();
                }

            }
            catch (Exception exc)
            {
                this._logger.LogError($"problem found with excption message: {exc.Message}");

            }
            return null;

        }

       

        public void AddEntity(object model)
        {
            this._logger.LogInformation($"Call to AddEntity");

            try
            {
                this.context.Add(model);
            }
            catch (Exception exc)
            {
                this._logger.LogError($"problem found with excption message: {exc.Message}");
            }
        }
        
        public Order GetOrderById(string name, int orderId)
        {
            this._logger.LogInformation($"Call to GetOrderByName");

            try
            {
                return this.context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).Where(o => o.Id == orderId && o.User.UserName == name).FirstOrDefault();
            }
            catch (Exception exc)
            {
                this._logger.LogError($"problem found with excption message: {exc.Message}");

            }
            return null;
        }
    }
}
