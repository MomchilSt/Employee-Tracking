using EmployeeTracking.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Data.Data
{
    public class EmployeeTrackingDbContext : IdentityDbContext<TrackingUser, IdentityRole, string>
    {
        public DbSet<Vacation> Vacations { get; set; }

        public EmployeeTrackingDbContext(DbContextOptions options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
