using Swashbuckle.AspNetCore.Annotations;

namespace DiscordBotApi.ApiModels
{
    [SwaggerSchema(Required = new[] { "BeschreibungsID", "BeschreibungsName",  })]
    public record MemeBeschreibungApiModel
    {
        [SwaggerSchema(
            Title = "BeschreibungsID",
            Format = "int",
            Nullable =false)]
        public int BeschreibungsID { get; set; }
        [SwaggerSchema(
        Title = "Inhalt des Textes",
        Format = "string",
        Nullable =false)]
        public required string BeschreibungsName { get; set; }


    }
}
