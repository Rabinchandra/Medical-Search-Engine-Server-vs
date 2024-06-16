using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly MedicalSearchEngineContext _context;

        public PatientController()
        {
            _context = new MedicalSearchEngineContext();
        }

        [HttpGet]
        public ActionResult GetAllPatient()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpGet("id")]
        public ActionResult GetOnePatient(string id)
        {
            var foundPatient = _context.Patients.FirstOrDefault(p => p.PatientId == id);
            // if not found
            if (foundPatient == null) return NotFound("Patient with the given id doesn't exists");

            return Ok(foundPatient);
        }

        [HttpDelete("{id}")]
        public ActionResult RemovePatient(string id)
        {
            var foundPatient = _context.Patients.FirstOrDefault(p => p.PatientId == id);

            // if not found
            if (foundPatient == null) return NotFound("Patient with the given id doesn't exists");

            _context.Patients.Remove(foundPatient);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult UpdatePatient(string id, [FromBody] JsonPatchDocument<Patient> patchDoc)
        {
            var foundPatient = _context.Patients.FirstOrDefault(p => p.PatientId == id);

            if (foundPatient == null) return NotFound();

            patchDoc.ApplyTo(foundPatient);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
