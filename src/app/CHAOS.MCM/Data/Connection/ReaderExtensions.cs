namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Dto.Standard;

    public static class ReaderExtensions
    {
        #region Fields

        private static IDictionary<Type, IMapping> _mappings;

        #endregion
        #region Initialization

        static ReaderExtensions()
        {
            _mappings = new Dictionary<Type, IMapping>();

            _mappings.Add(typeof(ObjectRelationInfo), new ObjectRelationInfoMapper());
        }

        #endregion

        public static IList<TResultType> ToDto<TResultType>(this IList<KeyValuePair<string, object>[]> readerData)
        {
            var type = typeof(TResultType);

            return readerData.Select(item => (TResultType) ToDto(item, type)).ToList();
        }

        public static TResultType ToDto<TResultType>(this KeyValuePair<string, object>[] readerData)
        {
            return (TResultType) ToDto(readerData, typeof(TResultType));
        }

        private static object ToDto(KeyValuePair<string, object>[] readerData, Type type)
        {
            return _mappings[type].Map(readerData);
        }
    }

    public class ObjectRelationInfoMapper : IMapping
    {
        public object Map(KeyValuePair<string, object>[] row)
        {
            var result = new ObjectRelationInfo
                {
                    Object1Guid          = GetGuid(row[0].Value).Value,
                    Object2Guid          = GetGuid(row[1].Value).Value,
                    MetadataGuid         = GetGuid(row[2].Value),
                    Sequence             = GetInt(row[3].Value),
                    ObjectRelationTypeID = GetUint(row[4].Value).Value,
                    ObjectRelationType   = GetString(row[5].Value),
                    LanguageCode         = GetString(row[6].Value),
                    MetadataSchemaGuid   = GetGuid(row[7].Value),
                    MetadataXml          = GetXDocument(row[8].Value)
                };

            return result;
        }

        private static Guid? GetGuid(object cell)
        {
            var value = cell;

            if (value is DBNull) return null;

            return new Guid((byte[])value);
        }

        private static XDocument GetXDocument(object cell)
        {
            if (cell is DBNull) return null;

            return XDocument.Parse((string)cell);
        }

        private static uint? GetUint(object value)
        {
            return (uint?)GetValue(value);
        }

        private static int? GetInt(object cell)
        {
            return (int?)GetValue(cell);
        }

        private static string GetString(object cell)
        {
            return (string)GetValue(cell);
        }

        private static object GetValue(object cell)
        {
            return cell is DBNull ? null : cell;
        }
    }
}