using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectShopManagment.Startup))]
namespace ProjectShopManagment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
