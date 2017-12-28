using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class Mixins
    {
        public static IDisposable ApplyDateStamping(this AcrDbContext data) => AcrDbContext
            .BeforeEach
            .Where(x => x.Entity is IDateStampEntity)
            .Subscribe(x =>
            {
                var ds = x.Entity as IDateStampEntity;

                if (x.State == EntityState.Added && ds.DateCreated == DateTimeOffset.MinValue)
                    ds.DateCreated = DateTimeOffset.UtcNow;

                if (x.State == EntityState.Modified)
                    ds.DateUpdated = DateTimeOffset.UtcNow;
            });


        public static IDisposable ApplyAuditing() => Observable.Create<Unit>(ob =>
        {
            var beforeSub = AcrDbContext
                .BeforeEach
                .Where(x => x.Entity is IAudit)
                .Cast<IAudit>()
                .Subscribe(audit => audit.Version++);

            var afterSub = AcrDbContext
                .AfterAll
                .Subscribe(entities =>
                {
                    var auditEntries = entities
                        .Where(x => x.Entry.Entity is IAudit)
                        .ToList();

                    foreach (var entry in auditEntries)
                    {

                    }
                });

            return () =>
            {
                beforeSub.Dispose();
                afterSub.Dispose();
            };
        })
        .Subscribe();
    }
}
