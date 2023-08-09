using EmployeeTracking.Common.Constants;
using EmployeeTracking.Data.Data;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.Data.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace EmployeeTracking.Web.Seeder
{
    public class DbInitializer
    {
        public async static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EmployeeTrackingDbContext>();
                var isDbCreated = context != null && context.Database.EnsureCreated();

                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<TrackingUser>>();

                if (!context.Roles.Any())
                {
                    context.Roles.Add(new IdentityRole
                    {
                        Name = BasicConstants.Manager,
                        NormalizedName = BasicConstants.ManagerNormalized
                    });

                    context.Roles.Add(new IdentityRole
                    {
                        Name = BasicConstants.Employee,
                        NormalizedName = BasicConstants.EmployeeNormalized
                    });
                }

                if (isDbCreated && !context.Users.Any(usr => usr.UserName == "admin@admin.com"))
                {
                    var user = new TrackingUser()
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        EmailConfirmed = true
                    };

                    var userEmp = new TrackingUser()
                    {
                        UserName = "emp@emp.com",
                        Email = "emp@emp.com",
                        EmailConfirmed = true
                    };

                    var userEmpResult = await _userManager.CreateAsync(userEmp, "123123");
                    var userResult = await _userManager.CreateAsync(user, "123123");

                    await _userManager.AddToRoleAsync(user, BasicConstants.Manager);
                }

                if (isDbCreated && !context.Vacations.Any())
                {
                    var employeeUser = context.Users.First(u => u.Email == "emp@emp.com");
                    var managerUser = context.Users.First(u => u.Email == "admin@admin.com");

                    var vacations = new List<Vacation>()
                    {
                        //Employee vacations
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Egypt Diving Trip",
                            Status = VacationStatus.Approved,
                            TrackingUserId = employeeUser.Id,
                            TrackingUser = employeeUser
                        },
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Chillin",
                            Status = VacationStatus.Pending,
                            TrackingUserId = employeeUser.Id,
                            TrackingUser = employeeUser
                        },
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Something",
                            Status = VacationStatus.Declined,
                            TrackingUserId = employeeUser.Id,
                            TrackingUser = employeeUser,
                            DeclineNote = "Who knows?"
                        },

                        //Manager Vacations
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Trail run",
                            Status = VacationStatus.Approved,
                            TrackingUserId = managerUser.Id,
                            TrackingUser = managerUser
                        },
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Trip to Titanik",
                            Status = VacationStatus.Pending,
                            TrackingUserId = managerUser.Id,
                            TrackingUser = managerUser
                        },
                        new Vacation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartDate = DateTime.UtcNow.AddDays(3),
                            EndDate = DateTime.UtcNow.AddDays(7),
                            Description = "Walk around",
                            Status = VacationStatus.Declined,
                            TrackingUserId = managerUser.Id,
                            TrackingUser = managerUser,
                            DeclineNote = "Too busy?"
                        }
                    };

                    context.Vacations.AddRange(vacations);
                }

                context.SaveChanges();
            }
        }
    }
}