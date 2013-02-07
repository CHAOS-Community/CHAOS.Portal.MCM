namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection.Mapping;
    using Chaos.Mcm.Data.Dto;

    public static class ReaderExtensions
    {
        private static readonly IDictionary<Type, IReaderMapping> _mappings;
 
        //todo: add proper error handling when there is no mapping for the requested type
        static ReaderExtensions()
        {
            _mappings = new Dictionary<Type, IReaderMapping>();

            _mappings.Add(typeof(ObjectRelationInfo), new ObjectRelationInfoMapping());
            _mappings.Add(typeof(NewMetadata), new MetadataMapping());
        }

        public static TResultType Map<TResultType>(this IDataReader reader)
        {
            var mapping = _mappings[typeof(TResultType)];

            return (TResultType)mapping.Map(reader);
        }

        public static IList<TResultType> Map<TResultType>(this IList<KeyValuePair<string, object>[]> readerData) where TResultType : IKeyValueMapper, new()
        {
            return readerData.Select(Map<TResultType>).ToList();
        }

        private static TResultType Map<TResultType>(KeyValuePair<string, object>[] row) where TResultType : IKeyValueMapper, new()
        {
            var dto = new TResultType();

            dto.Map(row);

            return dto;
        }

        public static string GetString(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as string;
        }

        public static Guid GetGuid(this IDataReader reader, string name)
        {
            return ConvertToGuidNullable(reader, name).Value;
        }

        public static Guid? ConvertToGuidNullable(this IDataReader reader, string name)
        {
            var value = reader.GetValue(reader.GetOrdinal(name));

            return Convert.IsDBNull(value) ? (Guid?)null : new Guid(value as byte[]);
        }

        public static XDocument ConvertToXDocument(this IDataReader reader, string name)
        {
            var value = reader.GetValue(reader.GetOrdinal(name));

            if (value is DBNull) return null;

            return XDocument.Parse(value as string);
        }

        public static uint ConvertToUint32(this IDataReader reader, string name)
        {
            return ConvertToUint32Nullable(reader, name).Value;
        }

        public static uint? ConvertToUint32Nullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as uint?;
        }

        public static int? ConvertToInt32Nullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as int?;
        }

        public static DateTime ConvertToDateTime(this IDataReader reader, string name)
        {
            return ConvertToDateTimeNullable(reader, name).Value;
        }

        public static DateTime? ConvertToDateTimeNullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as DateTime?;
        }
    }
}