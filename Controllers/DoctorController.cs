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
        private readonly FirebaseService _firebaseService;

        public DoctorController()
        {
            _context = new MedicalSearchEngineContext();
            _firebaseService = new FirebaseService();
        }

        [HttpGet]
        public ActionResult GetAllDoctors()
        {
            return Ok(_context.Doctors.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult GetOneDoctor(string id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);
            // if not found
            if (doctor == null) return NotFound("Doctor with the given id doesn't exists");

            return Ok(doctor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveDoctor(string id)
        {
            var foundDoctor = _context.Doctors.FirstOrDefault(d => d.DoctorId == id);


            // if not found
            if (foundDoctor == null) return NotFound("Doctor with the given id doesn't exists");

            // Reemove the doctor from appointment list table
            var doctorAppointments = _context.Appointments.Where(d => d.DoctorId == id);

            foreach (var appointment in doctorAppointments)
            {
                _context.Appointments.Remove(appointment);
            }

            // Remove the doctor from user roles table 
            var doctorRoles = _context.UserRoles.FirstOrDefault(ur => ur.UserId == foundDoctor.DoctorId);

            if (doctorRoles != null) _context.UserRoles.Remove(doctorRoles);

            // Remove the doctor from the firebase
            var result = await _firebaseService.DeleteUser(id);

            // Remove the doctor from patient table
            _context.Doctors.Remove(foundDoctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
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
