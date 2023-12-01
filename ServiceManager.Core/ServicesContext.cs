using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Entities.Requests;
using ServiceManager.Core.Entities.Services;

namespace ServiceManager.Core
{
    public class ServicesContext : IdentityDbContext<User, Role, Guid>
    {
        public ServicesContext(DbContextOptions options) : base(options) { }

        public DbSet<Client> Client { get; set; } = null!;

        public DbSet<Material> Material { get; set; } = null!;
        public DbSet<Service> Service { get; set; } = null!;
        public DbSet<ServiceType> ServiceType { get; set; } = null!;

        public DbSet<Request> Request { get; set; } = null!;
        public DbSet<RequestHistory> RequestHistory { get; set; } = null!;
        public DbSet<RequestNotify> RequestNotify { get; set; } = null!;
        public DbSet<RequestMaterial> RequestMaterial { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>()
                .HasOne(e => e.ServiceType)
                .WithMany(e => e.Services)
                .HasForeignKey(e => e.ServiceTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Requests)
                .WithOne(e => e.Service)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasMany(e => e.Histories)
                .WithOne(e => e.Request)
                .HasForeignKey(e => e.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClientRequests)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExecutedRequests)
                .WithOne(e => e.Executor)
                .HasForeignKey(e => e.ExecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RequestNotify)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasMany(e => e.Materials)
                .WithMany(e => e.Requests)
                .UsingEntity<RequestMaterial>(
                    j => j
                        .HasOne(pt => pt.Material)
                        .WithMany(p => p.RequestMaterials)
                        .HasForeignKey(pt => pt.MaterialId),
                    j => j
                        .HasOne(pt => pt.Request)
                        .WithMany(t => t.RequestMaterials)
                        .HasForeignKey(pt => pt.RequestId),
                    j =>j.HasKey(t => new { t.RequestId, t.MaterialId }));
        }
    }
}