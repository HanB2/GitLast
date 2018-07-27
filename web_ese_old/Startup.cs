using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_ese_old.Startup))]
namespace web_ese_old
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
