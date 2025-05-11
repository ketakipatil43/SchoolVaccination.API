using AutoMapper;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;

namespace UM.Core.Profiles
{
    internal class VaccinationStudentMapperProfile : Profile
    {
        public override string ProfileName => nameof(VaccinationStudentMapperProfile);
        public VaccinationStudentMapperProfile()
        {
            CreateMap<VaccinationStudentMapper, VaccinationStudentMapperModel>()
            .ReverseMap();
        }
    }
}
