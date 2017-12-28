using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;


namespace Acr
{
    public static class RxExtensions
    {
        public static IObservable<List<Timestamped<T>>> BufferWhile<T>(this IObservable<T> thisObs, Func<T, bool> predicate)
            => Observable.Create<List<Timestamped<T>>>(ob =>
            {
                List<Timestamped<T>> list = null;
                return thisObs
                    .Timestamp()
                    .Subscribe(x =>
                    {
                        if (predicate(x.Value))
                        {
                            if (list == null)
                            {
                                list = new List<Timestamped<T>>();
                            }
                            list.Add(x);
                        }
                        else if (list != null)
                        {
                            ob.OnNext(list);
                            list = null;
                        }
                    });
            });


        public static IObservable<TRet> WhenAnyValue<TSender, TRet>(this TSender This, Expression<Func<TSender, TRet>> expression) where TSender : INotifyPropertyChanged
        {
            var p = This.GetPropertyInfo(expression);
            return Observable
                .FromEventPattern<PropertyChangedEventArgs>(This, nameof(INotifyPropertyChanged.PropertyChanged))
                .StartWith(new EventPattern<PropertyChangedEventArgs>(This, new PropertyChangedEventArgs(p.Name)))
                .Where(x => x.EventArgs.PropertyName == p.Name)
                .Select(x =>
                {
                    var value = (TRet)p.GetValue(This);
                    return value;
                });
        }


        public static IObservable<TSender> WhenAnyPropertyChanged<TSender>(this TSender This) where TSender : INotifyPropertyChanged
            => Observable
                .FromEventPattern<PropertyChangedEventArgs>(This, nameof(INotifyPropertyChanged.PropertyChanged))
                .Select(x => This);


        public static void ApplyMaxLengthConstraint<T>(this T npc, Expression<Func<T, string>> expression, int maxLength) where T : INotifyPropertyChanged
        {
            var property = npc.GetPropertyInfo(expression);

            if (property.PropertyType != typeof(string))
                throw new ArgumentException($"You can only use maxlength constraints on string based properties - {npc.GetType()}.{property.Name}");

            if (!property.CanWrite)
                throw new ArgumentException($"You can only apply maxlength constraints to public setter properties - {npc.GetType()}.{property.Name}");

            npc
                .WhenAnyValue(expression)
                .Where(x => x != null && x.Length > maxLength)
                .Subscribe(x =>
                {
                    var value = x.Substring(0, maxLength);
                    property.SetValue(npc, value);
                });
        }
    }
}
