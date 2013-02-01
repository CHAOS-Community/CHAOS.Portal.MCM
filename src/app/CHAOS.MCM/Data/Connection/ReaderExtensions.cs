namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

            result.Object1Guid = new Guid((byte[])row[0].Value);

            return result;
        }
    }
}