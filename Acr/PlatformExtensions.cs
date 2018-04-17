using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Acr.Reactive;


namespace Acr
{
    public static class PlatformExtensions
    {
        public static IObservable<T> InvokeOnMainThread<T>(this IPlatform platform, Func<T> func) => Observable.Create<T>(ob =>
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


        public static IObservable<Unit> InvokeOnMainThread<T>(this IPlatform platform, Action action) => Observable.Create<Unit>(ob =>
        {
            platform.InvokeOnMainThread(() =>
            {
                try
                {
                    action();
                    ob.Respond(Unit.Default);
                }
                catch (Exception ex)
                {
                    ob.OnError(ex);
                }
            });

            return Disposable.Empty;
        });
    }
}
