using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace DiscordBotApi.Filters
{
    public class ApiRateLimiter: IAsyncActionFilter
    {

        // zu übungszwecken hier drin
        private readonly IMemoryCache _cache;
        
        public ApiRateLimiter(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"{ipAddress}_RateLimit";

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            // Setze die Rate-Limit-Einstellungen (z.B. 100 Anfragen alle 5 Minuten)
            var limit = 50;
            var expirationPeriod = TimeSpan.FromMinutes(5);

            if (requestCount >= limit)
            {
                context.HttpContext.Response.StatusCode = 429; // Zu viele Anfragen
                await context.HttpContext.Response.WriteAsJsonAsync("Rate limit exceeded.");
                context.Result = new StatusCodeResult(429);
                return;
            }

            // Inkrementiere den Zähler und aktualisiere den Cache
            _cache.Set(cacheKey, requestCount + 1, expirationPeriod);

            // Führe die nächste Middleware im Pipeline aus
            await next();
        }
    }
}
