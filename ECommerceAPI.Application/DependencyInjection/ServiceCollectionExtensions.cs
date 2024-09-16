using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.Features;
using ECommerceAPI.Application.Interfaces;

namespace ECommerceAPI.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register MediatR for handling CQRS commands and queries
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation validators from the assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register other application services, e.g., AutoMapper
            services.AddScoped<IProductService, ProductService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add additional services if needed
            // Example: services.AddScoped<IYourService, YourServiceImplementation>();

            return services;
        }
    }
}
