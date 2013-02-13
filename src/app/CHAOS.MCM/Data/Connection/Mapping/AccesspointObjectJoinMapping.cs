namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Mcm.Data.Dto;

    public class AccesspointObjectJoinMapping : IReaderMapping<ObjectAccessPoint>
    {
        public IEnumerable<ObjectAccessPoint> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new ObjectAccessPoint
                                 {
                                    AccessPointGuid = reader.GetGuid("AccessPointGUID"),
                                    ObjectGuid      = reader.GetGuid("ObjectGuid"),
                                    StartDate       = reader.GetDateTime("StartDate"),
                                    EndDate         = reader.GetDateTimeNullable("EndDate"),
                                    DateCreated     = reader.GetDateTime("DateCreated"),
                                    DateModified    = reader.GetDateTimeNullable("DateModified")

                                 };
            }
        }
    }
}