namespace Chaos.Mcm.Extension
{
    public class Mcm : AMcmExtension
    {
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
    }
}
