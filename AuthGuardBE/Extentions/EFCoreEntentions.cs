using AuthAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthAppAPI.Extentions
{
    public static class EFCoreEntentions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services,IConfiguration config)
        {
            // Configuring the dependency inejction for the DBcontext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("ConString")));
            return services;
        }
    }
}
