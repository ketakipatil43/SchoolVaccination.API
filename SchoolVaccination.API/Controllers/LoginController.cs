using DataHelper.Entities;
using SchoolVaccination.API.Controllers.Common;
using UM.Applications.BusinessInterfaces;
using UM.Core.ModelEntities;
using UM.Core.SearchEntities;

namespace SchoolVaccination.API.Controllers
{
    [LTAuthorize(AuthorizeLevel.None)]
    public class LoginController : SearchController<CreateAccountModel, CreateAccountSearchModel>
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        protected override CreateAccountModel GetSingleRecord(CreateAccountSearchModel searchModel)
        {
            var result = _loginService.GetDetail(searchModel);
            if (result != null)
            {
                result.AccessToken = GetAccessToken(result);
            }
            return result;
        }
    }
}
