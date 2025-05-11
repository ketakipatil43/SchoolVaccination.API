using DataHelper.Entities;
using SchoolVaccination.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace SchoolVaccination.API.Controllers
{
    public class VaccinationDriveController : PageController<VaccinationDriveModel, BaseSearchEntity>
    {
        private readonly IVaccinationDriveService _vaccinationDriveService;
        public VaccinationDriveController(IVaccinationDriveService studentsService)
        {
            _vaccinationDriveService = studentsService;
        }
        protected override void InternalUpdateNewDetails(VaccinationDriveModel newEntity)
        {
            _vaccinationDriveService.Add(newEntity);
        }
        protected override PaginedList<VaccinationDriveModel> GetList(BaseSearchEntity searchModel)
        {
            return _vaccinationDriveService.GetList(searchModel);
        }
        protected override void InternalUpdateExistingDetails(VaccinationDriveModel updatedEntity)
        {
            _vaccinationDriveService.Update(updatedEntity);
        }
        protected override VaccinationDriveModel GetSingleRecord(BaseSearchEntity searchModel)
        {
            return _vaccinationDriveService.GetDetail(searchModel);
        }
    }
}
