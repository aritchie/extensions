using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acr.Reflection;


namespace Acr.Reactive
{
    public class ItemChanged<T, TRet>
    {
        public ItemChanged(T obj, TRet value)
        {
            this.Object = obj;
            this.Value = value;
        }

        public T Object { get; }
        public TRet Value { get; }
    }


    public static class RxExtensions
    {
        public static IConnectableObservable<TItem> ReplayWithReset<TItem, TReset>(this IObservable<TItem> src, IObservable<TReset> resetTrigger)
            => new ClearableReplaySubject<TItem, TReset>(src, resetTrigger);


        public static void Respond<T>(this IObserver<T> ob, T value)
        {
            ob.OnNext(value);
            ob.OnCompleted();
        }


        public static IObservable<List<TOut>> SelectEach<TIn, TOut>(this IObservable<List<TIn>> observable, Func<TIn, TOut> transform) =>
            observable.Select(data =>
            {
                var list = new List<TOut>();
                foreach (var item in data)
                {
                    var t = transform(item);
                    list.Add(t);
                }
                return list;
            });


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


        public static IObservable<ItemChanged<T, string>> WhenItemChanged<T>(this ObservableCollection<T> collection)
            where T : INotifyPropertyChanged
            => Observable.Create<ItemChanged<T, string>>(ob =>
            {
                var disp = new CompositeDisposable();
                foreach (var item in collection)
                    disp.Add(item.RxWhenAnyPropertyChanged().Subscribe(ob.OnNext));

                return disp;
            });


        public static IObservable<ItemChanged<T, TRet>> WhenItemValueChanged<T, TRet>(
            this ObservableCollection<T> collection,
            Expression<Func<T, TRet>> expression) where T : INotifyPropertyChanged =>
            Observable.Create<ItemChanged<T, TRet>>(ob =>
            {
                var disp = new CompositeDisposable();
                foreach (var item in collection)
                {
                    disp.Add(item
                        .RxWhenAnyValue(expression)
                        .Subscribe(x => ob.OnNext(new ItemChanged<T, TRet>(item, x)))
                    );
                }

                return disp;
            });


        public static IObservable<TRet> RxWhenAnyValue<TSender, TRet>(this TSender This, Expression<Func<TSender, TRet>> expression) where TSender : INotifyPropertyChanged
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


        public static IObservable<ItemChanged<TSender, string>> RxWhenAnyPropertyChanged<TSender>(this TSender This) where TSender : INotifyPropertyChanged
            => Observable
                .FromEventPattern<PropertyChangedEventArgs>(This, nameof(INotifyPropertyChanged.PropertyChanged))
                .Select(x => new ItemChanged<TSender, string>(This, x.EventArgs.PropertyName));


        public static IDisposable ApplyMaxLengthConstraint<T>(this T npc, Expression<Func<T, string>> expression, int maxLength) where T : INotifyPropertyChanged
        {
            var property = npc.GetPropertyInfo(expression);

            if (property.PropertyType != typeof(string))
                throw new ArgumentException($"You can only use maxlength constraints on string based properties - {npc.GetType()}.{property.Name}");

            if (!property.CanWrite)
                throw new ArgumentException($"You can only apply maxlength constraints to public setter properties - {npc.GetType()}.{property.Name}");

            return npc
                .RxWhenAnyValue(expression)
                .Where(x => x != null && x.Length > maxLength)
                .Subscribe(x =>
                {
                    var value = x.Substring(0, maxLength);
                    property.SetValue(npc, value);
                });
        }
    }
}
