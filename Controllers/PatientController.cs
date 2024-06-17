using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly MedicalSearchEngineContext _context;
        private readonly FirebaseService _firebaseService;

        public PatientController()
        {
            _context = new MedicalSearchEngineContext();
            _firebaseService = new FirebaseService();
        }

        [HttpGet]
        public ActionResult GetAllPatient()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult GetOnePatient(string id)
        {
            var foundPatient = _context.Patients.FirstOrDefault(p => p.PatientId == id);
            // if not found
            if (foundPatient == null) return NotFound("Patient with the given id doesn't exists");

            return Ok(foundPatient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemovePatient(string id)
        {
            var foundPatient = _context.Patients.FirstOrDefault(p => p.PatientId == id);

            // if not found
            if (foundPatient == null) return NotFound("Patient with the given id doesn't exists");

            // Reemove the patient from appointment list table
            var patientAppointments = _context.Appointments.Where(a => a.PatientId == foundPatient.PatientId);

            foreach (var appointment in patientAppointments)
            {
                _context.Appointments.Remove(appointment);
            }

            // Remove the patient from user roles table 
            var patientRoles = _context.UserRoles.FirstOrDefault(ur => ur.UserId == foundPatient.PatientId);

            if (patientRoles != null) _context.UserRoles.Remove(patientRoles);

            // Remove the doctor from the firebase
            var result = await _firebaseService.DeleteUser(id);

            // Remove the patient from patient table
            _context.Patients.Remove(foundPatient);
            await _context.SaveChangesAsync();

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
