using AppointmentApi.Models;
using AppointmentApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _service;

        public AppointmentsController(AppointmentService service)
        {
            _service = service;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book(AppointmentRequest request)
        {
            try
            {
                var result = await _service.BookAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _service.CancelAsync(id);
            return result
                ? Ok(new { message = "Cancelled" })
                : NotFound(new { message = "Not found" });
        }

        [HttpGet("slots")]
        public async Task<IActionResult> GetSlots([FromQuery] DateTime date)
        {
            var slots = await _service.GetAvailableSlotsAsync(date);
            return Ok(slots);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] string email)
        {
            var appts = await _service.GetByEmailAsync(email);
            return Ok(appts);
        }
    }
}
