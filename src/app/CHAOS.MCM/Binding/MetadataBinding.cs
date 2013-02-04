namespace Chaos.Mcm.Binding
{
    using System.Reflection;
    using System.Xml.Linq;

    using CHAOS;

    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Portal;
    using Chaos.Portal.Bindings;

    public class MetadataBinding : IParameterBinding
    {
        public object Bind(ICallContext callContext, ParameterInfo parameterInfo)
        {
            var param    = callContext.Request.Parameters;
            var metadata = new Metadata();

            if(param.ContainsKey("metadataSchemaGUID")) metadata.MetadataSchemaGUID = new UUID(param["metadataSchemaGUID"]);
            if(param.ContainsKey("languageCode"))       metadata.LanguageCode       = param["languageCode"];
            if(param.ContainsKey("revisionID"))         metadata.RevisionID         = uint.Parse(param["revisionID"]);
            if(param.ContainsKey("metadataXML"))        metadata.MetadataXML        = XDocument.Parse(param["metadataXML"]);

            return metadata;
        }
    }
}