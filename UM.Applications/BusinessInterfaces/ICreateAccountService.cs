using DataHelper.Entities;
using UM.Core.BaseEntities;
using UM.Core.ModelEntities;

namespace UM.Applications.BusinessInterfaces
{
    public interface ICreateAccountService
    {
        void Add(CreateAccountModel entity);
        void Update(CreateAccountModel entity);
        PaginedList<CreateAccountModel> GetList(BaseSearchEntity baseSearch);
        CreateAccountModel GetDetail(BaseSearchEntity search);
    }
}
