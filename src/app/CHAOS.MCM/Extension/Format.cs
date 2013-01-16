using System.Collections.Generic;
using System.Linq;
using Chaos.Mcm.Data.EF;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using Chaos.Portal.Exceptions;

namespace Chaos.Mcm.Extension
{
    public class Format : AMcmExtension
    {
        #region Business Logic

        public IEnumerable<Data.Dto.Standard.Format> Get(ICallContext callContext, uint? ID, string name)
        {
            using (var db = DefaultMCMEntities)
            {
                return db.Format_Get((int?) ID, name).ToDto().ToList();
            }
        }

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
