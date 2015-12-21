using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PSMS_Beheer.Startup))]
namespace PSMS_Beheer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
