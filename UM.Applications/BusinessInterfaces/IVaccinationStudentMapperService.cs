using DataHelper.Entities;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface IVaccinationStudentMapperService
    {
        void Add(VaccinationStudentMapperModel entity);
        void Update(VaccinationStudentMapperModel entity);
        PaginedList<VaccinationStudentMapperModel> GetList(BaseSearchEntity baseSearch);
        VaccinationStudentMapperModel GetDetail(BaseSearchEntity search);
    }
}
