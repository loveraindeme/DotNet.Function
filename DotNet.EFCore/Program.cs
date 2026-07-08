using DotNet.EFCore.Database;
using DotNet.EFCore.Middlewares;
using EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotNet.EFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddEFCore<AppDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);
                optionsBuilder
                    .ConfigureWarnings(options => options.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning))
                    .LogTo(
                        Console.WriteLine,
                        LogLevel.Information,
                        DbContextLoggerOptions.Level | DbContextLoggerOptions.LocalTime)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorize();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}
