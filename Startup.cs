using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace aspnetcore_with_openid_connect
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
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
			.AddCookie("Cookies")
			.AddOpenIdConnect("oidc", options =>
			{
				//Directory (tenant) ID
				options.Authority = "https://login.microsoftonline.com/{tenantID}"; 
				//Application (client) ID
				options.ClientId = "";
				options.ClientSecret = ""; 
				options.ResponseType = "code";
				options.RequireHttpsMetadata = false;
				options.CallbackPath = "/auth/openid/return";														
				options.ResponseMode = "form_post";				
				options.Scope.Add("profile");
				options.Scope.Add("offline_access");				
				options.Scope.Add("openid");
				options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
				{
					OnTokenValidated = context =>
					{
						// Access Token
						var accessToken = context.SecurityToken.RawData;

						return Task.CompletedTask;
					},
					OnAuthenticationFailed = context =>
					{
						Console.WriteLine($"Token Authentication failed with error: " + context.Exception.Message);
						return Task.CompletedTask;
					}
				};
			});
			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});
			services.AddHttpClient();
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseDefaultFiles();
			app.UseStaticFiles();			

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();			

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
