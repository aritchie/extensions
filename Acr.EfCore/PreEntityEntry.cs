using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Acr.EfCore
{
    public class PreEntityEntry
    {
        public PreEntityEntry(EntityEntry entry)
        {
            this.OriginalState = entry.State;
            this.Entry = entry;
        }


        public EntityState OriginalState { get; }
        public EntityEntry Entry { get; }
    }
}
