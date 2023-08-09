using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Data.Data.Models.Enums;
using EmployeeTracking.InputModels;

namespace EmployeeTracking.Services.Interfaces
{
    public interface IVacationService
    {
        Task<bool> Create(VacationInputModel inputModel, string userId);

        IEnumerable<Vacation> GetVacationsByStatus(string userId, VacationStatus status);

        IEnumerable<Vacation> GetAllPendingVacations();

        Task<bool> ApproveVacation(string vacationId);

        Task<bool> DeclineVacation(string message, string vacationId);
    }
}
