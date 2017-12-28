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
    }
}
