using DataHelper.Entities;
using UM.Core.ModelEntities;
using UM.Core.SearchEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface IStudentsService
    {
        void Add(StudentsModel entity);
        void Update(StudentsModel entity);
        PaginedList<StudentsModel> GetList(StudentSearchModel baseSearch);
        StudentsModel GetDetail(StudentSearchModel search);
    }
}
