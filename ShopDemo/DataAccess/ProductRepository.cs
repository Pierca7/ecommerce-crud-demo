
using ShopDemo.Models;
using ShopDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ShopDemo.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public ApplicationDbContext db
        {
            get
            {
                return Context as ApplicationDbContext;
            }
        }

        public IEnumerable<Product> GetProductsByName(string text)
        {
            return db.Products.Where(x => x.Name.Contains(text));
        }

        public IEnumerable<Product> GetProductsByType(string type)
        {
            if (String.IsNullOrEmpty(type)) return db.Products.ToList();
            else return db.Products.Where(x => x.Type == type);
        }

        public void UpdateProduct(Product EditedProduct)
        {
            var product = db.Products.FirstOrDefault(x => x.ID == EditedProduct.ID);
            db.Entry(product).CurrentValues.SetValues(EditedProduct);
        }
    }
}