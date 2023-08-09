using AutoMapper;
using EmployeeTracking.Common.Constants;
using EmployeeTracking.Data.Data.Models.Enums;
using EmployeeTracking.Services.Interfaces;
using EmployeeTracking.ViewModel;
using EmployeeTracking.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTracking.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string IndexLoggedIn = "IndexLoggedIn";

        private readonly IVacationService _vacationService;
        private readonly IMapper _mapper;

        public HomeController(IVacationService vacationService, IMapper mapper)
        {
            this._vacationService = vacationService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            if (this.User?.Identity?.IsAuthenticated != null && this.User.Identity.IsAuthenticated)
            {
                return View(IndexLoggedIn, ArrangeViewModel(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message)
        {
            return View(new ErrorViewModel 
            { 
                Message = message
            });
        }

        private VacationHomeViewModel ArrangeViewModel(string id)
        {
            List<VacationViewModel> allPendingVacations = new List<VacationViewModel>();
            if (this.User.IsInRole(BasicConstants.Manager))
            {
                allPendingVacations = _vacationService.GetAllPendingVacations()
                    .Select(v => _mapper.Map<VacationViewModel>(v)).ToList();
            }

            var approvedVacations = _vacationService.GetVacationsByStatus(id, VacationStatus.Approved)
                .Select(v => _mapper.Map<VacationViewModel>(v)).ToList();

            var pendingVacations = _vacationService.GetVacationsByStatus(id, VacationStatus.Pending)
                .Select(v => _mapper.Map<VacationViewModel>(v)).ToList();

            var declinedVacations = _vacationService.GetVacationsByStatus(id, VacationStatus.Declined)
                .Select(v => _mapper.Map<VacationViewModel>(v)).ToList();

            return new VacationHomeViewModel()
            {
                AllPendingVacations = allPendingVacations,
                ApprovedVacations = approvedVacations,
                PendingVacations = pendingVacations,
                DeclinedVacations = declinedVacations
            };
        }
    }
}