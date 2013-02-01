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
            var result = new ObjectRelationInfo();

            result.Object1Guid        = new Guid((byte[])row[0].Value);
            result.Object2Guid        = new Guid((byte[])row[1].Value);
            result.MetadataGuid       = new Guid((byte[])row[2].Value);
            result.Sequence           = row[3].Value is DBNull ? null : (int?) row[3].Value;
            result.ObjectRelationType = (string) row[4].Value;
            result.LanguageCode       = (string) row[5].Value;
            result.MetadataSchemaGuid = new Guid((byte[])row[6].Value);
            result.MetadataXml        = XDocument.Load(new MemoryStream((byte[])row[7].Value));

            return result;
        }
    }
}