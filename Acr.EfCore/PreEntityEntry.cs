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
            this.OriginalValues = this.Entry.OriginalValues.Clone();
        }


        public EntityState OriginalState { get; }
        public PropertyValues OriginalValues { get; }
        public EntityEntry Entry { get; }
    }
}
