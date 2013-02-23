namespace Chaos.Mcm.Extension
{
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;

    public class Mcm : AMcmExtension
    {
        #region Initialization

        public Mcm(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        public Mcm()
            : base()
        {
        }

        #endregion

        //		public ScalarResult Test_ReIndex( ICallContext callContext, uint? folderID, bool? clearIndex )
//		{
//            var index = (Solr)callContext.IndexManager.GetIndex<Mcm>();
//
//            if (clearIndex.HasValue && clearIndex.Value)
//                index.RemoveAll(false);
//
//            const uint pageSize = 1000;
//
//            for (uint i = 0; ; i++)
//            {
//                // using ensure the Database Context is disposed once in a while, to avoid OOM exceptions
//                using (var db = DefaultMCMEntities)
//                {
//                    var objects = db.Object_Get(folderID, true, false, true, true, true, i, pageSize).ToDto().ToList();
//
//                    PutObjectInIndex(index, objects);
//
//                    if (objects.Count() != pageSize)
//                        break;
//                }
//            }
//
//            return new ScalarResult(1);
//		}
        public void Index(ICallContext callContext)
        {
            const uint pageSize = 1000;
            
            for (uint i = 0; ; i++)
            {
                var objects = McmRepository.ObjectGet(null, i, pageSize, true, true, true, true, true);
                
                callContext.ViewManager.Index(objects);
            
                if (objects.Count != pageSize)  break;
            }
        }
    }
}
