namespace MotoBest.Data
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using MotoBest.Models;
    using MotoBest.Models.Common;

    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(DatabaseConfig.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advert>()
                        .HasIndex(a => new { a.AdvertProviderId, a.RemoteId })
                        .IsUnique();

            modelBuilder.Entity<Brand>()
                        .HasMany(brand => brand.Models)
                        .WithOne(model => model.Brand)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Region>()
                        .HasMany(region => region.Towns)
                        .WithOne(town => town.Region)
                        .OnDelete(DeleteBehavior.Restrict);

            SetUniqueConstraints(modelBuilder,
                                    typeof(AdvertProvider),
                                    typeof(BodyStyle),
                                    typeof(Brand),
                                    typeof(Color),
                                    typeof(Condition),
                                    typeof(Engine),
                                    typeof(EuroStandard),
                                    typeof(Region),
                                    typeof(Town),
                                    typeof(Transmission));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Advert> Adverts { get; set; }

        public DbSet<AdvertProvider> AdvertProviders { get; set; }

        public DbSet<BodyStyle> BodyStyles { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Condition> Conditions { get; set; }

        public DbSet<Engine> Engines { get; set; }

        public DbSet<EuroStandard> EuroStandards { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Transmission> Transmissions { get; set; }

        private static void SetUniqueConstraints(ModelBuilder modelBuilder, params Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(BaseNameableModel)))
                {
                    modelBuilder.Entity(type).HasIndex("Name").IsUnique();
                }

                if (type.IsSubclassOf(typeof(BaseTypeableModel)))
                {
                    modelBuilder.Entity(type).HasIndex("Type").IsUnique();
                }
            }
        }
    }
}
