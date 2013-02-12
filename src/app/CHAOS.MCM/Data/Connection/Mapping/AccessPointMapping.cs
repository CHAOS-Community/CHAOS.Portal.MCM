namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto;

    public class AccessPointMapping : IReaderMapping<AccessPoint>
    {
        public IEnumerable<AccessPoint> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new AccessPoint
                {
                    Guid             = reader.GetGuid("GUID"),
                    Name             = reader.GetString("Name"),
                    SubscriptionGuid = reader.GetGuid("SubscriptionGuid"),
                    DateCreated      = reader.GetDateTime("DateCreated")
                };
            }
        }
    }
}