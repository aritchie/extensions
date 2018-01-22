using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class ITenantExtensions
    {
        public static void Tenancy(this AcrDbContext data, int tenantId)
        {
            data.WhenModelBuilding.Subscribe(builder =>
            {
                var entityTypes = builder.Model.GetEntityTypes();
                foreach (var entityType in entityTypes)
                {
                    var tenant = typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType);
                    if (tenant)
                    {
                        var method = SetTenancyFilterMethod.MakeGenericMethod(entityType.ClrType);
                        //method.Invoke(this, new[] { builder });
                    }
                }
            });

            data.BeforeEach.Subscribe(entry =>
            {
                if (entry.Entity is ITenantEntity tenant)
                    tenant.TenantId = tenantId;
            });
        }


        static MethodInfo instance;
        static MethodInfo SetTenancyFilterMethod
        {
            get
            {
                if (instance == null)
                {
                    instance = typeof(ITenantEntity)
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .FirstOrDefault(x =>
                            x.IsGenericMethod &&
                            x.Name == nameof(SetTenancyFilter)
                        );
                }

                return instance;
            }
        }


        public static void SetTenancyFilter<T>(ModelBuilder builder, int tenantId) where T : class, ITenantEntity
        {
            builder
                .Entity<T>()
                .HasQueryFilter(x => x.TenantId == tenantId);
        }
    }
}
