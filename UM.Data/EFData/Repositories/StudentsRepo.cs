using UM.Core;
using UM.Data.EFData.Interfaces;

namespace UM.Data.EFData.Repositories
{
    public class StudentsRepo : IStudentsRepo
    {
        public UmContext _dbContext { get; }

        public StudentsRepo(UmContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
