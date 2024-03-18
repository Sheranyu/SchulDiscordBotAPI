using DiscordBotApi.ApiModels;
using DiscordBotApi.Filters;
using DiscordBotApi.services;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class SettingsController : ControllerBase
    {
        private readonly ChatTextLogic chat;
        private readonly ILogger<SettingsController> logger;

        public SettingsController(ChatTextLogic chat, ILogger<SettingsController> logger)
        {
            this.chat = chat;
            this.logger = logger;
        }

        [HttpPut("UpdatemessagelistenAllowed")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult UpdateAllowedBotPosts([FromBody] BotConfigModel config)
        {
            try
            {
                if (config.Password == "4321")
                {
                    chat.UpdateCheckaktivierer(config);
                    return Ok("Updated Options");
                }
                else
                {
                    return BadRequest("Falsches Password");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return StatusCode(500, "Ein Fehler ist aufgetretten");
            }
            
        }

        [HttpPut("UpdateCustomMemeAllowed")]
        [ProducesResponseType(typeof(string),200)]
        public IActionResult UpdateAllowedMemePost([FromBody] BotConfigModel config)
        {
            try
            {
                if (config.Password == "4321")
                {
                    chat.Updatememeaktivierer(config);
                    return Ok("Updated Options");
                }
                else
                {
                    return BadRequest("Falsches Password");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return StatusCode(500,"Ein Fehler ist aufgetretten");
            }

        }

        [HttpGet("GetAllowedBotPosts")]
        [ProducesResponseType(typeof(bool),200)]
        public IActionResult GetAllowedBotPosts()
        {
            try
            {
                var wert = chat.CheckAktivierer();
                return Ok(wert);
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }

        }

        [HttpGet("getallowedBotMemePost")]
        [ProducesResponseType(typeof(bool),200)]
        public IActionResult GetAllowedBotmemePosts()
        {
            try
            {

                
                var allowed = chat.CheckCustomMemeactive();
                return Ok(allowed);
            }
            catch (Exception ex)
            {

                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }

        }

    }
}
