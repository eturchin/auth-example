using Microsoft.EntityFrameworkCore;

namespace TestProject.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasKey(x => x.Id);

        builder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Role>()
            .HasKey(x => x.Id);

        builder.Entity<UserRole>()
            .HasKey(x => x.Id);

        builder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        builder.Entity<Role>()
            .HasData(
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "user"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "admin"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "super-admin"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "support"
                });
    }
}