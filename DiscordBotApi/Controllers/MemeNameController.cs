using DiscordBotApi.ApiModels;
using DiscordBotApi.CustomException;
using DiscordBotApi.DTO;
using DiscordBotApi.Filters;
using DiscordBotApi.Models;
using DiscordBotApi.services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DiscordBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    [SwaggerTag("Alle Operationen für MemeName")]
    //[ServiceFilter(typeof(ApiKeyAuth))]
    public class MemeNameController : ControllerBase
    {
        private readonly IRepName _memename;
        private readonly ILogger<MemeNameController> logger;

        public MemeNameController(IRepName memename, ILogger<MemeNameController> logger)
        {
            _memename = memename;
            this.logger = logger;
        }


        [HttpPost("CreateMemeNamemitBeschreibung")]
        [ProducesResponseType(typeof(MemeName), 201)] // 200: OK
        public IActionResult CreateMemeNameWithB([FromBody] MemeCreationModel memedata)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(x => x.Errors);
                return BadRequest();
            }

            try
            {
                var createtduser = _memename.CreateMemeNamemitBeschreibung(memedata.Beschreibung, memedata.Memename);
                if (createtduser is null)
                {
                    return BadRequest();
                }

                return CreatedAtAction("getUser", new { name = createtduser.Name }, createtduser);
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex.Message}", ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("CreateOnlyMemeName")]
        [ProducesResponseType(typeof(MemeName), 201)]
        public IActionResult CreateOnlyMemeName([FromBody][BindRequired][RegularExpression("^[a-zA-Z0-9]*$")] string memename)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var createtduser = _memename.CreateMemeNameOnly(memename);
                return CreatedAtAction("User", new { name = createtduser.Name }, createtduser);
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                throw;
            }
        }
        [SwaggerOperation(
        Summary = "Liste von MeneName",
        Description = "Abfrage einer gesamtListe von MemeName ohne Beschreibung",
        OperationId = "GetUserList"
      )]
        [SwaggerResponse(200, "List Object of MemeName[]", typeof(MemeName[]))]
        [HttpGet("GetUserList")]
        public ActionResult<MemeName[]> Get_User_List()
        {

            var userlist = _memename.GetMeme_List();
            if (userlist.Length <= 0)
            {
                return NotFound();
            }
            return Ok(userlist);

        }

        [HttpGet("getUser")]
        public IActionResult GetUserbyName(string UserName)
        {
            try
            {
                var user = _memename.GetMeme_NamesbyName(UserName);
                return Ok(user);

            }
            catch (Exception ex)
            {

                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }

        }

        [HttpGet("GetBeschreibungvomMemeName")]
        public IActionResult GetBeschreibung(string MemeName)
        {
            try
            {
                var list = _memename.GetBeschreibungfromMemeNames(MemeName);
                if (list is not null && list.Length > 0)
                {
                    return Ok(list);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }
        }

        [SwaggerOperation(
            Summary = "Löscht ein Memename",
            Description = "Dieser Endpunkt löscht einen MemeName engültig",
            OperationId = "DeleteMemeName"
            )]
        [SwaggerResponse(200, "Id des gelöschten Objekts", typeof(int))]
        [HttpDelete("{memenameid}/deleteuser")]
        public IActionResult DeleteMemeName([FromRoute, SwaggerParameter("Eine MemeName ID", Required = true)] int memenameid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(x => x.Errors.Any()).ToList();
                return BadRequest(error);
            }

            try
            {
                var deletetitem = _memename.DeleteMemeNameByID(memenameid);
                return Ok(("Item mit der id: {memenameid} wurde gelöscht", memenameid));
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }
        }

        [SwaggerOperation(
           Summary = "Update des Textinhalt",
           Description = "Updatet die Beschreibung eines MemeNames",
           OperationId = "Put"
           )]
        [SwaggerResponse(200,"Update Complete")]
        [HttpPut("Beschreibung")]
        public IActionResult UpdateBeschreibungvonMemeName([FromBody] MemeBeschreibungApiModel model)
        {
            try
            {
                _memename.UpdateBeschreibung(model.BeschreibungsID, model.BeschreibungsName);
                return Ok("Update Complete");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [SwaggerOperation(
          Summary = "Löschen des Textinhalt",
          Description = "Löscht die Beschreibung eines MemeNames",
          OperationId = "DeleteBeschreibung"
            
          )]
        [SwaggerResponse(200, "Delete Complete")]
        [HttpDelete("Beschreibung/{id}")]
        public IActionResult DeleteBeschreibungvonMemeName([FromRoute] int id)
        {
            try
            {
                _memename.DeleteBeschreibung(id);
                return StatusCode(501);
            }
            catch (Nohtingfound ex)
            {
                logger.LogInformation("Info: {ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("Fehler: {ex}", ex);
                return BadRequest();
            }
        }
    }
}
