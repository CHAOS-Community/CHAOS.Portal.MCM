namespace Chaos.Mcm.Binding
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal;
    using Chaos.Portal.Bindings;

    public class MetadataBinding : IParameterBinding
    {
        public object Bind(ICallContext callContext, ParameterInfo parameterInfo)
        {
            var param    = callContext.Request.Parameters;
            var metadata = new NewMetadata();

            if(param.ContainsKey("metadataSchemaGUID")) metadata.MetadataSchemaGuid = new Guid(param["metadataSchemaGUID"]);
            if(param.ContainsKey("languageCode"))       metadata.LanguageCode       = param["languageCode"];
            if(param.ContainsKey("revisionID"))         metadata.RevisionID         = uint.Parse(param["revisionID"]);
            if(param.ContainsKey("metadataXML"))        metadata.MetadataXml        = XDocument.Parse(param["metadataXML"]);

            return metadata;
        }
    }
}