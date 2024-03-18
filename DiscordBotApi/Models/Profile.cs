namespace DiscordBotApi.Models;

public partial class Profile
{
    public Guid Id { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Username { get; set; }

    public string? FullName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Website { get; set; }

    public virtual User1 IdNavigation { get; set; } = null!;
}
