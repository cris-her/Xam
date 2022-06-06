using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(democsbackendService.Startup))]

namespace democsbackendService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}