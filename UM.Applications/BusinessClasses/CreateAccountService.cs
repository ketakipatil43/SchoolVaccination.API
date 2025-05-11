using AutoMapper;
using DataHelper.Entities;
using DataHelper.HelperClasses;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;
using UM.Data.UnitOfWork.Interfaces;

namespace UM.Applications.BusinessClasses
{
    public class CreateAccountService : ICreateAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAccountService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public void Add(CreateAccountModel entity)
        {
            var result = _mapper.Map<CreateAccount>(entity);
            _unitOfWork.CreateAccountRepo.Add(result);
        }

        public CreateAccountModel GetDetail(BaseSearchEntity search)
        {
            BaseSpecification<CreateAccount> spec = new()
            {
                Criteria = a => a.UniqueId == search.PrimaryKey
            };
            return _mapper.Map<CreateAccountModel>(_unitOfWork.CreateAccountRepo.GetUniqueRecordBySpec(spec));
        }

        public PaginedList<CreateAccountModel> GetList(BaseSearchEntity baseSearch)
        {
            var result = _unitOfWork.CreateAccountRepo.GetListByPage(baseSearch.StartIndex, baseSearch.PageSize, null, new BaseSpecification<CreateAccount>());
            return result == null ? null : new PaginedList<CreateAccountModel> { Items = _mapper.Map<List<CreateAccountModel>>(result.Items), TotalItems = result.TotalItems };
        }

        public void Update(CreateAccountModel entity)
        {
            var result = _mapper.Map<CreateAccount>(entity);
            result.ModifiedDate = DateTime.Now;
            _unitOfWork.CreateAccountRepo.Update(result, ["UniqueId", "CreatedDate"]);
        }
    }
}
