
using DiscordBotApi.Filters;
using DiscordBotApi.Imageverwalter;
using DiscordBotApi.Models;
using DiscordBotApi.services;
using DiscordBotApi.services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Supabase;
using System;
using System.Reflection;

namespace DiscordBotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");
           
            var url = builder.Configuration["SUPABASE_URL"];
            var key = builder.Configuration["SUPABASE_KEY"];
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
                // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
            };
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<PostgresContext>(options => options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));

          

            builder.Services.AddScoped<IRepName, RepName>();
            builder.Services.AddScoped<ApiRateLimiter>();
            builder.Services.AddScoped<ApiKeyAuth>();

            builder.Services.AddScoped<ChatTextLogic>();
            builder.Services.AddScoped<IOptionalInfo, OptionalInfo>();
            builder.Services.AddScoped(provider => new Client(url, key, options));
            builder.Services.AddScoped<ImageDownloader>();
            builder.Services.AddScoped<ICustomMemePicture ,CustomMemePicture>();
            builder.Services.AddHttpClient();
            

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Deine API", Version = "v1",Description= "Custom APi für DiscordBot" });

                c.DocumentFilter<SwaggerSecurityRequirementsDocumentFilter>(); // Füge hier den Filter hinzu
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                Console.WriteLine(xmlFile);
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               // c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

            });


            builder.Services.AddLogging(loginbuilder =>
            {
                //loginbuilder.SetMinimumLevel(LogLevel.Debug);
                loginbuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
                loginbuilder.AddConsole();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
      
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseReDoc(options =>
            {
                options.DocumentTitle = "Swagger Demo Documentation";
                options.SpecUrl = "/swagger/v1/swagger.json";
            });
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            //für komplettes Projekt und nicht nur einzelne Api routes
            //app.UseMiddleware<ApiKeyAuth>();

            app.MapControllers();

            


            app.Run();
        }
    }
}
