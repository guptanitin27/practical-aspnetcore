using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace PracticalAspNetCore 
{
    interface ICustomFeature 
    {
        string Greetings {get;}
    }

    public class CustomFeature : ICustomFeature
    {
        public string  Greetings => "This is my custom feature set from previous middleware";
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next ) =>{
                context.Features.Set<ICustomFeature>(new CustomFeature());
                await next.Invoke();
            });

            app.Run(context =>
            {
                var custom = context.Features.Get<ICustomFeature>();
                if (custom == null)
                    return context.Response.WriteAsync($"Custom is null");
                else
                    return context.Response.WriteAsync($"{custom.Greetings}");
            });
        }
    }
    
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}