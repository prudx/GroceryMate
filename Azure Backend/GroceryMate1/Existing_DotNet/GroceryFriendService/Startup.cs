using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GroceryFriendService.Startup))]

namespace GroceryFriendService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}