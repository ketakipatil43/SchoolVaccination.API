using UM.Applications.BusinessClasses;
using UM.Applications.BusinessInterfaces;
using UM.Data.UnitOfWork.Interfaces;
using UM.Data.UnitOfWork.Repositories;

namespace UM.API.Core_DI
{
    public static class CommonDependencyResolver
    {
        public static IServiceCollection AddBALDependencies(this IServiceCollection services)
        {
            
            services.AddScoped<ICreateAccountService, CreateAccountService>();
            return services;
        }
        public static IServiceCollection AddDACDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
