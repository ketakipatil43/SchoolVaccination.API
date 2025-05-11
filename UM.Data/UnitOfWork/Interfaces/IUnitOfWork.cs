using UM.Data.EFData.Interfaces;

namespace UM.Data.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ICreateAccountRepo CreateAccountRepo { get; }
        IStudentsRepo StudentsRepo { get; }
        IVaccinationDriveRepo VaccinationDriveRepo { get; }
        IVaccinationStudentMapperRepo VaccinationStudentMapperRepo { get; }
        void Save();
    }
}
