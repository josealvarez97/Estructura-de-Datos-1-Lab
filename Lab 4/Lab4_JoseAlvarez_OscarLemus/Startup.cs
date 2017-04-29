using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab4_JoseAlvarez_OscarLemus.Startup))]
namespace Lab4_JoseAlvarez_OscarLemus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
