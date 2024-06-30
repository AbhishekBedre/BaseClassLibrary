using BaseClassLibrary.Interface;
using BaseClassLibrary.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BaseClassLibrary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBaseLibraryServices(this IServiceCollection services)
        {
            // Register your custom services here
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

            // Add more services as needed

            return services;
        }
    }

}
