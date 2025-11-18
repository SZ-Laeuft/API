using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SZL_Backend.Entities;

namespace SZL_Backend.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Gift> Gifts { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Runner> Runners { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=100.102.196.30;Port=5432;Database=szl;Username=admin;Password=Szl-20010901;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.Donationid).HasName("donations_pkey");

            entity.ToTable("donations");

            entity.Property(e => e.Donationid).HasColumnName("donationid");
            entity.Property(e => e.Amount)
                .HasPrecision(7, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Runnerid).HasColumnName("runnerid");

            entity.HasOne(d => d.Runner).WithMany(p => p.Donations)
                .HasForeignKey(d => d.Runnerid)
                .HasConstraintName("fk_runnerid_don");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Endtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("endtime");
            entity.Property(e => e.Isactive)
                .HasMaxLength(50)
                .HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Place)
                .HasMaxLength(50)
                .HasColumnName("place");
            entity.Property(e => e.Starttime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");

            entity.HasMany(d => d.Runners).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "Participate",
                    r => r.HasOne<Runner>().WithMany()
                        .HasForeignKey("Runnerid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_runnerid"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("Eventid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_eventid"),
                    j =>
                    {
                        j.HasKey("Eventid", "Runnerid").HasName("takespart_pkey");
                        j.ToTable("participate");
                        j.IndexerProperty<int>("Eventid").HasColumnName("eventid");
                        j.IndexerProperty<int>("Runnerid").HasColumnName("runnerid");
                    });
        });

        modelBuilder.Entity<Gift>(entity =>
        {
            entity.HasKey(e => e.Giftid).HasName("gifts_pkey");

            entity.ToTable("gifts");

            entity.Property(e => e.Giftid).HasColumnName("giftid");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Roundsid).HasName("rounds_pkey");

            entity.ToTable("rounds");

            entity.Property(e => e.Roundsid).HasColumnName("roundsid");
            entity.Property(e => e.Runnerid).HasColumnName("runnerid");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Runner).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.Runnerid)
                .HasConstraintName("fk_runnerid");
        });

        modelBuilder.Entity<Runner>(entity =>
        {
            entity.HasKey(e => e.Runnerid).HasName("runner_pkey");

            entity.ToTable("runner");

            entity.Property(e => e.Runnerid).HasColumnName("runnerid");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Teamid).HasColumnName("teamid");
            entity.Property(e => e.Uid).HasColumnName("uid");

            entity.HasOne(d => d.Team).WithMany(p => p.Runners)
                .HasForeignKey(d => d.Teamid)
                .HasConstraintName("fk_teamid");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.Runners)
                .HasForeignKey(d => d.Uid)
                .HasConstraintName("fk_uid");

            entity.HasMany(d => d.Gifts).WithMany(p => p.Runners)
                .UsingEntity<Dictionary<string, object>>(
                    "Receife",
                    r => r.HasOne<Gift>().WithMany()
                        .HasForeignKey("Giftid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_giftid_receive"),
                    l => l.HasOne<Runner>().WithMany()
                        .HasForeignKey("Runnerid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_runnerid_receive"),
                    j =>
                    {
                        j.HasKey("Runnerid", "Giftid").HasName("receives_pkey");
                        j.ToTable("receives");
                        j.IndexerProperty<int>("Runnerid").HasColumnName("runnerid");
                        j.IndexerProperty<int>("Giftid").HasColumnName("giftid");
                    });
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.Property(e => e.Uid)
                .ValueGeneratedNever()
                .HasColumnName("uid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Teamid).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Teamid).HasColumnName("teamid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
