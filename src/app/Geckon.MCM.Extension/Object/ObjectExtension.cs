using System;
using Geckon.Portal.Core.Standard.Extension;
using Geckon.Index;

namespace CHAOS.MCM.Extension.Object
{
    public class ObjectExtension : AExtension
    {
        public void Get(CallContext callContext, IQuery query, bool? includeMetadata, bool? includeFiles, bool? includeObjectRelations )
        {
            
        }

		//public void Delete( CallContext callContext, Guid GUID, int folderID ){}
		public void Create( CallContext callContext, Guid? GUID, int objectTypeID, int folderID ){}
		//public void PutInFolder( CallContext callContext, Guid GUID, int folderID, int objectFolderTypeID ) {}
    }
}
