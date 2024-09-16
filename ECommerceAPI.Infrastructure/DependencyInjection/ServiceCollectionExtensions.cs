using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Infrastructure.Persistance;
using MongoDB.Driver;

namespace ECommerceAPI.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Register the database context with dependency injection
        //    //services.AddDbContext<ApplicationDbContext>(options =>
        //    //    options.UseSqlServer(
        //    //        configuration.GetConnectionString("DefaultConnection"),
        //    //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        //    // Register other infrastructure services (e.g., logging, email service, external APIs)
        //    // Example: services.AddTransient<IEmailService, EmailService>();

        //    // Register additional infrastructure dependencies
        //    // Example: services.AddScoped<IFileStorageService, FileStorageService>();

        //    return services;
        //}

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string mongoConnectionString, string databaseName)
        {
            services.AddSingleton<IMongoClient>(s => new MongoClient(mongoConnectionString));
            services.AddSingleton(s => s.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
            services.AddTransient<ProductRepository>();

            return services;
        }
    }
}
