using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.DataAccess
{
    interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        void Complete();
    }
}
