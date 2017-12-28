using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Acr.EfCore
{
    public static class MappingConventions
    {
        public static void GetProperties<T>(this EntityTypeBuilder<T> builder)
        {
            var allProperties = builder
                .Metadata
                .ClrType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanWrite && x.CanRead)
                .ToList();

            var navigations = builder
                .Metadata
                .GetNavigations()
                .Select(x => x.Name)
                .ToList();

            var list = allProperties
                .Where(x => !navigations.Contains(x.Name))
                .ToList();

            //return list;
        }


        public static void Defaults<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            builder.DefaultTableName();
            builder.DefaultId();

            //var properties = this.GetAllProperties();
            //foreach (var property in properties)
            //{
            //    builder
            //        .Property(property.Name)
            //        .HasMaxLength(50)
            //        .IsRequired();
            //}
        }


        public static void DefaultTableName<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            var tableName = builder.Metadata.ClrType.Name;
            if (tableName.EndsWith("y"))
                tableName = tableName.TrimEnd('y') + "ie";

            tableName += "s";
            builder.ToTable(tableName);
        }


        public static void DefaultId<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            var idProperty = typeof(T)
                .GetRuntimeProperties()
                .FirstOrDefault(x =>
                    x.Name == "Id" &&
                    x.CanWrite &&
                    x.CanRead &&
                    (
                        x.PropertyType == typeof(int) ||
                        x.PropertyType == typeof(long)
                    )
                );

            if (idProperty != null)
            {
                var columnName = builder.Metadata.ClrType.Name + "Id";
                builder.HasKey("Id");
                builder
                    .Property(idProperty.Name)
                    .HasColumnName(columnName);
            }
        }


        //static bool IsSimple(this Type type)
        //{
        //    if (type.IsPrimitive)
        //        return true;

        //    if (type == typeof(string))
        //        return true;

        //    if (type == typeof(DateTime))
        //        return true;

        //    if (type == typeof(DateTimeOffset))
        //        return true;

        //    return false;
        //}
    }
}