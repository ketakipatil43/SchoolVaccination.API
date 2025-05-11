using UM.Applications.BusinessClasses;
using UM.Applications.BusinessInterfaces;
using UM.Data.UnitOfWork.Interfaces;
using UM.Data.UnitOfWork.Repositories;

namespace SchoolVaccination.API.Core_DI
{
    public static class CommonDependencyResolver
    {
        public static IServiceCollection AddBALDependencies(this IServiceCollection services)
        {

            services.AddScoped<ICreateAccountService, CreateAccountService>();
            services.AddScoped<IStudentsService, StudentsService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IVaccinationDriveService, VaccinationDriveService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IVaccinationStudentMapperService, VaccinationStudentMapperService>();
            return services;
        }
        public static IServiceCollection AddDACDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
