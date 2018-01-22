using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public class AcrTenancyDbContext : AcrDbContext
    {
        static readonly MethodInfo SetTenancyFilterMethod = typeof(AcrTenancyDbContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(x =>
                x.IsGenericMethod &&
                x.Name == nameof(SetTenancyFilter)
            );


        public int? TenantId { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var entityTypes = builder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tenant = typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType);
                if (tenant)
                {
                    var method = SetTenancyFilterMethod.MakeGenericMethod(entityType.ClrType);
                    method.Invoke(this, new object[] { builder });
                }
            }
        }


        void SetTenancyFilter<T>(ModelBuilder builder) where T : class, ITenantEntity
        {
            builder
                .Entity<T>()
                .HasQueryFilter(x => x.TenantId == this.TenantId);
        }


        public override int SaveChanges()
        {
            if (this.TenantId != null)
            {
                var entities = this.ChangeTracker
                    .Entries()
                    .Select(x => x.Entity)
                    .OfType<ITenantEntity>();

                foreach (var e in entities)
                    e.TenantId = this.TenantId.Value;
            }
            return base.SaveChanges();
        }
    }
}
