namespace DiscordBotApi.Models;

public partial class SchulBotConfig
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? Messagelisten { get; set; }

    public long? UserId { get; set; }

    public bool GetRandomMeme { get; set; }

    public virtual User? User { get; set; }
}
