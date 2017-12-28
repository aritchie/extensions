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


        internal static bool IsDataReflectable(this Type propertyType) =>
            propertyType.IsPrimitive ||
            propertyType == typeof(string) ||
            propertyType == typeof(DateTime) ||
            propertyType == typeof(DateTimeOffset);


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
                            var value = reader.GetValue(ordinal.Value);
                            if (prop.PropertyType == typeof(DateTime) && value is string s1)
                            {
                                var dt = DateTime.Parse(s1);
                                prop.SetValue(obj, dt);
                            }
                            else if (prop.PropertyType == typeof(DateTimeOffset) && value is string s2)
                            {
                                var dt = DateTimeOffset.Parse(s2);
                                prop.SetValue(obj, dt);
                            }
                            else
                            {
                                prop.SetValue(obj, value);
                            }
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