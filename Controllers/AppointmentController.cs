using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Server_.Models.DTOModel;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        public readonly MedicalSearchEngineContext _context;

        public AppointmentController()
        {
            _context = new MedicalSearchEngineContext();
        }

        // Get all the appointments
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            return Ok(_context.Appointments.ToList());
        }

        [HttpGet("details")]
        public IActionResult GetAllAppointmentsDetails()
        {
            var result = from appointment in _context.Appointments
                         join doctor in _context.Doctors on appointment.DoctorId equals doctor.DoctorId
                         join patient in _context.Patients on appointment.PatientId equals patient.PatientId
                         select new
                         {
                             AppointmentId = appointment.AppointmentId,
                             Status = appointment.Status,
                             DoctorName = doctor.Name,
                             PatientName = patient.Name,
                             DoctorId = doctor.DoctorId,
                             PatientId = patient.PatientId,
                             PatientImgUrl = patient.ProfileImgUrl,
                             patientContact = patient.ContactNumber,
                         };
            return Ok(result);
        }

        // Add an appointment
        [HttpPost]
        public IActionResult AddAppointment([FromBody] AppointmentDTO app)
        {

            try
            {
                var newAppointment = new Appointment()
                {
                    AppointmentDate = app.AppointmentDate,
                    AppointmentTime = app.AppointmentTime,
                    DoctorId = app.DoctorId,
                    PatientId = app.PatientId,
                    Purpose = app.Purpose
                };
                _context.Appointments.Add(newAppointment);
                _context.SaveChanges();

                return CreatedAtAction(nameof(AddAppointment), newAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong while trying to add a new appointment!");
            }

        }

        [HttpGet("patient/{id}")]
        // Get appointment for a patient
        public IActionResult GetAppointmentForPatient(string id)
        {
            try
            {
                var appointments = _context.Appointments.Where(a => a.PatientId == id).ToList();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("patient/details/{id}")]
        public IActionResult GetAppointmentDetailsForPatient(string id)
        {
            var result = from appointment in _context.Appointments
                         join doctor in _context.Doctors on appointment.DoctorId equals doctor.DoctorId
                         join patient in _context.Patients on appointment.PatientId equals patient.PatientId
                         where patient.PatientId == id
                         select new
                         {
                             AppointmentId = appointment.AppointmentId,
                             AppointmentDate = appointment.AppointmentDate,
                             AppointmentTime = appointment.AppointmentTime,
                             Status = appointment.Status,
                             DoctorName = doctor.Name,
                             PatientName = patient.Name,
                             DoctorId = doctor.DoctorId,
                             PatientId = patient.PatientId,
                             PatientImgUrl = patient.ProfileImgUrl,
                             PatientContact = patient.ContactNumber,
                             Purpose = appointment.Purpose,
                             Notes = appointment.Notes
                         };
            return Ok(result);
        }

        [HttpGet("doctor/{id}")]
        // Get appointment for a doctor
        public IActionResult GetAppointmentForDoctor(string id)
        {
            try
            {
                var appointments = _context.Appointments
                                                    .Where(a => a.DoctorId == id && (a.Status == "accepted" || a.Status == "completed"))
                                                    .ToList();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("doctor/details/{id}")]
        public IActionResult GetAppointmentDetailsForDoctor(string id)
        {
            var result = from appointment in _context.Appointments
                         join doctor in _context.Doctors on appointment.DoctorId equals doctor.DoctorId
                         join patient in _context.Patients on appointment.PatientId equals patient.PatientId
                         where doctor.DoctorId == id && appointment.Status != "pending"
                         select new
                         {
                             AppointmentId = appointment.AppointmentId,
                             AppointmentDate = appointment.AppointmentDate,
                             AppointmentTime = appointment.AppointmentTime,
                             Status = appointment.Status,
                             DoctorName = doctor.Name,
                             PatientName = patient.Name,
                             DoctorId = doctor.DoctorId,
                             PatientId = patient.PatientId,
                             PatientImgUrl = patient.ProfileImgUrl,
                             PatientContact = patient.ContactNumber,
                             Purpose = appointment.Purpose,
                             Notes = appointment.Notes
                         };
            return Ok(result);
        }

        // Method to accept an appointment
        [HttpGet("accept/{appointmentId}")]
        public ActionResult AcceptAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null) return BadRequest("Appointment with the given id doesn't exists");

            // Update the status
            appointment.Status = "accepted";
            _context.SaveChanges();

            return Ok(appointment);
        }
    }
}
