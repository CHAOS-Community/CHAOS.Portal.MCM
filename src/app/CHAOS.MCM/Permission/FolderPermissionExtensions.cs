namespace Chaos.Mcm.Permission
{
    public static class FolderPermissionExtensions
    {
        public static FolderPermission Or(this FolderPermission permission, FolderPermission orWith)
        {
            permission = permission | orWith;

            return permission;
        }
    }
}
