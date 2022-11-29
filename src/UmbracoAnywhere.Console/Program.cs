using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.DependencyInjection;

namespace UmbracoAnywhere.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddUmbraco(context.Configuration)
                        .AddConfiguration()
                        .AddUmbracoCore()
                        .AddComposers()
                        .Build();
                })
                .Build();

            var contentService = host.Services.GetService<IContentService>();

            var items = contentService.GetRootContent()
                .Select(x => x.Name);

            foreach (var item in items)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}