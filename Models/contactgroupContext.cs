using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace contactgroupAPIefMySQL.Models
{
    public partial class contactgroupContext : DbContext
    {
        public contactgroupContext()
        {
        }

        public contactgroupContext(DbContextOptions<contactgroupContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cgroup> Cgroups { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Groupcontact> Groupcontacts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(System.Environment.GetEnvironmentVariable("DATABASE_URL"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8mb3");

            modelBuilder.Entity<Cgroup>(entity =>
            {
                entity.HasKey(e => e.Idgroups)
                    .HasName("PRIMARY");

                entity.ToTable("cgroups");

                entity.Property(e => e.Idgroups).HasColumnName("idgroups");

                entity.Property(e => e.Description)
                    .HasMaxLength(145)
                    .HasColumnName("description");

                entity.Property(e => e.Groupname)
                    .HasMaxLength(45)
                    .HasColumnName("groupname");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Idcontacts)
                    .HasName("PRIMARY");

                entity.ToTable("contacts");

                entity.HasIndex(e => e.Nickname, "nickname_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "username_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idcontacts).HasColumnName("idcontacts");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email");

                entity.Property(e => e.Isadmin)
                    .HasColumnName("isadmin")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(45)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(45)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(45)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Groupcontact>(entity =>
            {
                entity.HasKey(e => e.Idgroupcontacts)
                    .HasName("PRIMARY");

                entity.ToTable("groupcontacts");

                entity.HasIndex(e => e.Idcontacts, "idcontacts_idx");

                entity.HasIndex(e => e.Idgroups, "idgroups_idx");

                entity.Property(e => e.Idgroupcontacts).HasColumnName("idgroupcontacts");

                entity.Property(e => e.Idcontacts).HasColumnName("idcontacts");

                entity.Property(e => e.Idgroups).HasColumnName("idgroups");

                entity.Property(e => e.Isadmin)
                    .HasColumnName("isadmin")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.IdcontactsNavigation)
                    .WithMany(p => p.Groupcontacts)
                    .HasForeignKey(d => d.Idcontacts)
                    .HasConstraintName("idcontacts");

                entity.HasOne(d => d.IdgroupsNavigation)
                    .WithMany(p => p.Groupcontacts)
                    .HasForeignKey(d => d.Idgroups)
                    .HasConstraintName("idgroups");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
