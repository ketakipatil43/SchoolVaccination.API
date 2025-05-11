using DataHelper.EFData.Common.Interfaces;
using UM.Core;
using UM.Core.DBEntities;

namespace UM.Data.EFData.Interfaces
{
    public interface IVaccinationDriveRepo : IGenericBaseRepo<VaccinationDrive, UmContext>
    {
    }
    public interface IVaccinationStudentMapperRepo : IGenericBaseRepo<VaccinationStudentMapper, UmContext>
    {
    }
}
