namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal;

    public class Destination : AMcmExtension
    {
        public IEnumerable<DestinationInfo> Get(ICallContext callContext, uint? id)
        {
            return McmRepository.DestinationGet(id);
        }
    }
}