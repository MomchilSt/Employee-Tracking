using AutoMapper;
using EmployeeTracking.Data.Data.Models;
using EmployeeTracking.ViewModel;

namespace EmployeeTracking.Web.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Vacation, VacationViewModel>();
            CreateMap<VacationViewModel, Vacation>();
        }
    }
}
