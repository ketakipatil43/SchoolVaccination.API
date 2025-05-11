using DataHelper.Entities;
using SchoolVaccination.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;
using UM.Core.SearchEntities;

namespace SchoolVaccination.API.Controllers
{
    public class StudentController : PageController<StudentsModel, StudentSearchModel>
    {
        private readonly IStudentsService _studentsService;
        public StudentController(IStudentsService studentsService)
        {
            _studentsService = studentsService;
        }
        protected override void InternalUpdateNewDetails(StudentsModel newEntity)
        {
            _studentsService.Add(newEntity);
        }
        protected override PaginedList<StudentsModel> GetList(StudentSearchModel searchModel)
        {
            return _studentsService.GetList(searchModel);
        }
        protected override void InternalUpdateExistingDetails(StudentsModel updatedEntity)
        {
            _studentsService.Update(updatedEntity);
        }
        protected override StudentsModel GetSingleRecord(StudentSearchModel searchModel)
        {
            return _studentsService.GetDetail(searchModel);
        }
    }
}
