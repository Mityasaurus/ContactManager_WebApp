using ContactManager_WebApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager_WebApp.DataAccess;

public partial class ContactManagerContext : DbContext
{
    public ContactManagerContext()
    {
    }

    public ContactManagerContext(DbContextOptions<ContactManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ADMIN-PC\\SQLEXPRESS;Database=ContactManager;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC07D2ECF9CD");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
