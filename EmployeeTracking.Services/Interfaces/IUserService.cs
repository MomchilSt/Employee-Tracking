using EmployeeTracking.Data.Data.Models;

namespace EmployeeTracking.Services.Interfaces
{
    public interface IUserService
    {
        Task<TrackingUser> GetById(string id);
    }
}
