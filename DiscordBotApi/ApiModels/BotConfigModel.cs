

using System.ComponentModel.DataAnnotations;

namespace DiscordBotApi.ApiModels;
public class BotConfigModel
{
    [Required]
    public bool Allowed { get; set; }
    [Required]
    public long UserID { get; set; }
    [Required]
    public string Password { get; set; }
}