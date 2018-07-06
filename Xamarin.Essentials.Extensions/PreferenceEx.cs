using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Xamarin.Essentials.Extensions
{
    public static class PreferencesEx
    {
        public static T Get<T>(string key, T defaultT = default(T))
        {
            var json = Preferences.Get(key, (string) null);
            if (json == null)
                return defaultT;

            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }


        public static void Set<T>(string key, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            Preferences.Set(key, json);
        }


        public static T Bind<T>() where T : INotifyPropertyChanged, new()
        {
            var obj = new T();
            Bind(obj);
            return obj;
        }


        public static void Bind(INotifyPropertyChanged obj)
        {
            var type = obj.GetType();
            var prefix = type.Name + ".";
            var props = GetTypeProperties(type);

            foreach (var prop in props)
            {
                var key = prefix + prop.Name;

                if (Preferences.ContainsKey(key))
                {
                    //var value = GetValue(prop.PropertyType, key);
                    prop.SetValue(obj, value);
                }
            }
            obj.PropertyChanged += OnPropertyChanged;
        }


        public static void UnBind(INotifyPropertyChanged obj)
            => obj.PropertyChanged -= OnPropertyChanged;


        static IEnumerable<PropertyInfo> GetTypeProperties(Type type) => type
            .GetTypeInfo()
            .DeclaredProperties
            .Where(x => x.CanRead && x.CanWrite);


        static void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var prop = GetTypeProperties(sender.GetType()).FirstOrDefault(x => x.Name.Equals(args.PropertyName));

            if (prop != null)
            {
                var key = $"{sender.GetType().Name}.{prop.Name}";
                var value = prop.GetValue(sender);
                //Preferences.Set(key, value);
            }
        }
    }
}
