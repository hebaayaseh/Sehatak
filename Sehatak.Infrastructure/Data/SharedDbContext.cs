using Microsoft.EntityFrameworkCore;
using Sehatak.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Data
{
    public class SharedDbContext : DbContext 
    {
        public SharedDbContext(DbContextOptions<SharedDbContext> options)
        : base(options) { }
        public DbSet<SuperAdmin> SuperAdmins => Set<SuperAdmin>();
        public DbSet<MedicalCenter> MedicalCenters => Set<MedicalCenter>();
        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        public DbSet<CenterSubscription> CenterSubscriptions => Set<CenterSubscription>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  SuperAdmin 
            modelBuilder.Entity<SuperAdmin>(entity =>
            {
                entity.ToTable("super_admins");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.PasswordHash)
                      .IsRequired();
            });

            //  MedicalCenter 
            modelBuilder.Entity<MedicalCenter>(entity =>
            {
                entity.ToTable("medical_centers");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UniqueUrl).IsUnique();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.UniqueUrl)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.PrepaymentAmount)
                      .HasPrecision(10, 2);

                entity.Property(e => e.PartialRefundPercent)
                      .HasPrecision(5, 2);

                entity.HasOne(e => e.AddedBy)
                      .WithMany(s => s.AddedCenters)
                      .HasForeignKey(e => e.AddedBySuperAdminId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.Property(e => e.Status)
                      .HasConversion<string>();

                entity.Property(e => e.BookingType)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.ToTable("subscription_plans");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Price)
                      .HasPrecision(10, 2);
            });

            //  CenterSubscription 
            modelBuilder.Entity<CenterSubscription>(entity =>
            {
                entity.ToTable("center_subscriptions");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Center)
                      .WithMany(c => c.Subscriptions)
                      .HasForeignKey(e => e.CenterId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Plan)
                      .WithMany(p => p.CenterSubscriptions)
                      .HasForeignKey(e => e.PlanId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AmountPaid)
                      .HasPrecision(10, 2);

                entity.Property(e => e.Status)
                      .HasConversion<string>();
            });
        }
    }
}
