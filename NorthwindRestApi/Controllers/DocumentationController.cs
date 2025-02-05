using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        // Alustetaan tietokantayhteys
        NorthwindOriginalContext db = new NorthwindOriginalContext();
        // Dokumentaation haku
        [HttpGet("{id}")]
        public ActionResult SearchDoc(int id)
        {
            try
            {
                var dokumentti = db.Documentations.Find(id);
                if (dokumentti != null)
                {
                    return Ok(dokumentti);
                }
                else
                {
                    return NotFound(DateTime.Now + ": Documentation missing.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
