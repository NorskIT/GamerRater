using System.Data.SqlClient;
using GamerRater.Model;
using Microsoft.EntityFrameworkCore;

namespace GamerRater.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<GameRoot> Games { get; set; }
        public DbSet<GameCover> Covers { get; set; }
        public DbSet<Review> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<UserHasUserGroup> UserHasUserGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "Donau.hiof.no",
                InitialCatalog = "magnusp",
                UserID = "magnusp",
                Password = "6U8Ujvm7"

                /*
				DataSource = "(localdb)\\MSSQLLocalDB",
				InitialCatalog = "GamerRater",
				IntegratedSecurity = true
                */
            };

            optionsBuilder.UseSqlServer(builder.ConnectionString, b => b.MigrationsAssembly("GamerRater.Api"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne<GameRoot>()
                .WithMany(r => r.Reviews)
                .HasForeignKey(f => f.GameRootId);


            modelBuilder.Entity<UserHasUserGroup>()
                .HasKey(uug => new {uug.UserId, uug.UserGroupId});
            modelBuilder.Entity<UserHasUserGroup>()
                .HasOne(uug => uug.User)
                .WithMany(x => x.UserGroups)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserHasUserGroup>()
                .HasOne(sc => sc.UserGroup)
                .WithMany(c => c.Users)
                .HasForeignKey(bc => bc.UserGroupId);

            base.OnModelCreating(modelBuilder);
        }
    }
}