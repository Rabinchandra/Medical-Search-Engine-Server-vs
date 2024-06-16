using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Server_.Models.EntityModel;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly MedicalSearchEngineContext _context;

        public UserRolesController()
        {
            _context = new MedicalSearchEngineContext();
        }

        [HttpGet("{id}")]
        public ActionResult GetUserRole(string id)
        {
            var userRole = _context.UserRoles.FirstOrDefault(user => user.UserId == id);

            if (userRole == null) return NotFound("User with the given id doesn't exists");

            return Ok(new { role = userRole.Role });
        }
    }
}
