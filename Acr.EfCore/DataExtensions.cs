using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Acr.EfCore
{
    public static class DataExtensions
    {
        public static Dictionary<Type, Func<object, object>> Converters { get; } = new Dictionary<Type, Func<object, object>>
        {
            { typeof(short), x => Convert.ToInt16(x) },
            { typeof(ushort), x => Convert.ToUInt16(x) },
            { typeof(int), x => Convert.ToInt32(x) },
            { typeof(uint), x => Convert.ToUInt32(x) },
            { typeof(long), x => Convert.ToInt64(x) },
            { typeof(ulong), x => Convert.ToUInt64(x) },
            { typeof(double), x => Convert.ToDouble(x) },
            { typeof(decimal), x => Convert.ToDecimal(x) },
            { typeof(DateTime), x => DateTime.Parse(x as string) },
            { typeof(DateTimeOffset), x => DateTimeOffset.Parse(x as string) }
        };

        public static Task<DbDataReader> ExecuteReader(this DbContext data, string sql, params ValueTuple<string, object>[] parameters) => data.ExecuteReader(sql, CancellationToken.None, parameters);
        public static async Task<DbDataReader> ExecuteReader(this DbContext data, string sql, CancellationToken ct, params ValueTuple<string, object>[] parameters)
        {
            var conn = data.Database.GetDbConnection();
            await conn.OpenAsync(ct);
            var command = conn.CreateCommand();
            command.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var parm = command.CreateParameter();
                    parm.ParameterName = parameter.Item1;
                    parm.Value = parameter.Item2 ?? DBNull.Value;
                    command.Parameters.Add(parm);
                }
            }
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);
        }


        public static Task<List<T>> ExecuteToList<T>(this DbContext data, Func<DbDataReader, T> cast, string sql, params ValueTuple<string, object>[] parameters) => data.ExecuteToList(cast, sql, CancellationToken.None, parameters);
        public static async Task<List<T>> ExecuteToList<T>(this DbContext data, Func<DbDataReader, T> cast, string sql, CancellationToken ct, params ValueTuple<string, object>[] parameters)
        {
            var list = new List<T>();
            using (var reader = await data.ExecuteReader(sql, ct, parameters).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    var item = cast(reader);
                    list.Add(item);
                }
            }
            return list;
        }


        public static Type UnwrapType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return type;
        }


        internal static bool IsDataReflectable(this Type propertyType)
        {
            var t = UnwrapType(propertyType);
            return t.IsPrimitive ||
                   t == typeof(string) ||
                   t == typeof(DateTime) ||
                   t == typeof(DateTimeOffset);
        }


        public static Task<List<T>> ReflectionExecuteToList<T>(this DbContext data, string sql, params ValueTuple<string, object>[] parameters) where T : new() => data.ReflectionExecuteToList<T>(sql, CancellationToken.None, parameters);
        public static async Task<List<T>> ReflectionExecuteToList<T>(this DbContext data, string sql, CancellationToken ct, params ValueTuple<string, object>[] parameters) where T : new()
        {
            var list = new List<T>();
            using (var reader = await data.ExecuteReader(sql, ct, parameters).ConfigureAwait(false))
            {
                var props = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x =>
                        x.CanRead &&
                        x.CanWrite &&
                        x.PropertyType.IsDataReflectable()
                    )
                    .ToList();

                while (reader.Read())
                {
                    var obj = new T();

                    for (var i = 0; i < props.Count; i++)
                    {
                        var prop = props[i];

                        //var ordinal = reader.GetOrdinal(props[i].Name);
                        var ordinal = reader.GetSafeOrdinal(prop.Name);

                        if (ordinal != null && !reader.IsDBNull(ordinal.Value))
                        {
                            var type = UnwrapType(prop.PropertyType);
                            var fieldType = reader.GetFieldType(ordinal.Value);

                            var value = reader.GetValue(ordinal.Value);
                            if (fieldType != type && Converters.ContainsKey(type))
                                value = Converters[type].Invoke(value);

                            prop.SetValue(obj, value);
                        }
                    }
                    list.Add(obj);
                }
            }
            return list;
        }


        public static int? GetSafeOrdinal(this DbDataReader reader, string propertyName)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return null;
        }


        public static T GetOrDefault<T>(this DbDataReader reader, int ordinal) => reader.IsDBNull(ordinal)
            ? default(T)
            : reader.GetFieldValue<T>(ordinal);


        public static T GetOrDefault<T>(this DbDataReader reader, string columnName)
        {
            //var oridinal = reader.GetOrdinal(columnName);
            var oridinal = reader.GetSafeOrdinal(columnName);
            if (oridinal == null)
                return default(T);

            return reader.GetFieldValue<T>(oridinal.Value);
        }
    }
}