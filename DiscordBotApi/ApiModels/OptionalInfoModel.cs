using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiscordBotApi.ApiModels
{
    public class OptionalInfoModel
    {
        [Required]
        public int currentpage { get; set; }
        [Required]
        public int PageSize {  get; set; }
        
    }
}
