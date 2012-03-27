using Geckon.Portal.Core.Standard.Extension;

namespace Geckon.MCM.Extension.Folder
{
    public class FolderExtension : AExtension
    {
        #region GET

        public void Get( CallContext callContext, uint? id, uint? folderTypeID, uint? parentID )
        {
            
        }

        #endregion
        #region CREATE

        public void Create( CallContext callContext, string subscriptionGUID, string title, uint? parentID, uint folderTypeID )
        {

        }

        #endregion
        #region UPDATE

        public void Update( CallContext callContext, uint id, string newTitle, int? newFolderTypeID )
        {

        }

        #endregion
        #region DELETE

        public void Delete( CallContext callContext, uint id )
        {

        }

        #endregion
    }
}
