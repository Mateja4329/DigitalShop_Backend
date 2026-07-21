using DigitalShop.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Infrastructure.Data
{
    // ======================================================================================
    // In backend this is called OMR (Object Relational Mapping).
    // DbContext is the primary class that is responsible for interacting with the database.
    // It manages the entity objects during runtime, which includes populating objects with
    // data from a database, change tracking, and persisting data to the database.

    // We use this class to communicate with the database and perform CRUD operations.
    // ======================================================================================
    public class DataContext : DbContext
    {
        // This constructor allows external configuration.
        // Through these options, the application (later in Program.cs)
        // will pass the connection string that we arranged in appsettings.json.
        // Syntax ": base(options)" just passes those configurations to the parent
        // DbContext class which actually does all the heavy lifting in the background.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // this property represents a specific table in the database.
        // In this line you said: "I want there to be a table in the database
        // called Products, and each row in that table will correspond to
        // the structure of the Product class".
        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        // Mock Data: This method is used to seed the database with initial data.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    // IMPORTANT: For seed data, you must always enter a fixed GUID (i.e., hardcoded). 
                    // otherwise it will create a new Guid every time and confuse the database.
                    ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProductName = "Asus TUF F15",
                    ProductDescription = "Laptop that doesn't do beep boop or something idk",
                    ProductCategory = Entities.Enums.Category.Laptop,
                    // We must not use DateTime.UtcNow in seed data because
                    // it changes constantly, we use a fixed date
                    DateCreated = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    Price = 1500.00m
                },
                new Product
                {
                    ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ProductName = "Asus Desktop",
                    ProductDescription = "PC that does do beep boop or something idk",
                    ProductCategory = Entities.Enums.Category.Desktop,
                    DateCreated = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    Price = 1000.00m
                }
                );
        }
    }
}
