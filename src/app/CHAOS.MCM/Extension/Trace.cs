namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

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
