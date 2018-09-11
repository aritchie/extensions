using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Acr.Reactive;


namespace Acr
{
    public static class PlatformExtensions
    {
        public static IObservable<T> ObserveOnMainThread<T>(this IPlatform platform, Func<T> func)
            => Observable.Create<T>(ob =>
            {
                platform.InvokeOnMainThread(() =>
                {
                    try
                    {
                        var result = func();
                        ob.Respond(result);
                    }
                    catch (Exception ex)
                    {
                        ob.OnError(ex);
                    }
                });

                return Disposable.Empty;
            });


        public static IObservable<Unit> ObserveOnMainThread(this IPlatform platform, Action action)
            => platform.ObserveOnMainThread(() =>
            {
                action();
                return Unit.Default;
            });
    }
}
