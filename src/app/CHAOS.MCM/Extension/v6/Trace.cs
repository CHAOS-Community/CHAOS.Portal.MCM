using System.Collections.Generic;
using Chaos.Portal.Core.Data.Model;
using CHAOS.Serialization;

namespace Chaos.Mcm.Extension.v6
{
    public class Trace : AResult
    {
        private List<string> lines = new List<string>();

        [Serialize]
        public List<string> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
            }
        }
    }
}
