using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KBCC.Startup))]
namespace KBCC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
