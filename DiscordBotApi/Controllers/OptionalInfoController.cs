using DiscordBotApi.ApiModels;
using DiscordBotApi.DTO;
using DiscordBotApi.Filters;
using DiscordBotApi.services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class OptionalInfoController : ControllerBase
    {
        private readonly IOptionalInfo info;

        public OptionalInfoController(IOptionalInfo info)
        {
            this.info = info;
        }
        /// <summary>
        /// Get the page size length.
        /// </summary>
        /// <remarks>
        /// This endpoint returns information about the page size length.
        /// </remarks>
        [HttpGet("PageSizeLength")]
        public async Task<IActionResult> GetPageSizeLength()
        {
            try
            {
                var size = await info.MaxPageSizeLength();
                return Ok(size);
            }
            catch (Exception)
            {
                return StatusCode(500,"Error");
            }
            
        }
        [HttpGet("GetPageSizewithmaxsite")]
        [ProducesResponseType(typeof(PageSizeResultDTO), 200)] // 200: OK
        public async Task<IActionResult> GetPageSizewithmaxsite([FromQuery] OptionalInfoModel infoModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new
                {
                    Property = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                }).ToList();
                return BadRequest(errors);
            }

            try
            {
                var pageSizeResultdto = await info.PageSizewithMemeNamedata(infoModel);
                return Ok(pageSizeResultdto);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
