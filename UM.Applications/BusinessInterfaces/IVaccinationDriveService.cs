using DataHelper.Entities;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface IVaccinationDriveService
    {
        void Add(VaccinationDriveModel entity);
        void Update(VaccinationDriveModel entity);
        PaginedList<VaccinationDriveModel> GetList(BaseSearchEntity baseSearch);
        VaccinationDriveModel GetDetail(BaseSearchEntity search);
    }
}
