using EmployeeTracking.Data.Data.Models.Enums;

namespace EmployeeTracking.Data.Data.Models
{
    public class Vacation
    {
        public string Id { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public VacationStatus Status { get; set; }

        public string TrackingUserId { get; set; }

        public TrackingUser TrackingUser { get; set; }

        public string? DeclineNote { get; set; }
    }
}
