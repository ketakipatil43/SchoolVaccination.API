using DataHelper.Entities;
using SchoolVaccination.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace SchoolVaccination.API.Controllers
{
    public class VaccinationStudentMapperController : PageController<VaccinationStudentMapperModel, BaseSearchEntity>
    {
        private readonly IVaccinationStudentMapperService _vaccinationStudentMapperService;
        public VaccinationStudentMapperController(IVaccinationStudentMapperService vaccinationStudentMapperService)
        {
            _vaccinationStudentMapperService = vaccinationStudentMapperService;
        }
        protected override PaginedList<VaccinationStudentMapperModel> GetList(BaseSearchEntity searchModel)
        {
            return _vaccinationStudentMapperService.GetList(searchModel);
        }
        protected override VaccinationStudentMapperModel GetSingleRecord(BaseSearchEntity searchModel)
        {
            return _vaccinationStudentMapperService.GetDetail(searchModel);
        }
        protected override void InternalUpdateNewDetails(VaccinationStudentMapperModel newEntity)
        {
            _vaccinationStudentMapperService.Add(newEntity);
        }
        protected override void InternalUpdateExistingDetails(VaccinationStudentMapperModel updatedEntity)
        {
            _vaccinationStudentMapperService.Update(updatedEntity);
        }
    }
}
