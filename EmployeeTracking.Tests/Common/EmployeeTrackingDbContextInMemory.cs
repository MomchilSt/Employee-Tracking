using EmployeeTracking.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracking.Tests.Common
{
    internal class EmployeeTrackingDbContextInMemory
    {
        public static EmployeeTrackingDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<EmployeeTrackingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            return new EmployeeTrackingDbContext(options);
        }
    }
}
