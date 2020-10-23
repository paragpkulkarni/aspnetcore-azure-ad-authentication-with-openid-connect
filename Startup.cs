using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;

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
				options.Authority = "https://login.microsoftonline.com/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
				//Application (client) ID
				options.ClientId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
				options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
				options.ResponseType = "code";
				options.CallbackPath = "/index.html";
				options.SaveTokens = true;

			});
			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});
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

			//app.UseStaticFiles(new StaticFileOptions
			//{
			//	OnPrepareResponse = ctx =>
			//	{
			//		if (!ctx.Context.User.Identity.IsAuthenticated)
			//		{
			//			// Can redirect to any URL where you prefer.
			//			ctx.Context.Response.ContentType = "text/html";
			//			//ctx.Context.Response.StatusCode = 401;
			//			var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("You are not authoirzed to view this page"));
			//			ctx.Context.Response.Body = Stream.Null;					
			//			//ctx.Context.Response.Redirect("UnAuthoirzed.html");					

			//		}
			//	}
			//});


			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
			

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
				 Path.Combine(env.ContentRootPath, "angular-app")),
				RequestPath="/help"
				
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
