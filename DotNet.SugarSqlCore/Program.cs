using DotNet.SugarSqlCore.Middlewares;
using DotNet.SugarSqlCore.Repositories;
using SugarSqlCore;

namespace DotNet.SugarSqlCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var dbConnectionOptions = new DbConnectionOptions();
            builder.Configuration.GetSection(DbConnectionOptions.DbConnectionOption).Bind(dbConnectionOptions);
            builder.Services.AddSugarSql(options =>
            {
                options.Type = dbConnectionOptions.Type;
                options.ConnectionString = dbConnectionOptions.ConnectionString;
            });

            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI();

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
