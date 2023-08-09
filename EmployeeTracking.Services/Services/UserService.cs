using EmployeeTracking.Data.Data;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Services.Services
{
    public class UserService : IUserService
    {
        private readonly EmployeeTrackingDbContext _context;

        public UserService(EmployeeTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<TrackingUser> GetById(string id)
        {
            return await this._context.Users
                .FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
