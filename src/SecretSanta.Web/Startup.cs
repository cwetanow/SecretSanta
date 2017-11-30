using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Factory;
using Ninject.Infrastructure.Disposal;
using SecretSanta.Authentication;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Data;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Web.Infrastructure.Extensions;

namespace SecretSanta.Web
{
    public class Startup
    {
        private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
        private IKernel kernel;

        private object Resolve(Type type) => this.kernel.Get(type);
        private object RequestScope(IContext context) => scopeProvider.Value;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());

            services.AddCustomControllerActivation(Resolve);
            services.AddCustomViewComponentActivation(Resolve);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            this.kernel = this.RegisterApplicationComponents(app, provider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private IKernel RegisterApplicationComponents(IApplicationBuilder app, IServiceProvider provider)
        {
            var kernel = new StandardKernel();

            kernel.Load(new FuncModule());

            foreach (var ctrlType in app.GetControllerTypes())
            {
                kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
            }

            // Factories
            kernel.Bind<IUserFactory>().ToFactory().InSingletonScope();

            // Authentication
            kernel.Bind<IAuthenticationProvider>()
                .ToMethod((context => this.Get<IAuthenticationProvider>(provider)))
                .InScope(RequestScope);

            // Cross-wire required framework services
            kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

            return kernel;
        }

        private T Get<T>(IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        private sealed class Scope : DisposableObject { }
    }
}
