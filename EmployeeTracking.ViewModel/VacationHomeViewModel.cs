namespace EmployeeTracking.ViewModel
{
    public class VacationHomeViewModel
    {
        public List<VacationViewModel> AllPendingVacations { get; set; } = new List<VacationViewModel>();
        public List<VacationViewModel> ApprovedVacations { get; set; } = new List<VacationViewModel>();

        public List<VacationViewModel> PendingVacations { get; set; } = new List<VacationViewModel>();

        public List<VacationViewModel> DeclinedVacations { get; set; } = new List<VacationViewModel>();
    }
}
