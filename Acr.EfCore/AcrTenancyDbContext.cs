using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public class AcrTenancyDbContext : AcrDbContext
    {
        protected AcrTenancyDbContext() : base() { }
        protected AcrTenancyDbContext(DbContextOptions options) : base(options) { }


        public override int SaveChanges()
        {
            this.SetTenantId();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.SetTenantId();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.SetTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.SetTenantId();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


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


        protected virtual void SetTenantId()
        {
            if (this.TenantId == null)
                return;

            var entities = this.ChangeTracker
                .Entries()
                .Select(x => x.Entity)
                .OfType<ITenantEntity>();

            foreach (var e in entities)
                e.TenantId = this.TenantId.Value;
        }


        void SetTenancyFilter<T>(ModelBuilder builder) where T : class, ITenantEntity
        {

            var ecfg = builder.Entity<T>();
            //if (ecfg.Metadata.QueryFilter != null)
            //{
            //    Expression.And(ecfg.Metadata.QueryFilter, Expressio<Func<T, bool>>.Create);
            //}

            ecfg.HasQueryFilter(x => x.TenantId == this.TenantId);
        }





        //public override Int32 SaveChanges()
        //{
        //    return TriggersEnabled ? this.SaveChangesWithTriggers(base.SaveChanges) : base.SaveChanges();
        //}


    }
}
