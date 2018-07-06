using System;
using System.Reactive.Linq;


namespace Xamarin.Essentials.Extensions
{
    public static class AccelerometerEx
    {
        static IObservable<AccelerometerChangedEventArgs> obs;
        public static IObservable<AccelerometerChangedEventArgs> WhenReadingChanged()
        {
            obs = obs ?? Observable.Create<AccelerometerChangedEventArgs>(ob =>
            {
                var handler = new AccelerometerChangedEventHandler(ob.OnNext);
                Accelerometer.ReadingChanged += handler;
                Accelerometer.Start(SensorSpeed.Fastest);
                return () =>
                {
                    Accelerometer.Stop();
                    Accelerometer.ReadingChanged -= handler;
                };
            })
            .Publish()
            .RefCount();

            return obs;
        }
    }
}
