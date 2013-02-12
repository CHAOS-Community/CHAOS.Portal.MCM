namespace Chaos.Mcm.Data.Connection.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;

    public class FolderPermissionMapping : IReaderMapping<FolderPermission>
    {
        public IEnumerable<FolderPermission> Map(IDataReader reader)
        {
            var folderPermissions = new Dictionary<uint, FolderPermission>();

            while(reader.Read())
            {
                var folderID = reader.GetUint32("FolderID");

                if (!folderPermissions.ContainsKey(folderID))
                    folderPermissions.Add(folderID, new FolderPermission());
                    
                folderPermissions[folderID].UserPermissions.Add(new EntityPermission
                    {
                        Guid       = reader.GetGuid("UserGUID"),
                        Permission = (Mcm.Permission.FolderPermission)reader.GetUint32("Permission")
                    });
            }

            reader.NextResult();

            while(reader.Read())
            {
                var folderID = reader.GetUint32("FolderID");

                if (!folderPermissions.ContainsKey(folderID))
                    folderPermissions.Add(folderID, new FolderPermission());
                    
                folderPermissions[folderID].GroupPermissions.Add(new EntityPermission
                    {
                        Guid       = reader.GetGuid("GroupGUID"),
                        Permission = (Mcm.Permission.FolderPermission)reader.GetUint32("Permission")
                    });
            }

            return folderPermissions.Values;
        }
    }
}
