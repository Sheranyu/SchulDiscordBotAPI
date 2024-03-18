using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiscordBotApi.Filters
{
    public class SwaggerSecurityRequirementsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Components.SecuritySchemes.Add("ApiKey", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "ApiKey", // Name des Headers oder der Query-Parameter, der den API-Key enthält
                In = ParameterLocation.Header, // Oder Query, je nachdem, wo der API-Key erwartet wird
                Description = "API-Key für den Zugriff auf die API",
                Scheme = "ApiKeyScheme"
            });
            // Füge hier deine Sicherheitsdefinition hinzu
            var secureEndpoints = new List<string> { "/api/MemeName", "/api/Settings", "/api/OptionalInfo", "/api/CustomMemeBilder" };

            foreach (var path in swaggerDoc.Paths)
            {
                if (!secureEndpoints.Any(x => path.Key.StartsWith(x)))
                {
                    continue;
                }

                foreach (var operation in path.Value.Operations)
                {

                    operation.Value.Security = new List<OpenApiSecurityRequirement>
                    {

                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "ApiKey"
                                    }
                                },
                                new List<string>()
                            }
                        }
                    };
                }
            }
        }
    }

}
