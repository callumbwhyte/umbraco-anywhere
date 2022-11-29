using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.DependencyInjection;

namespace UmbracoAnywhere.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // workaround as Maui doesn't support appsetting.json
            var appsettingsManifest = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("appsettings.json"));

            if (appsettingsManifest != null)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appsettingsManifest))
                {
                    var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

                    builder.Configuration.AddConfiguration(config);
                }
            }

            // workaround as Maui doesn't support DI in controls yet
            builder.Services.AddTransient<MainPage>();

            // build Umbraco
            builder.Services
                .AddUmbraco(builder.Configuration)
                .AddConfiguration()
                .AddUmbracoCore()
                .AddHostingEnvironment<MauiHostingEnvironment>() // this maps the file system for Maui
                .AddComposers()
                .Build();

            return builder.Build();
        }
    }
}