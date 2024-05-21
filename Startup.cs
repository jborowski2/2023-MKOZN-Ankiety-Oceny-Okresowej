using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OOP.Startup))]
namespace OOP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
