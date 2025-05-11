using AutoMapper;
using DataHelper.HelperClasses;
using UM.Applications.BusinessInterfaces;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;
using UM.Core.SearchEntities;
using UM.Data.UnitOfWork.Interfaces;

namespace UM.Applications.BusinessClasses
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public CreateAccountModel GetDetail(CreateAccountSearchModel search)
        {
            BaseSpecification<CreateAccount> spec = new()
            {
                Criteria = a => a.UserId == search.UserName && a.PassWord==search.Password
            };
            return _mapper.Map<CreateAccountModel>(_unitOfWork.CreateAccountRepo.GetUniqueRecordBySpec(spec));
        }
    }
}
