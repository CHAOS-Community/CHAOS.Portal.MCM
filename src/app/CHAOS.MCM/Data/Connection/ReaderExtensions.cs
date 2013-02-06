namespace Chaos.Mcm.Data.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class ReaderExtensions
    {
        public static IList<TResultType> ToDto<TResultType>(this IList<KeyValuePair<string, object>[]> readerData) where TResultType : IKeyValueMapper, new()
        {
            return readerData.Select(Map<TResultType>).ToList();
        }

        private static TResultType Map<TResultType>(KeyValuePair<string, object>[] row) where TResultType : IKeyValueMapper, new()
        {
            var dto = new TResultType();

            dto.Map(row);

            return dto;
        }

        public static string GetString(this object value)
        {
            return value as string;
        }

        public static Guid? GetGuid(this object value)
        {
            return Convert.IsDBNull(value) ? (Guid?)null : new Guid(value as byte[]);
        }

        public static XDocument GetXDocument(this object value)
        {
            if (value is DBNull) return null;

            return XDocument.Parse(value as string);
        }

        public static uint? GetUint(this object value)
        {
            return value as uint?;
        }

        public static int? GetInt(this object value)
        {
            return value as int?;
        }

        public static DateTime? GetDateTime(this object value)
        {
            return value as DateTime?;
        }
    }
}