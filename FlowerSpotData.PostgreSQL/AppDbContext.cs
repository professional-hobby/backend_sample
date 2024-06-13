using FlowerSpotCore.ModelsRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlowerSpotData.PostgreSQL
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            var config = configuration.Build();
            string? connectionString1 = config.GetConnectionString("DefaultConnection");
            string connectionString = connectionString1 ?? "";

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<FlowerModel> Flowers { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<SightingModel> Sightings { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
