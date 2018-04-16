//using System;
//using System.Collections.Concurrent;
//using System.Reactive.Linq;
//using System.Reactive.Threading.Tasks;
//using System.Threading.Tasks;


//namespace Acr.Reactive
//{
//    public static class ObservableQueue
//    {
//        public static ConcurrentQueue<Func<Task>> GlobalActionQueue { get; set; } = new ConcurrentQueue<Func<Task>>();


//        /// <summary>
//        /// This forces observables passed through to run in order without any sort of locking
//        /// The observable must be COMPLETABLE
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="observable"></param>
//        /// <returns></returns>
//        public static IObservable<T> Queue<T>(this IObservable<T> observable) => Observable.Create<T>(ob =>
//        {
//            var cancel = false;

//            GlobalActionQueue.Enqueue(async () =>
//            {
//                try
//                {
//                    if (!cancel)
//                    {
//                        var result = await observable.ToTask().ConfigureAwait(false);
//                        ob.Respond(result);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ob.OnError(ex);
//                }
//            });

//            ProcessQueue();
//            return () => cancel = true;
//        });


//        static bool running = false;
//        static async void ProcessQueue()
//        {
//            if (running)
//                return;

//            running = true;
//            while (GlobalActionQueue.TryDequeue(out Func<Task> task))
//                await task();

//            running = false;
//        }
//    }
//}
