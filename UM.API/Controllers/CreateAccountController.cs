using DataHelper.Entities;
using UM.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace UM.API.Controllers
{
    public class CreateAccountController : PageController<CreateAccountModel, BaseSearchEntity>
    {
        private readonly ICreateAccountService _createAccountService;
        public CreateAccountController(ICreateAccountService createAccountService)
        {
            _createAccountService = createAccountService;
        }
        protected override void InternalUpdateNewDetails(CreateAccountModel newEntity)
        {
            _createAccountService.Add(newEntity);
        }
        protected override PaginedList<CreateAccountModel> GetList(BaseSearchEntity searchModel)
        {
            return _createAccountService.GetList(searchModel);
        }
        protected override void InternalUpdateExistingDetails(CreateAccountModel updatedEntity)
        {
            _createAccountService.Update(updatedEntity);
        }
        protected override CreateAccountModel GetSingleRecord(BaseSearchEntity searchModel)
        {
            return _createAccountService.GetDetail(searchModel);
        }
    }
}
