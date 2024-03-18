using DiscordBotApi.ApiModels;

namespace DiscordBotApi;

public interface ICustomMemePicture
{
    public Task InsertCustomMemeURL(CustomMemePictureModel bilddata);
    public Task<string> GetCustomMemeURL(CustomMemePictureModel bilddata);
}
