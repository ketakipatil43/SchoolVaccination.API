using UM.Core.BaseEntities;

namespace UM.Core.ModelEntities
{
    public class VaccinationStudentMapperModel : BaseModelEntity
    {
        public decimal UniqueId { get; set; }
        public decimal VaccinationDriveUniuqId { get; set; }
        public decimal StudentUniqueId { get; set; }
        public bool IsVaccinated { get; set; }
    }
}
