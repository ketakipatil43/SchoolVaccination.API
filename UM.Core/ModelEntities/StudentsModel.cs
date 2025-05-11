using UM.Core.BaseEntities;

namespace UM.Core.ModelEntities
{
    public class StudentsModel : BaseModelEntity
    {
        public decimal UniqueId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Id { get; set; }
        public List<VaccinationStudentMapperModel> vaccinationStudentMapper { get; set; }
    }
}
