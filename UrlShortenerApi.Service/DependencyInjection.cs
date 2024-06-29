using Microsoft.Extensions.DependencyInjection;
using UrlShortenerApi.Service.Services;

namespace UrlShortenerApi.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddSingleton<UserService>();
            services.AddSingleton<LinkService>();
            return services;
        }
    }
}
