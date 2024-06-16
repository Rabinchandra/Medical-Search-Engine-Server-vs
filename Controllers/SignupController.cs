using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        MedicalSearchEngineContext _context;

        public SignupController()
        {
            _context = new MedicalSearchEngineContext();
        }

        // Function to Add a new Patient
        [HttpPost("patient")]
        public async Task<ActionResult> AddPatient([FromBody] Patient newPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid attribute values");
            }

            try
            {
                // Add to Patient Table
                _context.Patients.Add(newPatient);
                // Add to user roles table
                _context.UserRoles.Add(new UserRole() { UserId = newPatient.PatientId, Role = "patient" });
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(AddPatient), newPatient);
        }

        // Function to Add a new Doctor
        [HttpPost("doctor")]
        public async Task<ActionResult> AddDoctor([FromBody] Doctor newDoctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid attribute values");
            }

            try
            {
                // Add to Doctors Table
                _context.Doctors.Add(newDoctor);
                // Add to user roles table
                _context.UserRoles.Add(new UserRole() { UserId = newDoctor.DoctorId, Role = "doctor" });
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return CreatedAtAction(nameof(AddDoctor), newDoctor);
        }
    }
}
