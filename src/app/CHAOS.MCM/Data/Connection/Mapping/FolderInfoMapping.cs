namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto.Standard;

    public class FolderInfoMapping : IReaderMapping<FolderInfo>
    {
        public IEnumerable<FolderInfo> Map( IDataReader reader )
        {
            while(reader.Read())
            {
                yield return new FolderInfo
                    {
                        ID                 = reader.GetUint32("ID"),
                        ParentID           = reader.GetUint32Nullable("ParentID"),
                        FolderTypeID       = reader.GetUint32("FolderTypeID"),
                        SubscriptionGuid   = reader.GetGuidNullable("SubscriptionGuid"),
                        Name               = reader.GetString("Name"),
                        DateCreated        = reader.GetDateTime("DateCreated"),
                        NumberOfObjects    = reader.GetUint32("NumberOfObjects"), 
                        NumberOfSubFolders = reader.GetUint32("NumberOfSubFolders"), 
                    };
            }
        }
    }
}
