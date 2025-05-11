using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UM.Core.DBEntities
{
    public class VaccinationStudentMapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal UniqueId { get; set; }
        public decimal VaccinationDriveUniuqId { get; set; }
        public decimal StudentUniqueId { get; set; }
        public bool IsVaccinated { get; set; }
        [ForeignKey(nameof(VaccinationDriveUniuqId))]
        public virtual VaccinationDrive VaccinationDrive { get; set; }
        [ForeignKey(nameof(StudentUniqueId))]
        public virtual Students Students { get; set; }
    }
}
