using AutoMapper;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;

namespace UM.Core.Profiles
{
    internal class CreateAccountProfile : Profile
    {
        public override string ProfileName => nameof(CreateAccountProfile);
        public CreateAccountProfile()
        {
            CreateMap<CreateAccount, CreateAccountModel>()
            .ReverseMap();
        }
    }
}
