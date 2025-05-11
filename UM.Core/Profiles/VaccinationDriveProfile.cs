using AutoMapper;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;

namespace UM.Core.Profiles
{
    public class VaccinationDriveProfile : Profile
    {
        public override string ProfileName => nameof(VaccinationDriveProfile);
        public VaccinationDriveProfile()
        {
            CreateMap<VaccinationDrive, VaccinationDriveModel>()
            .ReverseMap();
        }
    }

    public class DashboardProfile : Profile
    {
        public override string ProfileName => nameof(DashboardProfile);
        public DashboardProfile()
        {
            CreateMap<VaccinationDrive, DashboardModel>()
                .ForMember(dst => dst.totalStudent, opt => opt.MapFrom(src => GetTotalStudentsEnrolledInDrive(src)))
                .ForMember(dst => dst.totalVaccinatedStudent, opt => opt.MapFrom(src => GetVaccinatedCount(src)))
                .ForMember(dst => dst.vaccinatedStudentPercentage, opt => opt.MapFrom(src => GetVaccinatedPercentage(src)))
            .ReverseMap();
        }

        private int GetTotalStudentsEnrolledInDrive(VaccinationDrive vaccinationDrive)
        {
            return vaccinationDrive.VaccinationStudentMapper.Where(a => a.StudentUniqueId != 0).Count();
        }
        private int GetVaccinatedCount(VaccinationDrive vaccinationDrive)
        {
            return vaccinationDrive.VaccinationStudentMapper.Where(a => a.IsVaccinated).ToList().Count;
        }

        private decimal GetVaccinatedPercentage(VaccinationDrive vaccinationDrive)
        {
            var totalStudent = GetTotalStudentsEnrolledInDrive(vaccinationDrive);
            if (totalStudent > 0)
            {
                var vaccinatedStudents = GetVaccinatedCount(vaccinationDrive);
                decimal vaccinatedPercentage = Convert.ToDecimal(vaccinatedStudents) / Convert.ToDecimal(totalStudent) * 100;
                return vaccinatedPercentage;
            }
            return 0;
        }
    }
}
