using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.Core.Abstractions;
using Pcf.Preferences.DataAccess;
using Pcf.Preferences.DataAccess.Data;
using Pcf.Preferences.DataAccess.Repositories;
namespace Pcf.Preferences.WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<PreferenceRepository>();
        builder.Services.AddScoped<IPreferenceRepository>(sp =>
            new CachedPreferenceRepository(
                sp.GetRequiredService<IDistributedCache>(),
                sp.GetRequiredService<PreferenceRepository>()
            )
        );

        builder.Services.AddScoped<IDbInitializer, EfDbInitializer>();
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PromocodeFactoryPreferencesDb"));
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("PromocodeFactoryPreferencesRedis");
            options.InstanceName = "PreferenceCache";
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.InitializeDb();
        }

        app.Run();
    }
}
