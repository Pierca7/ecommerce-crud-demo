using ShopDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.DataAccess
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetByOwner(string user);
        void UpdateCartItems(Cart cart);
        Cart CreateCart(string user);
        void RemoveAllCartItemsByProductId(int id);
        void RemoveCartItem(int id);
    }
}
