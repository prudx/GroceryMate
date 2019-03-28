using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(grocerypalService.Startup))]

namespace grocerypalService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}