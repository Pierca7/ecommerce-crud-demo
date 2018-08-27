using ShopDemo.Models;
using ShopDemo.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopDemo.DataAccess
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private UserManager<ApplicationUser> UserManager;

        public CartRepository(DbContext context) : base(context)
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ApplicationDbContext db
        {
            get
            {
                return Context as ApplicationDbContext;
            }
        }

        public Cart CreateCart(string user)
        {
            var cart = new Cart(user);
            UserManager.FindByName(user).Cart = cart;
            return cart;
        }

        public Cart GetByOwner(string user)
        {
            return UserManager.FindByName(user).Cart;
        }

        public void RemoveAllCartItemsByProductId(int id)
        {
            var items = db.CartItems.Where(x => x.Product.ID == id).ToList();
            db.CartItems.RemoveRange(items);
        }

        public void RemoveCartItem(int id)
        {
            var item = db.CartItems.FirstOrDefault(x => x.CartItemID == id);
            db.CartItems.Remove(item);
        }

        public void UpdateCartItems(Cart cart)
        {
            var UserCart = db.Carts.FirstOrDefault(x => x.Owner == cart.Owner);

            // Update parent
            db.Entry(UserCart).CurrentValues.SetValues(cart);

            // Delete children
            foreach (var existingItem in UserCart.CartItems.ToList())
            {
                if (!cart.CartItems.Any(c => c.CartItemID == existingItem.CartItemID))
                    db.CartItems.Remove(existingItem);
            }

            foreach (var item in cart.CartItems)
            {
                var existingItem = UserCart.CartItems.Where(c => c.CartItemID == item.CartItemID).FirstOrDefault();

                if (existingItem != null)
                    // Update child
                    db.Entry(existingItem).CurrentValues.SetValues(item);
                else
                {
                    // Insert child
                    var newItem = item;

                    UserCart.CartItems.Add(newItem);
                }
            }
        }
    }
}