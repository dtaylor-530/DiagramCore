using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ConnectionCore.Common
{
    public static class ObservableCollectionHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        public static IObservable<NotifyCollectionChangedEventArgs> SelectChanges(this INotifyCollectionChanged oc)
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                h => oc.CollectionChanged += h,
                h => oc.CollectionChanged -= h)
                .Select(_ => _.EventArgs);
        }

        public static IObservable<T> MakeObservable<T>(this IEnumerable oc)
        {

            return oc is INotifyCollectionChanged notifyCollectionChanged ?
                    oc.Cast<T>().ToObservable()
                        .Concat(notifyCollectionChanged.SelectNewItems<T>()) :
                    oc.Cast<T>().ToObservable();
        }

        public static IObservable<T> MakeObservable<T>(this IEnumerable<T> oc)
        {
            return oc is INotifyCollectionChanged notifyCollectionChanged ?
                        oc.ToObservable()
                            .Concat(notifyCollectionChanged.SelectNewItems<T>()) :
                        oc.ToObservable();
        }


        public static IObservable<object> MakeObservable(this IEnumerable oc)
        {
            return MakeObservable<object>(oc);
        }




        public static IObservable<T> SelectNewItems<T>(this INotifyCollectionChanged notifyCollectionChanged)
        {
            return notifyCollectionChanged
              .SelectChanges()
              .SelectMany(x => x.NewItems?.Cast<T>() ?? new T[] { });

        }

        public static IObservable<T> SelectOldItems<T>(this INotifyCollectionChanged notifyCollectionChanged)
        {
            return notifyCollectionChanged
              .SelectChanges()
              .SelectMany(x => x.OldItems?.Cast<T>() ?? new T[] { });
        }

        public static IObservable<NotifyCollectionChangedAction> SelectActions(this INotifyCollectionChanged notifyCollectionChanged)
        {
            return notifyCollectionChanged
              .SelectChanges()
              .Select(x => x.Action);
        }



    }
}

