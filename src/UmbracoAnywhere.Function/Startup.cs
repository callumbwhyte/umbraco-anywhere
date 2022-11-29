using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

[assembly: FunctionsStartup(typeof(UmbracoAnywhere.Function.Startup))]

namespace UmbracoAnywhere.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddUmbraco(builder.GetContext().Configuration)
                .AddConfiguration()
                .AddUmbracoCore()
                .AddHostingEnvironment<DefaultHostingEnvironment>() // this maps the file system
                .AddComposers()
                .Build();
        }
    }
}