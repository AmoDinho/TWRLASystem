using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TWRLA_System_Master.Startup))]
namespace TWRLA_System_Master
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
