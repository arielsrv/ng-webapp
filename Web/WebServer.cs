using Core.Shared.Errors;
using Core.Shared.Users.Application;
using Core.Users.Application;
using Core.Users.Domain;
using Core.Users.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Web;

public static class WebServer
{
    public static void Run(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "NgWebApp", Version = "v1" });
        });

        builder.Services.AddHttpClient<IUserRepository, UserHttpRepository>();
        builder.Services.AddSingleton<IUserQuery, UserQuery>();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NgWebApp v1"));

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseMiddleware<ErrorHandlerMiddleware>();
        
        app.MapControllerRoute(
            "default",
            "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}