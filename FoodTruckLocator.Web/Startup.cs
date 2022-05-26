using FoodTruckLocator.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckLocator.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
          .AddAzureAD(options => Configuration.Bind("AzureAd", options));

      services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, opt =>
      {
        var resourceUri = new Uri(Configuration["DownstreamApi:BaseUrl"]);
        var resource = $"{resourceUri.Scheme}://{resourceUri.Host}/";
        opt.ResponseType = "code";
        opt.SaveTokens = true;
        opt.Scope.Add("user_impersonation");
        opt.Scope.Add(Configuration["DownstreamApi:Scopes"]);
        opt.Resource = Configuration["DownstreamApi:Resource"];
      });

      services.AddControllersWithViews(options =>
      {
        var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
      });

      services.AddCors(o =>
                  {
                    o.AddPolicy(name: "MyCorsPolicy", policy =>
              {
                policy.WithOrigins("*");
              });
                  });

      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddHttpClient();
      services.AddScoped<TokenProvider>();
      services.AddScoped<HttpClientService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
