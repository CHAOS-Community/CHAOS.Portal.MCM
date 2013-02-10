namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto.Standard;

    public class AccesspointObjectJoinMapping : IReaderMapping<AccessPoint_Object_Join>
    {
        public IEnumerable<AccessPoint_Object_Join> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new AccessPoint_Object_Join
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