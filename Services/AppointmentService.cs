using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppointmentApi.Services
{
    public class AppointmentService
    {
        private readonly AppointmentDbContext _context;

        public AppointmentService(AppointmentDbContext context)
        {
            _context = context;
        }

        private static readonly TimeSpan[] Slots =
            Enumerable.Range(9, 9).Select(h => new TimeSpan(h, 0, 0)).ToArray();

        public async Task<AppointmentModel> BookAsync(AppointmentRequest req)
        {
            if (req.Date < DateTime.Now)
                throw new ArgumentException("Cannot book in the past.");

            var time = req.Date.TimeOfDay;
            if (!Slots.Contains(time))
                throw new ArgumentException("Invalid slot.");

            var hasConflict = await _context.Appointments.AnyAsync(a =>
                a.Date.Date == req.Date.Date &&
                a.Date.TimeOfDay == time &&
                a.Email == req.Email);

            if (hasConflict)
                throw new ArgumentException("You already booked this slot.");

            var slotTaken = await _context.Appointments.AnyAsync(a =>
                a.Date.Date == req.Date.Date &&
                a.Date.TimeOfDay == time);

            if (slotTaken)
                throw new ArgumentException("Slot is already booked.");

            var appt = new AppointmentModel
            {
                CustomerName = req.CustomerName,
                Email = req.Email,
                ServiceType = req.ServiceType,
                Date = req.Date
            };

            _context.Appointments.Add(appt);
            await _context.SaveChangesAsync();
            return appt;
        }

        public async Task<bool> CancelAsync(Guid id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt is null) return false;

            _context.Appointments.Remove(appt);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetAvailableSlotsAsync(DateTime date)
        {
            var booked = await _context.Appointments
                .Where(a => a.Date.Date == date.Date)
                .Select(a => a.Date.TimeOfDay)
                .ToListAsync();

            return Slots
                .Where(slot => !booked.Contains(slot))
                .Select(s => s.ToString(@"hh\:mm"));
        }

        public async Task<IEnumerable<AppointmentModel>> GetByEmailAsync(string email)
        {
            return await _context.Appointments
                .Where(a => a.Email == email)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }
    }
}
