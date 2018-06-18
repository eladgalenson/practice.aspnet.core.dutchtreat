using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository   
    {
        IEnumerable<Product> GetProducts();
        bool SaveAll();
    }
}