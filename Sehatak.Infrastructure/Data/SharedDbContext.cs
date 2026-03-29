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
    }
}
