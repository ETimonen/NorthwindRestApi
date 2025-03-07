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

        // Perinteinen tapa
        // NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public DocumentationController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // Dokumentaation haku id:llä
        [HttpGet("{id}")]
        public ActionResult SearchDocById(int id)
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

        // Hakee kaikki dokumentit
        [HttpGet]
        public ActionResult GetAllDocs()
        {
            try
            {
                var dokumentit = db.Documentations.ToList();
                return Ok(dokumentit);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Hakee metodin osalla: /api/method/[hakusana]
        [HttpGet("method/{mname}")]
        public ActionResult GetDocByMethod(string mname)
        {
            try
            {
                var doc = db.Documentations.Where(m => m.Method.Contains(mname));
                return Ok(doc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Uuden dokumentaation lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Documentation doc)
        {
            try
            {
                db.Documentations.Add(doc);
                db.SaveChanges();
                return Ok($"Lisättiin uusi dokumentaatio {doc.Method} from {doc.AvaibleRoute}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Dokumentaation poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var doc = db.Documentations.Find(id);

                if (doc != null)
                {
                    db.Documentations.Remove(doc);
                    db.SaveChanges();
                    return Ok("Dokumentaatio " + doc.AvaibleRoute + " poistettiin.");
                }
                else
                {
                    return NotFound("Dokumentaatiota id:llä " + id + " ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Dokumentaation muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditDoc(string id, [FromBody] Documentation doc)
        {
            var dokumentti = db.Documentations.Find(id);
            if (dokumentti != null)
            {
                dokumentti.AvaibleRoute = doc.AvaibleRoute;
                dokumentti.Method = doc.Method;
                dokumentti.Description = doc.Description;

                db.SaveChanges();
                return Ok("Muokattu dokumentaatiota " + dokumentti.AvaibleRoute);

            }

            return NotFound("Dokumentaatiota ei löytynyt id:llä " + id);

        }
    }
}
