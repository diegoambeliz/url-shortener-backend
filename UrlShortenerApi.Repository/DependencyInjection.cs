using Microsoft.Extensions.DependencyInjection;
using UrlShortenerApi.Repository.Repositories;

namespace UrlShortenerApi.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<UserRepository>();
            services.AddSingleton<LinkRepository>();
            return services;
        }
    }
}
