namespace packt_asp_net_core_and_angular2.Data
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata;
  using Data.Items;
  using Data.Comments;
  using Data.Users;

  public class ApplicationDbContext : DbContext
  {
      public ApplicationDbContext(DbContextOptions options) : base(options)
      {
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<ApplicationUser>()
          .HasMany(u => u.Items)
          .WithOne(i => i.Author);
        builder.Entity<ApplicationUser>()
          .HasMany(u => u.Comments)
          .WithOne(c => c.Author)
          .HasPrincipalKey(u => u.Id);
        
        builder.Entity<Item>().ToTable("Items");
        builder.Entity<Item>()
          .Property(i => i.Id)
          .ValueGeneratedOnAdd();
        builder.Entity<Item>()
          .HasOne(i => i.Author)
          .WithMany(u => u.Items);
        builder.Entity<Item>()
          .HasMany(i => i.Comments)
          .WithOne(c => c.Item);

        builder.Entity<Comment>().ToTable("Comments");
        builder.Entity<Comment>()
          .HasOne(c => c.Author)
          .WithMany(u => u.Comments)
          .HasForeignKey(c => c.UserId)
          .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Comment>()
          .HasOne(c => c.Item)
          .WithMany(i => i.Comments);
        builder.Entity<Comment>()
          .HasOne(c => c.Parent)
          .WithMany(c => c.Children);
        builder.Entity<Comment>()
          .HasMany(c => c.Children)
          .WithOne(c => c.Parent);
      }

      public DbSet<Item> Items { get; set; }

      public DbSet<Comment> Comments { get; set; }

      public DbSet<ApplicationUser> Users { get; set; }
  }
}