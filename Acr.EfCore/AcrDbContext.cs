using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Acr.EfCore
{
    public class AcrDbContext : DbContext
    {
        public bool IsTriggersEnabled { get; set; } = true;

        static readonly Subject<EntityEntry> beforeEachSubject = new Subject<EntityEntry>();
        public static IObservable<EntityEntry> BeforeEach => beforeEachSubject;

        static readonly Subject<IEnumerable<EntityEntry>> beforeAllSubject = new Subject<IEnumerable<EntityEntry>>();
        public static IObservable<IEnumerable<EntityEntry>> BeforeAll => beforeAllSubject;

        static readonly Subject<PreEntityEntry> afterEachSubject = new Subject<PreEntityEntry>();
        public static IObservable<PreEntityEntry> AfterEach => afterEachSubject;

        static readonly Subject<IEnumerable<PreEntityEntry>> afterAllSubject = new Subject<IEnumerable<PreEntityEntry>>();
        public static IObservable<IEnumerable<PreEntityEntry>> AfterAll => afterAllSubject;


        protected bool HasAfterTriggerEnabled => afterEachSubject.HasObservers || afterAllSubject.HasObservers;


        public override int SaveChanges() => this.SaveChanges(true);
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (!this.IsTriggersEnabled)
                return base.SaveChanges(acceptAllChangesOnSuccess);

            var entries = this.ChangeTracker.Entries();
            this.FireBeforeTriggers(entries);

            IEnumerable<PreEntityEntry> preActionState = null;
            if (this.HasAfterTriggerEnabled)
                preActionState = entries.Select(x => new PreEntityEntry(x)).ToList();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            this.FireAfterTriggers(preActionState);

            return result;
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => this.SaveChangesAsync(true, cancellationToken);
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = this.ChangeTracker.Entries();
            IEnumerable<PreEntityEntry> preActionState = null;
            if (this.HasAfterTriggerEnabled)
                preActionState = entries.Select(x => new PreEntityEntry(x)).ToList();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            this.FireAfterTriggers(preActionState);

            return result;
        }


        protected virtual void FireBeforeTriggers(IEnumerable<EntityEntry> entries)
        {
            if (!this.IsTriggersEnabled)
                return;

            if (beforeEachSubject.HasObservers)
                foreach (var entry in entries)
                    beforeEachSubject.OnNext(entry);

            if (beforeAllSubject.HasObservers)
                beforeAllSubject.OnNext(entries);
        }


        protected virtual void FireAfterTriggers(IEnumerable<PreEntityEntry> entries)
        {
            if (!this.IsTriggersEnabled)
                return;

            if (afterEachSubject.HasObservers)
                foreach (var entry in entries)
                    afterEachSubject.OnNext(entry);

            if (afterAllSubject.HasObservers)
                afterAllSubject.OnNext(entries);
        }
    }
}