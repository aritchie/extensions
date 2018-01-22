using System;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class IDateStampExtensions
    {
        public static void DateStamps(this AcrDbContext data) => data
            .BeforeEach
            .Where(x => x.Entity is IDateStampEntity)
            .Subscribe(x =>
            {
                if (x.Entity is IDateStampEntity ds)
                {
                    var now = DateTimeOffset.UtcNow;
                    switch (x.State)
                    {
                        case EntityState.Added:
                            ds.DateCreated = now;
                            ds.DateUpdated = now;
                            break;

                        case EntityState.Modified:
                            ds.DateUpdated = now;
                            break;
                    }
                }
            });
    }
}
