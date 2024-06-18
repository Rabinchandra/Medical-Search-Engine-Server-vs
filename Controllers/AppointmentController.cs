using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Server_.Models.DTOModel;
using Server_.Models.EntityModel;

namespace Server_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AppointmentController : ControllerBase
    {
        public readonly MedicalSearchEngineContext _context;
        public readonly WhatsAppService _whatsAppService;

        public AppointmentController()
        {
            _context = new MedicalSearchEngineContext();
            _whatsAppService = new WhatsAppService();
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
                             MeetingUrl = appointment.MeetingUrl
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
                             Notes = appointment.Notes,
                             MeetingUrl = appointment.MeetingUrl
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
                             Notes = appointment.Notes,
                             MeetingUrl = appointment.MeetingUrl
                         };
            return Ok(result);
        }

        // Method to accept an appointment
        [HttpGet("accept/{appointmentId}")]
        public ActionResult AcceptAppointment(int appointmentId)
        {
            try
            {
                var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

                if (appointment == null) return BadRequest("Appointment with the given id doesn't exists");

                // Update the status
                appointment.Status = "accepted";
                appointment.MeetingUrl = RandomID(5);

                // Extract the information required for sending whats app message
                var patientName = _context.Patients.FirstOrDefault(p => p.PatientId == appointment.PatientId)?.Name;
                var doctorName = _context.Doctors.FirstOrDefault(d => d.DoctorId == appointment.DoctorId)?.Name;
                var appointmentDate = appointment.AppointmentDate.ToString() ?? "";
                var appointmentTime = appointment.AppointmentTime.ToString() ?? "";

                // Send message to whatsapp informing that appointment has been accepted
                // Phone number is hardcoded now, but we can change it later
                _whatsAppService.SendWhatsAppMessage("+916009383347", patientName ?? "", doctorName ?? "", appointment.MeetingUrl, appointmentDate, appointmentTime);

                _context.SaveChanges();

                return Ok(new { appointmentId = appointment.AppointmentId });
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!!");
            }
        }


        public static string RandomID(int len)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(result)) return result;

            string chars = "12345qwertyuiopasdfgh67890jklmnbvcxzMNBVCZXASDQWERTYHGFUIOLKJP";
            int maxPos = chars.Length;

            len = len > 0 ? len : 5;  // Default length to 5 if len is zero or negative

            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                result += chars[random.Next(maxPos)];
            }

            return result;
        }

    }
}
