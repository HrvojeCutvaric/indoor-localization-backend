using System;
using System.Collections.Generic;
using IndoorLocalization.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Data;

public partial class IndoorLocalizationContext : DbContext
{
    public IndoorLocalizationContext()
    {
    }

    public IndoorLocalizationContext(DbContextOptions<IndoorLocalizationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetPositionHistory> Assetpositionhistories { get; set; }

    public virtual DbSet<AssetZoneHistory> Assetzonehistories { get; set; }

    public virtual DbSet<FloorMap> Floormaps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assets_pkey");

            entity.ToTable("assets");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.FloorMapId).HasColumnName("floormapid");
            entity.Property(e => e.LastSync).HasColumnName("lastsync");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.FloorMap).WithMany(p => p.Assets)
                .HasForeignKey(d => d.FloorMapId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assets_floormapid_fkey");
        });

        modelBuilder.Entity<AssetPositionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assetpositionhistory_pkey");

            entity.ToTable("assetpositionhistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssetId).HasColumnName("assetid");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("datetime");
            entity.Property(e => e.FloorMapId).HasColumnName("floormapid");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetPositionHistories)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assetpositionhistory_assetid_fkey");

            entity.HasOne(d => d.FloorMap).WithMany(p => p.AssetPositionHistories)
                .HasForeignKey(d => d.FloorMapId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assetpositionhistory_floormapid_fkey");
        });

        modelBuilder.Entity<AssetZoneHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assetzonehistory_pkey");

            entity.ToTable("assetzonehistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssetId).HasColumnName("assetid");
            entity.Property(e => e.EnterDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("enterdatetime");
            entity.Property(e => e.ExitDateTime).HasColumnName("exitdatetime");
            entity.Property(e => e.RetentionTime).HasColumnName("retentiontime");
            entity.Property(e => e.ZoneId).HasColumnName("zoneid");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetZoneHistories)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assetzonehistory_assetid_fkey");

            entity.HasOne(d => d.Zone).WithMany(p => p.AssetZoneHistories)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("assetzonehistory_zoneid_fkey");
        });

        modelBuilder.Entity<FloorMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("floormaps_pkey");

            entity.ToTable("floormaps");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");

            entity.Property(e => e.ImageWidthPx)
                .HasColumnName("image_width_px");

            entity.Property(e => e.ImageHeightPx)
                .HasColumnName("image_height_px");

            entity.Property(e => e.WidthInMeters)
                .HasColumnName("width_m");

            entity.Property(e => e.HeightInMeters)
                .HasColumnName("height_m");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .HasColumnName("firstname");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .HasColumnName("lastname");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.RefreshToken).HasColumnName("refreshtoken");
            entity.Property(e => e.RefreshTokenExpiry).HasColumnName("refreshtokenexpiry");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("zones_pkey");

            entity.ToTable("zones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Points)
                .HasColumnType("jsonb")
                .HasColumnName("points");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Zones)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("zones_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
