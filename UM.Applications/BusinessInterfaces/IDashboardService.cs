using DataHelper.Entities;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface IDashboardService
    {
        PaginedList<DashboardModel> GetList(BaseSearchEntity baseSearch);
    }
}
