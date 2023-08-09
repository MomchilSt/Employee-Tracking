using EmployeeTracking.Data.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace EmployeeTracking.Data.Data.Models
{
    public class TrackingUser : IdentityUser
    {
        public int VacationDays { get; set; } = 20;

        public ICollection<Vacation> Vacations { get; set; } = new HashSet<Vacation>();
    }
}
