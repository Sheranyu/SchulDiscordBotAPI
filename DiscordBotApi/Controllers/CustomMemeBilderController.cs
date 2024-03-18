using DiscordBotApi.ApiModels;
using DiscordBotApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(ApiRateLimiter))]
    [ApiKeyAuth]
    public class CustomMemeBilderController : ControllerBase
    {
        private readonly ICustomMemePicture customMeme;
        private readonly ILogger<CustomMemeBilderController> logger;

        public CustomMemeBilderController(ICustomMemePicture customMeme, ILogger<CustomMemeBilderController> logger)
        {
            this.customMeme = customMeme;
            this.logger = logger;
        }
        
        [HttpGet("MemePicturefromMemeName")]
        public async Task<IActionResult> GetMemePicturefromMemeName([FromQuery] CustomMemePictureModel bild)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }
            try
            {
                var url = await customMeme.GetCustomMemeURL(bild);
                return Ok(url);
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }
          
        }

        [HttpPost("SetNewPictureforMemeName")]
        public async Task<IActionResult> SetNewPictureforMemeName([FromBody] CustomMemePictureModel bild)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await customMeme.InsertCustomMemeURL(bild);
                return Created(new Uri("/api/MemePicturefromMemeName?" + bild.MemeName),"Bild abgelegt");
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }

        }
    }
}
