using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Models
{
    public class AppointmentModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTime Date { get; set; }
    }
}
