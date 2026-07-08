using EFCore.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EFCore
{
    public abstract class EFCoreDbContext : DbContext, IEFCoreDbContext
    {
        protected EFCoreDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSoftDeleteQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureSoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!IsSoftDeleteEntity(entityType.ClrType))
                {
                    continue;
                }

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeleted = Expression.Property(parameter, nameof(IDeletionAuditedEntity<object>.IsDeleted));
                var filter = Expression.Lambda(Expression.Not(isDeleted), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            HandleAuditedProperty();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            HandleAuditedProperty();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void HandleAuditedProperty()
        {
            ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is Entity<Guid> keyEntity && keyEntity.Id == Guid.Empty)
                    {
                        entry.Property("Id").CurrentValue = Guid.NewGuid();
                    }

                    if (entry.Entity is ICreationAuditedEntity guidCreationEntity)
                    {
                        guidCreationEntity.CreationTime = DateTime.Now;
                        guidCreationEntity.CreatorId = CurrentUserAmbient.UserId;
                    }
                    else if (entry.Entity is ICreationAuditedEntity<long?> longCreationEntity)
                    {
                        longCreationEntity.CreationTime = DateTime.Now;
                        longCreationEntity.CreatorId = CurrentUserAmbient<long?>.UserId;
                    }
                    else if (entry.Entity is ICreationAuditedEntity<string?> stringCreationEntity)
                    {
                        stringCreationEntity.CreationTime = DateTime.Now;
                        stringCreationEntity.CreatorId = CurrentUserAmbient<string?>.UserId;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IUpdationAuditedEntity guidUpdationEntity)
                    {
                        guidUpdationEntity.LastModificationTime = DateTime.Now;
                        guidUpdationEntity.LastModifierId = CurrentUserAmbient.UserId;
                    }
                    else if (entry.Entity is IUpdationAuditedEntity<long?> longUpdationEntity)
                    {
                        longUpdationEntity.LastModificationTime = DateTime.Now;
                        longUpdationEntity.LastModifierId = CurrentUserAmbient<long?>.UserId;
                    }
                    else if (entry.Entity is IUpdationAuditedEntity<string?> stringUpdationEntity)
                    {
                        stringUpdationEntity.LastModificationTime = DateTime.Now;
                        stringUpdationEntity.LastModifierId = CurrentUserAmbient<string?>.UserId;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if (entry.Entity is IDeletionAuditedEntity guidDeletionEntity)
                    {
                        entry.State = EntityState.Modified;
                        guidDeletionEntity.IsDeleted = true;
                        guidDeletionEntity.DeletionTime = DateTime.Now;
                        guidDeletionEntity.DeleterId = CurrentUserAmbient.UserId;
                    }
                    else if (entry.Entity is IDeletionAuditedEntity<long?> longDeletionEntity)
                    {
                        entry.State = EntityState.Modified;
                        longDeletionEntity.IsDeleted = true;
                        longDeletionEntity.DeletionTime = DateTime.Now;
                        longDeletionEntity.DeleterId = CurrentUserAmbient<long?>.UserId;
                    }
                    else if (entry.Entity is IDeletionAuditedEntity<string?> stringDeletionEntity)
                    {
                        entry.State = EntityState.Modified;
                        stringDeletionEntity.IsDeleted = true;
                        stringDeletionEntity.DeletionTime = DateTime.Now;
                        stringDeletionEntity.DeleterId = CurrentUserAmbient<string?>.UserId;
                    }
                }
            });
        }

        private static bool IsSoftDeleteEntity(Type clrType)
        {
            return clrType.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDeletionAuditedEntity<>));
        }
    }
}
