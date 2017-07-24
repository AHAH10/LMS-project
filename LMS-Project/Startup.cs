using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LMS_Project.Startup))]
namespace LMS_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
