using EmployeeTracking.InputModels.Validators;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTracking.InputModels
{
    public class VacationInputModel
    {
        [Required]
        [DateLessThanOrEqualToToday]
        public DateTime StartDate { get; set; }

        [Required]
        [DateLessThanOrEqualToToday]
        public DateTime EndDate { get; set; }

        [Required]
        public string Description { get; set; }
    }
}