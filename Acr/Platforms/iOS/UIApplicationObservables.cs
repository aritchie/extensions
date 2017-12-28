//using System;
//using System.Globalization;
//using System.Linq;
//using Foundation;
//using UIKit;

using System;
using System.Reactive;
using System.Reactive.Linq;
using UIKit;


namespace Acr
{
    public static class UIApplicationObservables
    {
        public static IObservable<Unit> WhenEnteringForeground() => Observable.Create<Unit>(ob =>
        {
            var token = UIApplication
                .Notifications
                .ObserveWillEnterForeground((sender, args) => ob.OnNext(Unit.Default));

            return () => token.Dispose();
        });


        public static IObservable<Unit> WhenEnteringBackground() => Observable.Create<Unit>(ob =>
        {
            var token = UIApplication
                .Notifications
                .ObserveDidEnterBackground((sender, args) => ob.OnNext(Unit.Default));

            return () => token.Dispose();
        });
    }
}