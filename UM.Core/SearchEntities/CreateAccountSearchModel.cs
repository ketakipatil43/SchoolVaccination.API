using UM.Core.BaseEntities;

namespace UM.Core.SearchEntities
{
    public class CreateAccountSearchModel : BaseSearchEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }    
    }
}
