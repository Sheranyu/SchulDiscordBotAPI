namespace DiscordBotApi.Models;

public partial class TextInhalt
{
    public long IdText { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Text { get; set; }

    public long? NamesId { get; set; }

    public virtual MemeName? Names { get; set; }
}
