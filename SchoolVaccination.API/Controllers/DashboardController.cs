using DataHelper.Entities;
using SchoolVaccination.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace SchoolVaccination.API.Controllers
{
    public class DashboardController : PageController<DashboardModel, BaseSearchEntity>
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        protected override PaginedList<DashboardModel> GetList(BaseSearchEntity searchModel)
        {
            return _dashboardService.GetList(searchModel);
        }
    }
}
