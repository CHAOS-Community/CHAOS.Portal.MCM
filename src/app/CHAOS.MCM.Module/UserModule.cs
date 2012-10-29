using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class UserModule : AMCMModule
    {
        #region Get

        [Datatype("User","Get")]
        public void Get(ICallContext callContext, uint folderID)
        {
            using(var db = DefaultMCMEntities)
            {

            }
        }

        #endregion
    }
}
