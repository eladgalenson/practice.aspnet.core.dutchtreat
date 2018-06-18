using DutchTreat.Data.Entities;
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

        public IEnumerable<Product> GetProducts()
        {
            this._logger.LogInformation("Call to GetProducts")

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
    }
}
