using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distributed.Cache.Config
{
    public static class RedisConfiguration
    {
        private const string REDIS_SETTINGS = "Redis:Connection";

        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection(REDIS_SETTINGS).Get<string>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
    }
}
