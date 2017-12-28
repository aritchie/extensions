//http://blogs.microsoft.co.il/blogs/shimmy/archive/2010/12/26/observabledictionary-lt-tkey-tvalue-gt-c.aspx
using System;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace Acr.Collections
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        const string CountString = "Count";
        const string IndexerName = "Item[]";
        const string KeysName = "Keys";
        const string ValuesName = "Values";


        IDictionary<TKey, TValue> innerDictionary;
        protected IDictionary<TKey, TValue> InnerDictionary => this.innerDictionary;


        public ObservableDictionary()
        {
            this.innerDictionary = new Dictionary<TKey, TValue>();
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.innerDictionary = new Dictionary<TKey, TValue>(dictionary);
        }
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            this.innerDictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public ObservableDictionary(int capacity)
        {
            this.innerDictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this.innerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.innerDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }


        public void Add(TKey key, TValue value) => this.Insert(key, value, true);
        public bool ContainsKey(TKey key) => this.InnerDictionary.ContainsKey(key);
        public ICollection<TKey> Keys => this.InnerDictionary.Keys;

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue value;
            this.InnerDictionary.TryGetValue(key, out value);
            var removed = this.InnerDictionary.Remove(key);
            if (removed)
                this.OnCollectionChanged();
            return removed;
        }

        public bool TryGetValue(TKey key, out TValue value) => this.InnerDictionary.TryGetValue(key, out value);
        public ICollection<TValue> Values => this.InnerDictionary.Values;
        public TValue this[TKey key]
        {
            get => this.InnerDictionary[key];
            set => this.Insert(key, value, false);
        }


        public void Add(KeyValuePair<TKey, TValue> item) => this.Insert(item.Key, item.Value, true);
        public void Clear()
        {
            if (this.InnerDictionary.Count > 0)
            {
                this.InnerDictionary.Clear();
                OnCollectionChanged();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => this.InnerDictionary.Contains(item);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => this.InnerDictionary.CopyTo(array, arrayIndex);
        public int Count => this.InnerDictionary.Count;
        public bool IsReadOnly => this.InnerDictionary.IsReadOnly;
        public bool Remove(KeyValuePair<TKey, TValue> item) => this.Remove(item.Key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.InnerDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.InnerDictionary).GetEnumerator();


        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (items.Count > 0)
            {
                if (this.InnerDictionary.Count > 0)
                {
                    if (items.Keys.Any((k) => this.InnerDictionary.ContainsKey(k)))
                        throw new ArgumentException("An item with the same key has already been added.");
                    else
                        foreach (var item in items) this.InnerDictionary.Add(item);
                }
                else
                    this.innerDictionary = new Dictionary<TKey, TValue>(items);

                this.OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
            }
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue item;
            if (this.InnerDictionary.TryGetValue(key, out item))
            {
                if (add) throw new ArgumentException("An item with the same key has already been added.");
                if (Equals(item, value)) return;
                this.InnerDictionary[key] = value;

                this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                this.InnerDictionary[key] = value;

                this.OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        void OnPropertyChanged()
        {
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
            this.OnPropertyChanged(KeysName);
            this.OnPropertyChanged(ValuesName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        void OnCollectionChanged()
        {
            this.OnPropertyChanged();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            this.OnPropertyChanged();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            this.OnPropertyChanged();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            this.OnPropertyChanged();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItems));
        }
    }
}