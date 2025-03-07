using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Alustetaan tietokantayhteys

        // Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public ProductsController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // Hakee kaikki tuotteet
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                var prods = db.Products.ToList();
                return Ok(prods);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Hakee yhden tuotteen id:n perusteella
        [HttpGet("{id}")]
        public ActionResult GetOneProductById(int id)
        {
            try
            {
                var prod = db.Products.Find(id);
                if (prod != null)
                {
                    return Ok(prod);
                }
                else
                {
                    return NotFound($"Tuotetta id:llä {id} ei löydy.");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }

        // Uuden tuotteen lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok($"Lisättiin uusi tuote {prod.ProductName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Tuotteen poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var prod = db.Products.Find(id);

                if (prod != null)
                {
                    db.Products.Remove(prod);
                    db.SaveChanges();
                    return Ok("Tuote " + prod.ProductName + " poistettiin.");
                }
                else
                {
                    return NotFound("Tuotetta id:llä " + id + " ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Tuotteen muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditProduct(int id, [FromBody] Product prod)
        {
            var tuote = db.Products.Find(id);
            if (tuote != null)
            {
                tuote.ProductName = prod.ProductName;
                tuote.SupplierId = prod.SupplierId;
                tuote.CategoryId = prod.CategoryId;
                tuote.QuantityPerUnit = prod.QuantityPerUnit;
                tuote.UnitPrice = prod.UnitPrice;
                tuote.UnitsInStock = prod.UnitsInStock;
                tuote.UnitsOnOrder = prod.UnitsOnOrder;
                tuote.ReorderLevel = prod.ReorderLevel;
                tuote.Discontinued = prod.Discontinued;
                tuote.ImageLink = prod.ImageLink;

                db.SaveChanges();
                return Ok("Muokattu tuotetta " + tuote.ProductName);

            }

            return NotFound("Tuotetta ei löytynyt id:llä " + id);

        }

        // Hakee nimen osalla: /api/productname/[hakusana]
        [HttpGet("productname/{pname}")]
        public ActionResult GetByName(string pname)
        {
            try
            {
                var prod = db.Products.Where(p => p.ProductName.Contains(pname));
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
