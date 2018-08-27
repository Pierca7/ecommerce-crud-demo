using ShopDemo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDemo.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public IProductRepository Products { get; private set; }
        public ICartRepository Carts { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            db = context;
            Products = new ProductRepository(db);
            Carts = new CartRepository(db);
        }

        public void Complete()
        {
            db.SaveChanges();
            Dispose();
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }




    }
}