using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<AppointmentModel> Appointments => Set<AppointmentModel>();
    }
}
