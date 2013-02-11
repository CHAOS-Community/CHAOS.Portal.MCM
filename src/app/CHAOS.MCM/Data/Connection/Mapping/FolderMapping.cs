namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto.Standard;

    public class FolderMapping : IReaderMapping<Folder>
    {
        public IEnumerable<Folder> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new Folder
                                 {
                                    ID               = reader.GetUint32("ID"),
                                    ParentID         = reader.GetUint32Nullable("ParentID"),
                                    Name             = reader.GetString("Name"),
                                    SubscriptionGuid = reader.GetGuidNullable("SubscriptionGuid"),
                                    DateCreated      = reader.GetDateTime("DateCreated"),

                                 };
            }
        }
    }
}