using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Maxfit_.Models;

public partial class MaxFitContext : IdentityDbContext
{
    public MaxFitContext() { }

    public MaxFitContext(DbContextOptions<MaxFitContext> options) : base(options) { }

    // DbSet Tanımlamaları
    public virtual DbSet<CheckIn> CheckIns { get; set; }
    public virtual DbSet<Class> Classes { get; set; }
    public virtual DbSet<CourseRegistration> CourseRegistrations { get; set; }
    public virtual DbSet<CourseSession> CourseSessions { get; set; }
    public virtual DbSet<CourseSessionPhoto> CourseSessionPhotos { get; set; }
    public virtual DbSet<Equipment> Equipment { get; set; }
    public virtual DbSet<EquipmentMaintenance> EquipmentMaintenances { get; set; }
    public virtual DbSet<EquipmentOrder> EquipmentOrders { get; set; }
    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<MemberNotification> MemberNotifications { get; set; }
    public virtual DbSet<Membership> Memberships { get; set; }
    public virtual DbSet<MembershipType> MembershipTypes { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<Staff> Staff { get; set; }
    public virtual DbSet<StaffWorkSchedule> StaffWorkSchedules { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=KEREM\\MSSQLSERVER2017;Database=MaxFit;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity için şart

        // 1. MemberNotification
        modelBuilder.Entity<MemberNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);
        });

        // 2. EquipmentMaintenance
        modelBuilder.Entity<EquipmentMaintenance>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId);
        });

        // 3. CourseSessionPhoto
        modelBuilder.Entity<CourseSessionPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId);
        });

        // 4. EquipmentOrder
        modelBuilder.Entity<EquipmentOrder>(entity =>
        {
            entity.HasKey(e => e.EquipmentOrderId);
        });

        // 5. CourseRegistration
        modelBuilder.Entity<CourseRegistration>(entity =>
        {
            entity.HasKey(e => e.CourseRegistrationId);
        });

        // 6. Temel Tablolar
        modelBuilder.Entity<CheckIn>(entity => entity.HasKey(e => e.CheckInId));
        modelBuilder.Entity<Class>(entity => entity.HasKey(e => e.ClassId));
        modelBuilder.Entity<CourseSession>(entity => entity.HasKey(e => e.CourseSessionId));
        modelBuilder.Entity<Equipment>(entity => entity.HasKey(e => e.EquipmentId));
        modelBuilder.Entity<Member>(entity => entity.HasKey(e => e.MemberId));
        modelBuilder.Entity<Membership>(entity => entity.HasKey(e => e.MembershipId));
        modelBuilder.Entity<MembershipType>(entity => entity.HasKey(e => e.MembershipTypeId));
        modelBuilder.Entity<Room>(entity => entity.HasKey(e => e.RoomId));
        modelBuilder.Entity<Staff>(entity => entity.HasKey(e => e.StaffId));
        modelBuilder.Entity<StaffWorkSchedule>(entity => entity.HasKey(e => e.StaffWorkScheduleId));
        modelBuilder.Entity<Payment>(entity => entity.HasKey(e => e.PaymentId));

        // Fix MembershipType decimal precision
        modelBuilder.Entity<MembershipType>()
            .Property(m => m.Price)
            .HasColumnType("decimal(10,2)");


        // İlişkiler
        modelBuilder.Entity<Member>()
            .HasOne(d => d.MembershipType)
            .WithMany(p => p.Members)
            .HasForeignKey(d => d.MembershipTypeId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}