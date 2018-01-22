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

        static readonly Subject<AcrDbContext> contextCreatedSubject = new Subject<AcrDbContext>();
        public static IObservable<AcrDbContext> ContextCreated => contextCreatedSubject;

        readonly Subject<DbContextOptionsBuilder> configSubject = new Subject<DbContextOptionsBuilder>();
        public IObservable<DbContextOptionsBuilder> WhenConfiguring => this.configSubject;

        readonly Subject<ModelBuilder> modelBuilderSubject = new Subject<ModelBuilder>();
        public IObservable<ModelBuilder> WhenModelBuilding => this.modelBuilderSubject;

        readonly Subject<EntityEntry> beforeEachSubject = new Subject<EntityEntry>();
        public IObservable<EntityEntry> BeforeEach => this.beforeEachSubject;

        readonly Subject<IEnumerable<EntityEntry>> beforeAllSubject = new Subject<IEnumerable<EntityEntry>>();
        public IObservable<IEnumerable<EntityEntry>> BeforeAll => this.beforeAllSubject;

        readonly Subject<PreEntityEntry> afterEachSubject = new Subject<PreEntityEntry>();
        public IObservable<PreEntityEntry> AfterEach => this.afterEachSubject;

        readonly Subject<IEnumerable<PreEntityEntry>> afterAllSubject = new Subject<IEnumerable<PreEntityEntry>>();
        public IObservable<IEnumerable<PreEntityEntry>> AfterAll => this.afterAllSubject;


        protected bool HasAfterTriggerEnabled => this.afterEachSubject.HasObservers || this.afterAllSubject.HasObservers;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            contextCreatedSubject.OnNext(this);
            this.configSubject.OnNext(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.modelBuilderSubject.OnNext(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }


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

            if (this.beforeEachSubject.HasObservers)
                foreach (var entry in entries)
                    this.beforeEachSubject.OnNext(entry);

            if (this.beforeAllSubject.HasObservers)
                this.beforeAllSubject.OnNext(entries);
        }


        protected virtual void FireAfterTriggers(IEnumerable<PreEntityEntry> entries)
        {
            if (!this.IsTriggersEnabled)
                return;

            if (this.afterEachSubject.HasObservers)
                foreach (var entry in entries)
                    this.afterEachSubject.OnNext(entry);

            if (this.afterAllSubject.HasObservers)
                this.afterAllSubject.OnNext(entries);
        }
    }
}