using DiscordBotApi.ApiModels;
using DiscordBotApi.DTO;

namespace DiscordBotApi.services.Contracts
{
    public interface IOptionalInfo
    {
        public Task<long> MaxPageSizeLength();
        public Task<PageSizeResultDTO> PageSizewithMemeNamedata(OptionalInfoModel pagedata);
    }
}
