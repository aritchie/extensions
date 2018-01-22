using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Acr.EfCore
{
    public static class Extensions
    {
        //public static IServiceCollection AddEFSecondLevelCache(this IServiceCollection services)
        //{
        //    services.AddSingleton<IEFCacheKeyHashProvider, EFCacheKeyHashProvider>();
        //    services.AddSingleton<IEFCacheKeyProvider, EFCacheKeyProvider>();
        //    services.AddSingleton<IEFCacheServiceProvider, EFCacheServiceProvider>();

        //    return services;
        //}


        //public static IApplicationBuilder UseEFSecondLevelCache(this IApplicationBuilder app)
        //{
        //    EFServiceProvider.ApplicationServices = app.ApplicationServices;
        //    return app;
        //}


        public static void Cancel(this EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    break;

                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }
}
