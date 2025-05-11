using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UM.Core.BaseEntities;

namespace UM.Core.DBEntities
{
    public class VaccinationDrive : BaseDBEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal UniqueId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfDrive { get; set; }
        public int TotalDoses { get; set; }
        public int NumberOfAvailableDoses { get; set; }
        public ICollection<VaccinationStudentMapper> VaccinationStudentMapper { get; set; }
    }
}
