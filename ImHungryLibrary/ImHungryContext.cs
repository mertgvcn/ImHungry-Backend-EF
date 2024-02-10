using ImHungryLibrary;
using ImHungryLibrary.Configruations;
using ImHungryLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace ImHungryBackendER
{
    public class ImHungryContext : DbContext
    {
        public ImHungryContext(DbContextOptions<ImHungryContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<RestaurantLocation> RestaurantsLocations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Ingredient> Ingredients { get; set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<CreditCard> CreditCards { get; set;}
        public DbSet<Cart> CartItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        private DbSet<AuditLog> Auditlog { get; set; }

        //Save changes'i override edip log ekleyebiliriz.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var ChangedObject = ChangeTracker.Entries().Where(a => a.State == EntityState.Modified || a.State == EntityState.Added || a.State == EntityState.Deleted).ToList();
    
            foreach (var item in ChangedObject)
            {
               
                this.Auditlog.Add(new AuditLog()
                {
                    Object = item.Entity.ToString(),
                    Name = "MERT Kullanici ekle EKLE AMK",
                    Mutation = item.State.ToString()
                }); 

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configurations
            //modelBuilder.ApplyConfiguration(new RestaurantConfiguration());

            //modelBuilder.BuildIndex();

            //modelBuilder.Entity<User>().HasMany(a => a.CartItems) //user silinince cart itemlarıda silinecek örneği
            //   .WithOne(a => a.User).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
