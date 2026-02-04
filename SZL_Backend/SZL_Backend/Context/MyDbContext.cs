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

    public virtual DbSet<BestTimeView> BestTimeViews { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Gift> Gifts { get; set; }

    public virtual DbSet<Participate> Participates { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Runner> Runners { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=100.94.128.1;Port=5432;Database=szl;Username=admin;Password=Szl-20010901");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BestTimeView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BestTimeView");

            entity.Property(e => e.ParticipateId).HasColumnName("ParticipateID");
            entity.Property(e => e.RoundId).HasColumnName("RoundID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.Donationid).HasName("donations_pkey");

            entity.ToTable("donations");

            entity.Property(e => e.Donationid).HasColumnName("donationid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Participateid).HasColumnName("participateid");

            entity.HasOne(d => d.Participate).WithMany(p => p.Donations)
                .HasForeignKey(d => d.Participateid)
                .HasConstraintName("fk_participateid_donations");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Isactive)
                .HasMaxLength(30)
                .HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Place)
                .HasMaxLength(30)
                .HasColumnName("place");
            entity.Property(e => e.Starttime).HasColumnName("starttime");
        });

        modelBuilder.Entity<Gift>(entity =>
        {
            entity.HasKey(e => e.Giftid).HasName("gifts_pkey");

            entity.ToTable("gifts");

            entity.Property(e => e.Giftid).HasColumnName("giftid");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Requirement).HasColumnName("requirement");

            entity.HasMany(d => d.Participates).WithMany(p => p.Gifts)
                .UsingEntity<Dictionary<string, object>>(
                    "Receife",
                    r => r.HasOne<Participate>().WithMany()
                        .HasForeignKey("ParticipateId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_participateid_receives"),
                    l => l.HasOne<Gift>().WithMany()
                        .HasForeignKey("Giftid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_giftid_receives"),
                    j =>
                    {
                        j.HasKey("Giftid", "ParticipateId").HasName("receives_pkey");
                        j.ToTable("receives");
                        j.IndexerProperty<int>("Giftid").HasColumnName("giftid");
                        j.IndexerProperty<int>("ParticipateId").HasColumnName("participateid");
                    });
        });

        modelBuilder.Entity<Participate>(entity =>
        {
            entity.HasKey(e => e.Participateid).HasName("participate_pkey");

            entity.ToTable("participate");

            entity.Property(e => e.Participateid).HasColumnName("participateid");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Runnerid).HasColumnName("runnerid");
            entity.Property(e => e.Tagid).HasColumnName("tagid");
            entity.Property(e => e.Teamid).HasColumnName("teamid");

            entity.HasOne(d => d.Category).WithMany(p => p.Participates)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_categoryid_participate");

            entity.HasOne(d => d.Event).WithMany(p => p.Participates)
                .HasForeignKey(d => d.Eventid)
                .HasConstraintName("fk_eventid_participate");

            entity.HasOne(d => d.Runner).WithMany(p => p.Participates)
                .HasForeignKey(d => d.Runnerid)
                .HasConstraintName("fk_runnerid_participate");

            entity.HasOne(d => d.Tag).WithMany(p => p.Participates)
                .HasForeignKey(d => d.Tagid)
                .HasConstraintName("fk_tagid_participate");

            entity.HasOne(d => d.Team).WithMany(p => p.Participates)
                .HasForeignKey(d => d.Teamid)
                .HasConstraintName("fk_teamid_participate");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Roundid).HasName("rounds_pkey");

            entity.ToTable("rounds");

            entity.Property(e => e.Roundid).HasColumnName("roundid");
            entity.Property(e => e.Participateid).HasColumnName("participateid");
            entity.Property(e => e.Roundtime).HasColumnName("roundtime");
            entity.Property(e => e.Roundtimestamp).HasColumnName("roundtimestamp");

            entity.HasOne(d => d.Participate).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.Participateid)
                .HasConstraintName("fk_participateid_rounds");
        });

        modelBuilder.Entity<Runner>(entity =>
        {
            entity.HasKey(e => e.Runnerid).HasName("runner_pkey");

            entity.ToTable("runner");

            entity.Property(e => e.Runnerid).HasColumnName("runnerid");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(30)
                .HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(30)
                .HasColumnName("lastname");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Tagid).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.Property(e => e.Tagid)
                .ValueGeneratedNever()
                .HasColumnName("tagid");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Teamid).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Teamid).HasColumnName("teamid");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
