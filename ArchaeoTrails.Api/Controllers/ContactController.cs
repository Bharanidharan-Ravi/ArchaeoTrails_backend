using ArchaeoTrails.Application.Features.Contact;
using ArchaeoTrails.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArchaeoTrails.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        // Inject the interface, NOT the concrete class
        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] ContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserEmail) || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { status = "error", message = "Invalid form data." });
            }

            var success = await _emailService.SendContactEmailAsync(request);

            if (success)
            {
                return Ok(new { status = "success" });
            }

            return StatusCode(500, new { status = "error", message = "Failed to send email." });
        }
    }
}
