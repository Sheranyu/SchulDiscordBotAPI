

using System.ComponentModel.DataAnnotations;

namespace DiscordBotApi.ApiModels;
public class MemeCreationModel
{
    [Required(ErrorMessage = "MemeName ist erforderlich.")]
    public string Memename { get; set; }

    [Required(ErrorMessage = "Beschreibung ist erforderlich.")]
    [RegularExpression("^[a-zA-Z0-9]*$")]
    public string Beschreibung { get; set; }
}