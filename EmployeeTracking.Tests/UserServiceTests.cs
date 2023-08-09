using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Data.Data;
using EmployeeTracking.Services.Interfaces;
using EmployeeTracking.Services.Services;
using EmployeeTracking.Tests.Common;
using Microsoft.Extensions.Logging;
using EmployeeTracking.InputModels;

namespace EmployeeTracking.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private EmployeeTrackingDbContext _context;
        private IUserService _userService;

        public UserServiceTests()
        {
            _context = EmployeeTrackingDbContextInMemory.InitializeContext();
            _userService = new UserService(_context);

            _context.Users.Add(new TrackingUser() { Id = "1" });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Succesfully_GetById()
        {
            var result = await _userService.GetById("1");

            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public async Task Missing_Id_GetById()
        {
            var result = await _userService.GetById("2");

            Assert.IsNull(result);
        }
    }
}
