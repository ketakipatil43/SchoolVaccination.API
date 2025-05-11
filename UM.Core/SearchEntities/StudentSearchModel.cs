using UM.Core.BaseEntities;

namespace UM.Core.SearchEntities
{
    public class StudentSearchModel : BaseSearchEntity
    {
        public bool IsFilterApplied { get; set; } = false;
        public decimal StudentId { get; set; }
        public decimal VaccinationDriveId { get; set; }
    }
}
