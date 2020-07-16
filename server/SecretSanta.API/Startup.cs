using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretSanta.API.Infrastructure.Authentication;
using SecretSanta.API.Infrastructure.Middleware;
using SecretSanta.Application.Common.Extensions;
using SecretSanta.Identity.Configuration;
using SecretSanta.Identity.Extensions;
using SecretSanta.Persistence.Extensions;

namespace SecretSanta.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddControllers();

			services
				.AddMvcCore(opts => {
					opts.OutputFormatters.Add(new HttpNoContentOutputFormatter());
					opts.Filters.Add(new ProducesAttribute("application/json"));
					opts.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build()));
				});

			services
				.AddPersistence(options => options.UseSqlite(Configuration.GetConnectionString("SecretSantaDb")))
				.AddApplication()
				.AddIdentity(options => options.UseSqlite(Configuration.GetConnectionString("IdentityDb")));

			var jwtConfiguration = Configuration.GetSection("Authentication").Get<JwtAuthConfiguration>();
			services
				.AddJwtBearerAuthentication(jwtConfiguration)
				.AddJwtTokenAuthentication(config => Configuration.GetSection("Authentication").Bind(config));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseMiddleware<ExceptionHandlerMiddleware>();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
