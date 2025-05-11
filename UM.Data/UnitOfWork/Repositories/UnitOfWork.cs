using UM.Core;
using UM.Data.EFData.Interfaces;
using UM.Data.EFData.Repositories;
using UM.Data.UnitOfWork.Interfaces;

namespace UM.Data.UnitOfWork.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICreateAccountRepo createAccountRepo;
        private IStudentsRepo studentsRepo;
        private IVaccinationDriveRepo vaccinationDriveRepo;
        private IVaccinationStudentMapperRepo vaccinationStudentMapperRepo;
        private readonly UmContext _context;

        public UnitOfWork(UmContext context)
        {
            _context = context;
        }
        public ICreateAccountRepo CreateAccountRepo { get { return createAccountRepo ??= new CreateAccountRepo(_context); } }
        public IStudentsRepo StudentsRepo { get { return studentsRepo ??= new StudentsRepo(_context); } }
        public IVaccinationDriveRepo VaccinationDriveRepo { get { return vaccinationDriveRepo ??= new VaccinationDriveRepo(_context); } }
        public IVaccinationStudentMapperRepo VaccinationStudentMapperRepo { get { return vaccinationStudentMapperRepo ??= new VaccinationStudentMapperRepo(_context); } }
        public void Save()
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
