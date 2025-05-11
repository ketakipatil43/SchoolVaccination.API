using UM.Core;
using UM.Data.EFData.Interfaces;

namespace UM.Data.EFData.Repositories
{
    public class VaccinationDriveRepo : IVaccinationDriveRepo
    {
        public UmContext _dbContext { get; }

        public VaccinationDriveRepo(UmContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
    public class VaccinationStudentMapperRepo : IVaccinationStudentMapperRepo
    {
        public UmContext _dbContext { get; }

        public VaccinationStudentMapperRepo(UmContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
