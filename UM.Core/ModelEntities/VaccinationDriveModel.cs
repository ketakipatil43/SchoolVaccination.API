using UM.Core.BaseEntities;
using UM.Core.DBEntities;

namespace UM.Core.ModelEntities
{
    public class VaccinationDriveModel : BaseModelEntity
    {
        public decimal UniqueId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfDrive { get; set; }
        public int TotalDoses { get; set; }
        public int NumberOfAvailableDoses { get; set; }
        public List<VaccinationStudentMapperModel> VaccinationStudentMapper { get; set; }
    }
}
