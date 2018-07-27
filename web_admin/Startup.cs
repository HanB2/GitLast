using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_admin.Startup))]
namespace web_admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
