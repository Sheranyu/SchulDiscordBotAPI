using System.ComponentModel.DataAnnotations;

namespace DiscordBotApi.ApiModels;

public class CustomMemePictureModel
{
    [Required]
    public string MemeName {get; set;}
    [Required]
    public string BucketName {get;set;}
    [Required]
    public ulong DiscordGuildID {get; set;}
    public Uri? url {get; set;}
}
