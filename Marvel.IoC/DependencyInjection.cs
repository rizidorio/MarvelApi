using Marvel.Application.Interfaces;
using Marvel.Application.Services;
using Marvel.Domain.Interfaces;
using Marvel.Infra.Context;
using Marvel.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marvel.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                                     b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ICharactersRepository, CharactersRepository>();
            services.AddScoped<ICharactersService, CharactersService>();
            services.AddScoped<ICharactersServerService, CharactersServerService>();

            var myHandlers = AppDomain.CurrentDomain.Load("Marvel.Application");
            services.AddMediatR(myHandlers);

            return services;
        }
    }
}
