using Microsoft.EntityFrameworkCore;
using Sehatak.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Doctorschedule> DoctorSchedules => Set<Doctorschedule>();
        public DbSet<StaffShift> StaffShifts => Set<StaffShift>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Waitlist> Waitlists => Set<Waitlist>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<LabResult> LabResults => Set<LabResult>();
        public DbSet<Consultation> Consultations => Set<Consultation>();
        public DbSet<DoctorRating> DoctorRatings => Set<DoctorRating>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Chat> Chats => Set<Chat>();
    }
}
