using ShopDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDemo.DataAccess
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByType(string type);
        IEnumerable<Product> GetProductsByName(string text);
        void UpdateProduct(Product EditedProduct);
    }
}