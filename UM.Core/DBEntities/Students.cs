using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UM.Core.BaseEntities;

namespace UM.Core.DBEntities
{
    public class Students : BaseDBEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal UniqueId { get; set; }
        public string Name { get; set; }    
        public string Class { get; set; }
        public string Id { get; set; }
        public ICollection<VaccinationStudentMapper> VaccinationStudentMapper { get; set; }
    }
}
