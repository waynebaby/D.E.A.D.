using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleMyBusinessSolution1.Portal.Startup))]
namespace SampleMyBusinessSolution1.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
