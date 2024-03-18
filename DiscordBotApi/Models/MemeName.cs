namespace DiscordBotApi.Models;

public partial class MemeName
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TextInhalt> TextInhalts { get; set; } = new List<TextInhalt>();
}
