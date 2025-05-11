using AutoMapper;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;

namespace UM.Core.Profiles
{
    internal class StudentsProfile : Profile
    {
        public override string ProfileName => nameof(StudentsProfile);
        public StudentsProfile()
        {
            CreateMap<Students, StudentsModel>()
            .ReverseMap();
        }
    }
}
