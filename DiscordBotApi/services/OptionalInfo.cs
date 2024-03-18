using DiscordBotApi.ApiModels;
using DiscordBotApi.DTO;
using DiscordBotApi.Models;
using DiscordBotApi.services.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscordBotApi.services
{
    public class OptionalInfo : IOptionalInfo
    {
        private readonly PostgresContext db;

        public OptionalInfo(PostgresContext db)
        {
            this.db = db;
        }
        
        public async Task<long> MaxPageSizeLength()
        {
            var totalRecords = db.MemeNames.Count();
            return await Task.FromResult<long>(totalRecords);
        }
        public async Task<PageSizeResultDTO> PageSizewithMemeNamedata(OptionalInfoModel pagedata)
        {
            var totalRecords = db.MemeNames.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pagedata.PageSize);
            var data = await db.MemeNames.OrderBy(x => x.Name).Select(x => x.Name)
               .Skip((pagedata.currentpage - 1) * pagedata.PageSize)
               .Take(pagedata.PageSize)
               .ToArrayAsync();

            return  new PageSizeResultDTO {MemeNameDTO = data, totalPages = totalPages };
        }
    }
}
