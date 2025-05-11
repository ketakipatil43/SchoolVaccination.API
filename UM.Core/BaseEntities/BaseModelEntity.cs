namespace UM.Core.BaseEntities
{
    public class BaseModelEntity
    {
        public int Flag { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
