using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;

namespace Chaos.Mcm.Extension.v6
{
    public class Format : AMcmExtension
    {
        #region Initialization

        public Format(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
        {
        }

        #endregion
        #region Business Logic

        public IEnumerable<Data.Dto.Format> Get(uint? id, string name)
        {
            return McmRepository.FormatGet(id, name);
        }

        public Data.Dto.Format Create(uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension)
        {
            var result = McmRepository.FormatCreate(formatCategoryID, name, formatXml, mimeType, extension);

            return Get(result, null).First();
        }

        #endregion
    }
}
