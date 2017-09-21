using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PolishWarehouse.Startup))]
namespace PolishWarehouse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
