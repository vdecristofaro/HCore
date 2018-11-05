﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HCore.Web.Middleware;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Routing;
using HCore.Web.Providers.Impl;
using HCore.Web.Providers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HCore.Web.Startup
{
    public abstract class Startup
    {
        private bool _useHttps;
        private int _port;
        private bool _useSpa;
        private bool _useSpaStaticFiles;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        protected virtual void ConfigureCoreServices(IServiceCollection services)
        {

        }

        protected virtual void ConfigureCore(IApplicationBuilder app)
        {

        }

        protected virtual void ConfigureCoreRoutes(IRouteBuilder routes)
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureHttpContextAccessor(services);
            ConfigureLocalization(services);
            ConfigureUrlHelper(services);
            ConfigureWebServer(services);
            ConfigureMvc(services);

            ConfigureGenericServices(services);

            ConfigureCoreServices(services);            
        }

        private void ConfigureHttpContextAccessor(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        private void ConfigureLocalization(IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddCoreTranslations();
        }

        private void ConfigureUrlHelper(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }        

        private void ConfigureWebServer(IServiceCollection services)
        {
            bool useWeb = Configuration.GetValue<bool>("UseWeb");

            if (useWeb)
            {
                // configure cookie policies

                services.Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.

                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                });
            }

            _useHttps = Configuration.GetValue<bool>("WebServer:UseHttps");
            _port = Configuration.GetValue<int>("WebServer:WebPort");

            if (_useHttps)
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(60);
                });

                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = _port;
                });              
            }

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            _useSpaStaticFiles = Configuration.GetValue<bool>("WebServer:UseSpaStaticFiles");

            if (_useSpaStaticFiles)
            {
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/build";
                });
            }
        }        

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureCore(app);

            ConfigureLogging(app, env);
            ConfigureLocalization(app, env);
            ConfigureHttps(app, env);
            ConfigureResponseCompression(app, env);
            ConfigureStaticFiles(app, env);
            ConfigureCsp(app, env);
            ConfigureRequestLocalization(app, env);
            ConfigureExceptionHandling(app, env);            

            ConfigureMvc(app, env);

            ConfigureSpa(app, env);
        }

        private void ConfigureLogging(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();            
        }

        private void ConfigureLocalization(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCoreTranslations();
        }

        public void ConfigureHttps(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_useHttps)
            {
                if (!env.IsDevelopment())
                    app.UseHsts();

                app.UseHttpsRedirection();
            }
        }

        private void ConfigureResponseCompression(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
        }

        private void ConfigureStaticFiles(IApplicationBuilder app, IHostingEnvironment env)
        {
            var staticFileOptions = new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    var cacheControlHeaderValue = new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };

                    context.Context.Response.GetTypedHeaders().CacheControl = cacheControlHeaderValue;
                }
            };

            app.UseStaticFiles(staticFileOptions);

            if (_useSpaStaticFiles)
            {
                app.UseSpaStaticFiles(staticFileOptions);
            }
        }

        private void ConfigureCsp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<CspHandlingMiddleware>(); 
        }

        private void ConfigureRequestLocalization(IApplicationBuilder app, IHostingEnvironment env)
        {
            var englishCultureInfo = new CultureInfo("en");
            var germanCultureInfo = new CultureInfo("de");

            var cultures = new CultureInfo[] { englishCultureInfo, germanCultureInfo };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(englishCultureInfo),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });
        }

        private void ConfigureExceptionHandling(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<UnhandledExceptionHandlingMiddleware>();
        }

        private void ConfigureMvc(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                ConfigureCoreRoutes(routes);

                if (_useSpa)
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                }
            });        
        }        

        private void ConfigureSpa(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_useSpa)
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";

                    bool useSpaDevelopmentServer = Configuration.GetValue<bool>("WebServer:UseSpaDevelopmentServer");

                    if (useSpaDevelopmentServer && env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            }
        }

        private void ConfigureGenericServices(IServiceCollection services)
        {
            services.AddScoped<IUrlProvider, UrlProviderImpl>();
            services.AddScoped<INowProvider, NowProviderImpl>();
        }
    }
}
