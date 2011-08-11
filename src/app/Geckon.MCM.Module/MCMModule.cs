using System;
using System.Linq;
using System.Xml.Linq;
using Geckon.Portal.Core.Standard.Module;

namespace Geckon.MCM.Module
{
    public class MCMModule : AModule
    {
        #region Properties

        private String ConnectionString { get; set; }

        #endregion
        #region Construction

        public override void Init( XElement config )
        {
            ConnectionString = config.Descendants( "ConnectionString" ).First().Value;
        }

        #endregion
        #region MyRegion
        


        #endregion
    }
}
