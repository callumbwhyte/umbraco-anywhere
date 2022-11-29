using System;
using System.Data.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Infrastructure.DependencyInjection;
using Umbraco.Cms.Infrastructure.Migrations.Install;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Persistence.SqlSyntax;
using Umbraco.Extensions;

namespace UmbracoAnywhere
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddUmbraco(this IServiceCollection services, IConfiguration configuration)
        {
            StaticApplicationLogging.Initialize(new SerilogLoggerFactory());

            var loggerFactory = new SerilogLoggerFactory();

            var typeLoader = services.AddTypeLoader(Assembly.GetEntryAssembly(), loggerFactory, configuration);

            return new UmbracoBuilder(services, configuration, typeLoader);
        }

        public static TypeLoader AddTypeLoader(this IServiceCollection services, Assembly entryAssembly, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            TypeFinderSettings typeFinderSettings =
                configuration.GetSection(Constants.Configuration.ConfigTypeFinder).Get<TypeFinderSettings>() ??
                new TypeFinderSettings();

            var assemblyProvider = new DefaultUmbracoAssemblyProvider(
                entryAssembly,
                loggerFactory,
                typeFinderSettings.AdditionalEntryAssemblies);

            var typeFinderConfig = new TypeFinderConfig(Options.Create(typeFinderSettings));

            var typeFinder = new TypeFinder(
                loggerFactory.CreateLogger<TypeFinder>(),
                assemblyProvider,
                typeFinderConfig);

            var typeLoader = new TypeLoader(typeFinder, loggerFactory.CreateLogger<TypeLoader>());

            services.TryAddSingleton<ITypeFinder>(typeFinder);

            services.TryAddSingleton(typeLoader);

            return typeLoader;
        }

        public static IUmbracoBuilder AddUmbracoCore(this IUmbracoBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Add ASP.NET specific services
            /*builder.Services.AddUnique<IBackOfficeInfo, AspNetCoreBackOfficeInfo>();
            builder.Services.AddUnique<IHostingEnvironment>(sp =>
                ActivatorUtilities.CreateInstance<AspNetCoreHostingEnvironment>(
                    sp,
                    sp.GetRequiredService<IApplicationDiscriminator>()));

            builder.Services.AddHostedService(factory => factory.GetRequiredService<IRuntime>());*/

            builder.Services.AddSingleton<DatabaseSchemaCreatorFactory>();
            builder.Services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IDatabaseProviderMetadata, CustomConnectionStringDatabaseProviderMetadata>());

            // Must be added here because DbProviderFactories is netstandard 2.1 so cannot exist in Infra for now
            builder.Services.AddSingleton<IDbProviderFactoryCreator>(factory => new DbProviderFactoryCreator(
                DbProviderFactories.GetFactory,
                factory.GetServices<ISqlSyntaxProvider>(),
                factory.GetServices<IBulkSqlInsertProvider>(),
                factory.GetServices<IDatabaseCreator>(),
                factory.GetServices<IProviderSpecificMapperFactory>(),
                factory.GetServices<IProviderSpecificInterceptor>()));

            builder.AddCoreInitialServices();
            builder.AddTelemetryProviders();

            // aspnet app lifetime mgmt
            /*builder.Services.AddUnique<IUmbracoApplicationLifetime, AspNetCoreUmbracoApplicationLifetime>();
            builder.Services.AddUnique<IApplicationShutdownRegistry, AspNetCoreApplicationShutdownRegistry>();
            builder.Services.AddTransient<IIpAddressUtilities, IpAddressUtilities>();*/

            return builder;
        }

        public static IUmbracoBuilder AddHostingEnvironment<T>(this IUmbracoBuilder builder)
            where T : class, IHostingEnvironment
        {
            builder.Services.AddUnique<IHostingEnvironment, T>();

            return builder;
        }
    }
}