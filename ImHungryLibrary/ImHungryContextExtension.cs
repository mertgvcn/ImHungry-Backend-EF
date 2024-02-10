using ImHungryBackendER;
using ImHungryLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImHungryLibrary
{
    public static class ImHungryContextExtension
    {
        //No need for update-database, this func does it when program starts
        public static IHost AutoMigrateDatabase(this IHost app)
        {
            using var scope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();

            using var context = scope.ServiceProvider.GetRequiredService<ImHungryContext>();

            var pendingMigrations = context.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                var originalTimeOut = context.Database.GetCommandTimeout();
                context.Database.SetCommandTimeout(30*60);
                context.Database.Migrate();
                context.Database.SetCommandTimeout(originalTimeOut);
            }

            return app;
        }

        public static void BuildIndex(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().HasIndex(a => new { a.OwnerId });
            modelBuilder.Entity<User>().HasIndex(a => new { a.CurrentLocationId });
        }
        
    }

}
