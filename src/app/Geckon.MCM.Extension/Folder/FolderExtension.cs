using Geckon.Portal.Core.Standard.Extension;

namespace Geckon.MCM.Extension.Folder
{
    public class FolderExtension : AExtension
    {
        #region GET

        public void Get( CallContext callContext, int? id, int? folderTypeID, int? parentID )
        {
            
        }

        #endregion
        #region CREATE

        public void Create( CallContext callContext, string subscriptionGUID, string title, int? parentID, int folderTypeID )
        {

        }

        #endregion
        #region UPDATE

        public void Update( CallContext callContext, int id, string newTitle, int? newFolderTypeID )
        {

        }

        #endregion
        #region DELETE

        public void Delete( CallContext callContext, int id )
        {

        }

        #endregion
    }
}
