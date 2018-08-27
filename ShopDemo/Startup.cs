using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopDemo.Startup))]
namespace ShopDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
