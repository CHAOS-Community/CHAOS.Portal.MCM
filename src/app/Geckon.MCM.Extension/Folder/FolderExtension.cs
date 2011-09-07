using Geckon.Portal.Core.Standard.Extension;

namespace Geckon.MCM.Extension.Folder
{
    public class FolderExtension : AExtension
    {
        #region GET

        public void Get( string sessionID, int? id, int? folderTypeID, int? parentID )
        {
            
        }

        #endregion
        #region CREATE

        public void Create( string sessionID, string subscriptionGUID, string title, int? parentID, int folderTypeID )
        {

        }

        #endregion
        #region UPDATE

        public void Update( string sessionID, int folderID, string newTitle, int? newParentID, int? newFolderTypeID )
        {

        }

        #endregion
        #region DELETE

        public void Delete( string sessionID, int id )
        {

        }

        #endregion
    }
}
