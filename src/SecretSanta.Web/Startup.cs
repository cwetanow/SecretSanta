﻿using System;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Factory;
using Ninject.Infrastructure.Disposal;
using SecretSanta.Authentication;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Common;
using SecretSanta.Data;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Infrastructure.Extensions;
using SecretSanta.Providers.Contracts;

namespace SecretSanta.Web
{
    public class Startup
    {
        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        private IKernel kernel;
        private IServiceProvider provider;

        private object Resolve(Type type) => this.kernel.Get(type);
        private object RequestScope(IContext context) => scopeProvider.Value;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SecretSantaContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SecretSantaContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IDbContext, SecretSantaContext>();
            services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer((config) =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration[Constants.TokenIssuer],
                        ValidAudience = Configuration[Constants.TokenIssuer],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[Constants.TokenKey]))
                    };
                });


            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());

            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            this.kernel = this.RegisterApplicationComponents(app);
            this.provider = provider;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }

        private IKernel RegisterApplicationComponents(IApplicationBuilder app)
        {
            var kernel = new StandardKernel();

            kernel.Load(new FuncModule());

            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf()
                    .InScope(RequestScope);
            }

            // Factories
            kernel.Bind<IUserFactory>().ToFactory()
                .InSingletonScope();

            kernel.Bind<IDtoFactory>().ToFactory()
                .InSingletonScope();

            // Authentication
            kernel.Bind<IAuthenticationProvider>()
                .ToMethod((context => this.Get<IAuthenticationProvider>()))
                .InScope(RequestScope);

            // Data
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>()
                .InScope(RequestScope);

            // Cross-wire required framework services
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            return kernel;
        }

        private T Get<T>()
        {
            return (T)this.provider.GetService(typeof(T));
        }

        private sealed class Scope : DisposableObject { }
    }
}
