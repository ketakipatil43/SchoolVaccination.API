using UM.Core.ModelEntities;
using UM.Core.SearchEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface ILoginService
    {
        CreateAccountModel GetDetail(CreateAccountSearchModel search);
    }
}
