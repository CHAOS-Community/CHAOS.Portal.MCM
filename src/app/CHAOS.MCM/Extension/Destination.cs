namespace Chaos.Mcm.Extension
{
    using System.Collections.Generic;

    using Chaos.Portal;

    using DestinationInfo = Chaos.Mcm.Data.Dto.Standard.DestinationInfo;

    public class Destination : AMcmExtension
    {
        public IEnumerable<DestinationInfo> Destination_Get(ICallContext callContext, uint id)
        {
            return McmRepository.DestinationGet(id);
        }
    }
}