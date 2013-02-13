namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class DestinationInfoMapping : IReaderMapping<DestinationInfo>
    {
        public IEnumerable<DestinationInfo> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new DestinationInfo
                {
                    ID               = reader.GetUint32("ID"),
                    Name             = reader.GetString("Name"),
                    SubscriptionGuid = reader.GetGuid("SubscriptionGuid"),
                    DateCreated      = reader.GetDateTime("DateCreated"),
                    BasePath         = reader.GetString("BasePath"),
                    StringFormat     = reader.GetString("StringFormat"),
                    Token            = reader.GetString("Token") 
                };
            }
        }
    }
}