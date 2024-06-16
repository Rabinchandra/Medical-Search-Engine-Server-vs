using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly MedicalSearchEngineContext _context;

        public DoctorController()
        {
            _context = new MedicalSearchEngineContext();
        }

        [HttpGet]
        public ActionResult GetAllDoctors()
        {
            return Ok(_context.Doctors.ToList());
        }

        [HttpGet("id")]
        public ActionResult GetOneDoctor(string id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);
            // if not found
            if (doctor == null) return NotFound("Doctor with the given id doesn't exists");

            return Ok(doctor);
        }

        [HttpDelete("id")]
        public ActionResult RemoveDoctor(string id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

            // if not found
            if (doctor == null) return NotFound("Doctor with the given id doesn't exists");

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("id")]
        public ActionResult PatchDoctor(string id, [FromBody] JsonPatchDocument<Doctor> patchDoc)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);

            if (doctor == null) return NotFound();

            patchDoc.ApplyTo(doctor);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
