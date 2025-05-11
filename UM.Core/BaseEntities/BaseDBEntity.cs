namespace UM.Core.BaseEntities
{
    public class BaseDBEntity
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
