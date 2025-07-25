using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Models
{
    public class AppointmentRequest
    {
        [Required]
        public string CustomerName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public ServiceType ServiceType { get; set; }

        [Required]
        public DateTime Date { get; set; } // Must be at an hour mark
    }
}
