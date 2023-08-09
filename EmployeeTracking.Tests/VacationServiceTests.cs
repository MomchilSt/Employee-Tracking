using EmployeeTracking.Data.Data;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Data.Data.Models.Enums;
using EmployeeTracking.InputModels;
using EmployeeTracking.Services.Interfaces;
using EmployeeTracking.Services.Services;
using EmployeeTracking.Tests.Common;
using Microsoft.Extensions.Logging;

namespace EmployeeTracking.Tests
{
    [TestClass]
    public class VacationServiceTests
    {
        private IVacationService _vacationService;
        private EmployeeTrackingDbContext _context;
        private IUserService _userService;

        public VacationServiceTests()
        {
            _context = EmployeeTrackingDbContextInMemory.InitializeContext();
            _userService = new UserService(_context);
            _vacationService = new VacationService(_context, _userService, new LoggerFactory());

            _context.Users.Add(new TrackingUser() { Id = "1" });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Succesfully_Create_Vacation()
        {
            var result = _vacationService.Create(new VacationInputModel()
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                Description = "Description",
            }, "1");

            var vacation = _context.Vacations.FirstOrDefault();

            Assert.IsTrue(result.Result);
            Assert.IsNotNull(vacation);
            Assert.IsTrue(vacation?.Description == "Description");
        }

        [TestMethod]
        public async Task Create_Vacation_With_Missing_With_Missing_UserId()
        {
            var result = await _vacationService.Create(new VacationInputModel()
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                Description = "asd",
            }, string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetAllPendingVacations()
        {
            await SeedVacations(VacationStatus.Pending);

            var result = _vacationService.GetAllPendingVacations();

            Assert.IsTrue(result.Count() == 3);
        }


        [TestMethod]
        public void GetAllPendingVacations_With_Missing_Vacations()
        {
            var result = _vacationService.GetAllPendingVacations();

            Assert.IsTrue(result.Count() == 0);
        }


        [TestMethod]
        public async Task GetVacationsByStatus_With_Approved_Status()
        {
            await SeedVacations(VacationStatus.Approved);

            var result = _vacationService.GetVacationsByStatus("1", VacationStatus.Approved);

            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public async Task GetVacationsByStatus_With_Missing_Id()
        {
            await SeedVacations(VacationStatus.Approved);

            var result = _vacationService.GetVacationsByStatus(string.Empty, VacationStatus.Approved);

            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void GetVacationsByStatus_With_No_Vacations()
        {
            var result = _vacationService.GetVacationsByStatus(string.Empty, VacationStatus.Approved);

            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public async Task Approve_Vacation()
        {
            await SeedVacation(VacationStatus.Pending);
            var vacation = _context.Vacations.First();

            var result = await _vacationService.ApproveVacation(vacation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(vacation.Status == VacationStatus.Approved);
        }

        [TestMethod]
        public async Task Approve_Vacation_Missing_Id()
        {

            var result = await _vacationService.ApproveVacation(string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Decline_Vacation()
        {
            await SeedVacation(VacationStatus.Pending);
            var vacation = _context.Vacations.First();

            var result = await _vacationService.DeclineVacation("test", vacation.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(vacation.DeclineNote == "test");
        }

        [TestMethod]
        public async Task Decline_Vacation_With_Missing_Message()
        {
            await SeedVacation(VacationStatus.Pending);
            var vacation = _context.Vacations.First();

            var result = await _vacationService.DeclineVacation(string.Empty, vacation.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Decline_Vacation_With_Missing_Id()
        {
            await SeedVacation(VacationStatus.Pending);
            var vacation = _context.Vacations.First();

            var result = await _vacationService.DeclineVacation("123", string.Empty);

            Assert.IsFalse(result);
        }

        private async Task SeedVacations(VacationStatus status)
        {
            await _context.Vacations.AddAsync(new Vacation()
            {
                Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(5),
                Description = "123",
                Status = status,
                TrackingUserId = "1"
            });

            await _context.Vacations.AddAsync(new Vacation()
            {
                Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(5),
                Description = "123",
                Status = status,
                TrackingUserId = "1"
            });

            await _context.Vacations.AddAsync(new Vacation()
            {
                Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(5),
                Description = "123",
                Status = status,
                TrackingUserId = "1"
            });

            await _context.SaveChangesAsync();
        }

        private async Task SeedVacation(VacationStatus status)
        {
            await _context.Vacations.AddAsync(new Vacation()
            {
                Id = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(5),
                Description = "123",
                Status = status,
                TrackingUserId = "1"
            });

            await _context.SaveChangesAsync();
        }
    }
}