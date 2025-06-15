using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorProyectoPostres.Models.DatabaseModels;

public partial class PostresContext : DbContext
{
    public PostresContext()
    {
    }

    public PostresContext(DbContextOptions<PostresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ARCHIVO> ARCHIVOs { get; set; }

    public virtual DbSet<ARCHIVO_POSTRE> ARCHIVO_POSTREs { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<MENU> MENUs { get; set; }

    public virtual DbSet<PORCION> PORCIONs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.179;Database=Postres;user id=sa;password=0984220903jA@;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ARCHIVO>(entity =>
        {
            entity.HasKey(e => e.ARC_ID);

            entity.ToTable("ARCHIVO");

            entity.Property(e => e.ARC_ARCHIVO).IsRequired();
            entity.Property(e => e.ARC_EXTENSION)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ARC_NOMBRE)
                .IsRequired()
                .HasMaxLength(150);
        });

        modelBuilder.Entity<ARCHIVO_POSTRE>(entity =>
        {
            entity.HasKey(e => e.ARP_ID).HasName("PK__ARCHIVO___355879E78B8B746A");

            entity.ToTable("ARCHIVO_POSTRE");

            entity.HasOne(d => d.ARP_ARCHIVONavigation).WithMany(p => p.ARCHIVO_POSTREs)
                .HasForeignKey(d => d.ARP_ARCHIVO)
                .HasConstraintName("FK_ARCHIVOPOSTRE_ARCHIVO");

            entity.HasOne(d => d.ARP_MENUNavigation).WithMany(p => p.ARCHIVO_POSTREs)
                .HasForeignKey(d => d.ARP_MENU)
                .HasConstraintName("FK_ARCHIVOPOSTRE_POSTRE");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<MENU>(entity =>
        {
            entity.HasKey(e => e.MEN_ID).HasName("PK__MENU__836B5EB1B3D08FFE");

            entity.ToTable("MENU");

            entity.Property(e => e.MEN_DESCRIPCION)
                .HasMaxLength(600)
                .IsUnicode(false);
            entity.Property(e => e.MEN_ESTADO).HasDefaultValue(true);
            entity.Property(e => e.MEN_NOMBRE)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PORCION>(entity =>
        {
            entity.HasKey(e => e.POR_ID).HasName("PK__PORCION__C1AD4BD34A38EA73");

            entity.ToTable("PORCION");

            entity.Property(e => e.POR_CANTIDAD)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.POR_NOMBRE)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.POR_PRECIO).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.POR_MENUNavigation).WithMany(p => p.PORCIONs)
                .HasForeignKey(d => d.POR_MENU)
                .HasConstraintName("FK_PORCION_MENU");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
