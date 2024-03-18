namespace DiscordBotApi.Models;

public partial class User
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string Authtoken { get; set; } = null!;

    public virtual ICollection<SchulBotConfig> SchulBotConfigs { get; set; } = new List<SchulBotConfig>();
}
