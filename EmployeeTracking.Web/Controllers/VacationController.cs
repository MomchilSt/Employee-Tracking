using EmployeeTracking.InputModels;
using EmployeeTracking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTracking.Web.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;

        public VacationController(IVacationService vacationService, IUserService userService)
        {
            this._vacationService = vacationService;
            this._userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userService.GetById(userId);
            ViewData["Days"] = user.VacationDays;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacationInputModel inputModel)
        {
            if (ModelState.IsValid && inputModel.EndDate > inputModel.StartDate) 
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var response = await _vacationService.Create(inputModel, userId);

                return response ? RedirectToAction("Index", "Home") : RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Error", "Home", new { message = "Error occured while trying to book vacation!" });
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string vacationId)
        {
            var response = await this._vacationService.ApproveVacation(vacationId);

            if (!response)
            {
                return RedirectToAction("Error", "Home", new { message = "Error occured while trying to approve vacation!" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Decline(string message, string vacationId)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(vacationId))
            {
                return RedirectToAction("Error", "Home", new { message = "Bad Request! Missing message or vacation id!" });
            }

            var response = await this._vacationService.DeclineVacation(message, vacationId);

            if (!response)
            {
                return RedirectToAction("Error", "Home", new { message = "Error occured while trying to decline vacation!" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
