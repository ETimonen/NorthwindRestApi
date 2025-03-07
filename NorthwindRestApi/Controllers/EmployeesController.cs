using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // Alustetaan tietokantayhteys

        // Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public EmployeesController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // Hakee kaikki työntekijät
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var emp = db.Employees.ToList();
                return Ok(emp);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Hakee yhden työntekijän id:n perusteella
        [HttpGet("{id}")]
        public ActionResult GetOneEmployeeById(int id)
        {
            try
            {
                var emp = db.Employees.Find(id);
                if (emp != null)
                {
                    return Ok(emp);
                }
                else
                {
                    return NotFound($"Työntekijää id:llä {id} ei löydy.");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }

        // Uuden työntekijän lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Employee emp)
        {
            try
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä {emp.FirstName} {emp.LastName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Työntekijän poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var emp = db.Employees.Find(id);

                if (emp != null)
                {
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    return Ok($"Työntekijä {emp.FirstName} {emp.LastName} poistettiin.");
                }
                else
                {
                    return NotFound("Työntekijää id:llä " + id + " ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        // Työntekijän muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditEmployee(int id, [FromBody] Employee emp)
        {
            var duunari = db.Employees.Find(id);
            if (duunari != null)
            {
                duunari.LastName = emp.LastName;
                duunari.FirstName = emp.FirstName;
                duunari.Title = emp.Title;
                duunari.TitleOfCourtesy = emp.TitleOfCourtesy;
                duunari.BirthDate = emp.BirthDate;
                duunari.HireDate = emp.HireDate;
                duunari.Address = emp.Address;
                duunari.City = emp.City;
                duunari.Region = emp.Region;
                duunari.PostalCode = emp.PostalCode;
                duunari.Country = emp.Country;
                duunari.HomePhone = emp.HomePhone;
                duunari.Extension = emp.Extension;
                duunari.Photo = emp.Photo;
                duunari.Notes = emp.Notes;
                duunari.ReportsTo = emp.ReportsTo;
                duunari.PhotoPath = emp.PhotoPath;

                db.SaveChanges();
                return Ok($"Muokattu työntekijää {duunari.FirstName} {duunari.LastName}");

            }

            return NotFound("Työntekijää ei löytynyt id:llä " + id);

        }

        // Hakee sukunimen osalla: /api/lastname/[hakusana]
        [HttpGet("lastname/{ename}")]
        public ActionResult GetByName(string ename)
        {
            try
            {
                var emp = db.Employees.Where(e => e.LastName.Contains(ename));
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}