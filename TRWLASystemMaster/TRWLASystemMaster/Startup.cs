using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TRWLASystemMaster.Startup))]
namespace TRWLASystemMaster
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
