using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;


namespace Acr
{
    public class ObservableQueue
    {
        readonly ConcurrentQueue<Func<Task>> queue = new ConcurrentQueue<Func<Task>>();


        public bool IsProcessing { get; private set; }
        public int ItemsInQueue => this.queue.Count;


        /// <summary>
        /// This forces observables passed through to run in order without any sort of locking
        /// The observable must be COMPLETABLE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable"></param>
        /// <returns></returns>
        public IObservable<T> Queue<T>(IObservable<T> observable) => Observable.Create<T>(ob =>
        {
            var cancel = false;

            this.queue.Enqueue(async () =>
            {
                try
                {
                    if (!cancel)
                    {
                        var result = await observable
                            .Take(1)
                            .ToTask()
                            .ConfigureAwait(false);
                        ob.Respond(result);
                    }
                }
                catch (Exception ex)
                {
                    ob.OnError(ex);
                }
            });

            this.ProcessQueue();
            return () => cancel = true;
        });


        async void ProcessQueue()
        {
            if (this.IsProcessing)
                return;

            this.IsProcessing = true;
            while (this.queue.TryDequeue(out Func<Task> task))
                await task();

            this.IsProcessing = false;
        }
    }
}
