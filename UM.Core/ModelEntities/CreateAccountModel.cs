using UM.Core.BaseEntities;
using static UM.Core.HelperClasses.EnumFields;

namespace UM.Core.ModelEntities
{
    public class CreateAccountModel : BaseModelEntity
    {
        public decimal UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PinCode { get; set; }
        public string PassWord { get; set; }
        public GenderType Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string? AccessToken { get; set; }
    }
}
