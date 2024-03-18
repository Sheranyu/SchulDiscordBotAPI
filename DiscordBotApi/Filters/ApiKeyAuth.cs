using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Runtime.CompilerServices;


namespace DiscordBotApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuth : Attribute, IAsyncActionFilter
    {
        
        private readonly int limit = 5;
        private readonly TimeSpan expirationPeriod = TimeSpan.FromMinutes(5);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var apikey = config.GetValue<string>("ApiKey");
            var cacheKey = $"{apikey}_RateLimit";

            await APiWrongLimiter(context, cacheKey, memoryCache);

            if (context.Result != null)
            {
                return;
            }


            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var key))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!key.Equals(apikey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            memoryCache.Set(cacheKey, 0, expirationPeriod);

            await next();
        }

        private async Task APiWrongLimiter(ActionExecutingContext context, string cacheKey, IMemoryCache memoryCache)
        {
             

            if (!memoryCache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            if (requestCount >= limit)
            {
                context.HttpContext.Response.StatusCode = 429; // Zu viele Anfragen
                await context.HttpContext.Response.WriteAsJsonAsync("Rate limit exceeded.");
                context.Result = new StatusCodeResult(429);
                return;
            }
           
            // Inkrementiere den Zähler und aktualisiere den Cache
            memoryCache.Set(cacheKey, requestCount + 1, expirationPeriod);
            return;


        }
    }
}
