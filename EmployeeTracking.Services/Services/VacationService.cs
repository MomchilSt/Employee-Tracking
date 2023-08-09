using EmployeeTracking.Data.Data;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Data.Data.Models.Enums;
using EmployeeTracking.InputModels;
using EmployeeTracking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeTracking.Services.Services
{
    public class VacationService : IVacationService
    {
        private readonly EmployeeTrackingDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<VacationService> _logger;

        public VacationService(
            EmployeeTrackingDbContext context,
            IUserService userService, 
            ILoggerFactory loggerFactory)
        {
            this._context = context;
            this._userService = userService;
            this._logger = loggerFactory.CreateLogger<VacationService>();
        }

        public async Task<bool> Create(VacationInputModel inputModel, string userId)
        {
            try
            {
                var user = await this._userService.GetById(userId);
                var datesTimeSpan = inputModel.EndDate - inputModel.StartDate;

                if (user.VacationDays < datesTimeSpan.Days) 
                {
                    this._logger.LogError("Not Enough Vacation Days");
                    return false;
                }

                if (user == null)
                {
                    this._logger.LogError("Missing User!");
                    return false;
                }

                var vacation = new Vacation()
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDate = inputModel.StartDate,
                    EndDate = inputModel.EndDate,
                    Description = inputModel.Description,
                    TrackingUserId = userId,
                    TrackingUser = user
                };

                await _context.Vacations.AddAsync(vacation);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                this._logger.LogError("Error occured while trying to create vacation.", ex.Message);
                return false;
            }
        }

        public IEnumerable<Vacation> GetVacationsByStatus(string userId, VacationStatus status) =>
            this._context
            .Vacations
            .Where(v => v.TrackingUserId == userId && v.Status == status);

        public IEnumerable<Vacation> GetAllPendingVacations() =>
            this._context
            .Vacations
            .Where(v => v.Status == VacationStatus.Pending);

        public async Task<bool> ApproveVacation(string vacationId)
        {
            var vacation = await _context.Vacations.FirstOrDefaultAsync(v => v.Id == vacationId);

            if (vacation == null)
            {
                _logger.LogError($"Missing vacation with {vacationId}!");
                return false;
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == vacation.TrackingUserId);

            if (user == null)
            {
                _logger.LogError($"Missing user with {vacation.TrackingUserId}!");
                return false;
            }

            var vacationTimeSpan = vacation.EndDate - vacation.StartDate;

            vacation.Status = VacationStatus.Approved;
            user.VacationDays -= vacationTimeSpan.Days;

            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeclineVacation(string message, string vacationId)
        {
            var vacation = await _context.Vacations.FirstOrDefaultAsync(v => v.Id == vacationId);

            if (vacation == null || string.IsNullOrWhiteSpace(message))
            {
                _logger.LogError($"Missing vacation with {vacationId}!");
                return false;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == vacation.TrackingUserId);

            if (user == null)
            {
                _logger.LogError($"Missing user with {vacation.TrackingUserId}!");
                return false;
            }

            vacation.Status = VacationStatus.Declined;
            vacation.DeclineNote = message;

            return _context.SaveChanges() > 0;
        }
    }
}
