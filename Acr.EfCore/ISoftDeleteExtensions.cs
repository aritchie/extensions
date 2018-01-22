using System;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class ISoftDeleteExtensions
    {
        public static void SoftDeletes(this AcrDbContext data) => data
            .BeforeEach
            .Subscribe(x =>
            {
                if (x.State == EntityState.Deleted && x.Entity is ISoftDeleteEntity softDelete)
                {
                    softDelete.IsDeleted = true;
                    x.State = EntityState.Modified;
                }
            });
    }
}
