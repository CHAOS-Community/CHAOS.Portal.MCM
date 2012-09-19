using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Portal.Exception;

namespace CHAOS.MCM.Module
{
    [Module("MCM")]
    public class FormatModule : AMCMModule
    {
        #region Business Logic

        [Datatype("Format","Get")]
        public IEnumerable<Data.DTO.Format> Get( ICallContext callContext, uint? ID, string name )
        {
            using (var db = DefaultMCMEntities)
            {
                return db.Format_Get((int?) ID, name).ToDTO().ToList();
            }
        }

        [Datatype("Format", "Create")]
        public ScalarResult Create(ICallContext callContext, uint? formatCategoryID, string name, string formatXML, string mimeType, string extension )
        {
            using (var db = DefaultMCMEntities)
            {
                var result = db.Format_Create((int?)formatCategoryID, name, formatXML, mimeType, extension).FirstOrDefault();

                if(result == null)
                    throw new UnhandledException("No result was received from the database");

                return new ScalarResult( result.Value );
            }
        }

        #endregion
    }
}
