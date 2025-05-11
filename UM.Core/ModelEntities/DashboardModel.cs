namespace UM.Core.ModelEntities
{
    public class DashboardModel : VaccinationDriveModel
    {
        public int totalStudent { get; set; }
        public int totalVaccinatedStudent { get; set; }
        public decimal  vaccinatedStudentPercentage { get; set; }
    }
}
