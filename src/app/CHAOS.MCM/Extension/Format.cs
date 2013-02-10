using System.Collections.Generic;

using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Extension
{
    using System.Xml.Linq;

    public class Format : AMcmExtension
    {
        #region Business Logic

        public IEnumerable<Data.Dto.Standard.Format> Get(ICallContext callContext, uint? id, string name)
        {
            return McmRepository.FormatGet(id, name);
        }

        public ScalarResult Create(ICallContext callContext, uint? formatCategoryID, string name, XDocument formatXml, string mimeType, string extension )
        {
            var result = McmRepository.FormatCreate(formatCategoryID, name, formatXml, mimeType, extension);

            return new ScalarResult((int)result);
        }

        #endregion
    }
}
