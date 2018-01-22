using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class ISoftDeleteExtensions
    {
        public static void SoftDeletes(this AcrDbContext data)
        {
            data.WhenModelBuilding.Subscribe(x => x.SoftDeletes());
            data.BeforeEach.Subscribe(x =>
            {
                if (x.Entity is ISoftDeleteEntity softDelete)
                {
                    switch (x.State)
                    {
                        case EntityState.Added:
                            softDelete.IsDeleted = false;
                            break;

                        case EntityState.Deleted:
                            softDelete.IsDeleted = true;
                            x.State = EntityState.Modified;
                            break;
                    }
                }
            });
        }


        public static void SoftDeletes(this ModelBuilder builder)
        {
            var entityTypes = builder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tenant = typeof(ISoftDeleteEntity).IsAssignableFrom(entityType.ClrType);
                if (tenant)
                {
                    var method = SetFilterMethod.MakeGenericMethod(entityType.ClrType);
                    method.Invoke(null, new object[] { builder });
                }
            }
        }


        static readonly MethodInfo SetFilterMethod = typeof(ISoftDeleteExtensions)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(x =>
                x.IsGenericMethod &&
                x.Name == nameof(SetFilter)
            );


        static void SetFilter<T>(ModelBuilder builder) where T : class, ISoftDeleteEntity
        {
            builder
                .Entity<T>()
                .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
