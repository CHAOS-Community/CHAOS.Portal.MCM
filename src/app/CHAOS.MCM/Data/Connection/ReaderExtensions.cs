namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection.Mapping;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;

    public static class ReaderExtensions
    {
        private static readonly IDictionary<Type, IReaderMapping<object>> _mappings;
 
        //todo: add proper error handling when there is no mapping for the requested type
        static ReaderExtensions()
        {
            _mappings = new Dictionary<Type, IReaderMapping<object>>();

            _mappings.Add(typeof(ObjectRelationInfo), new ObjectRelationInfoMapping());
            _mappings.Add(typeof(NewMetadata), new MetadataMapping());
            _mappings.Add(typeof(ObjectMetadata), new ObjectMetadataMapping());
            _mappings.Add(typeof(NewObject), new ObjectMapping());
            _mappings.Add(typeof(FileInfo), new FileInfoMapping());
            _mappings.Add(typeof(FolderInfo), new FolderInfoMapping());
            _mappings.Add(typeof(ObjectFolder), new ObjectFolderMapping());
            _mappings.Add(typeof(AccessPoint_Object_Join), new AccesspointObjectJoinMapping());
        }

        public static IEnumerable<TResultType> Map<TResultType>(this IDataReader reader)
        {
            var mapping = _mappings[typeof(TResultType)];

            return (IEnumerable<TResultType>)mapping.Map(reader);
        }

        public static string GetString(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as string;
        }

        public static Guid GetGuid(this IDataReader reader, string name)
        {
            return GetGuidNullable(reader, name).Value;
        }

        public static Guid? GetGuidNullable(this IDataReader reader, string name)
        {
            var value = reader.GetValue(reader.GetOrdinal(name));

            return Convert.IsDBNull(value) ? (Guid?)null : new Guid(value as byte[]);
        }

        public static XDocument GetXDocument(this IDataReader reader, string name)
        {
            var value = reader.GetValue(reader.GetOrdinal(name));

            if (value is DBNull) return null;

            return XDocument.Parse(value as string);
        }

        public static uint GetUint32(this IDataReader reader, string name)
        {
            return GetUint32Nullable(reader, name).Value;
        }

        public static uint? GetUint32Nullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as uint?;
        }

        public static int? GetInt32Nullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as int?;
        }

        public static DateTime GetDateTime(this IDataReader reader, string name)
        {
            return GetDateTimeNullable(reader, name).Value;
        }

        public static DateTime? GetDateTimeNullable(this IDataReader reader, string name)
        {
            return reader.GetValue(reader.GetOrdinal(name)) as DateTime?;
        }
    }
}