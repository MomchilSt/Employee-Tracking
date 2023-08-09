using EmployeeTracking.Data.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTracking.ViewModel
{
    public class VacationViewModel
    {
        public string Id { get; set; }

        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime EndDate { get; set; }

        public VacationStatus Status { get; set; }

        public string TrackingUserId { get; set; }

        public string? DeclineNote { get; set; }
    }
}