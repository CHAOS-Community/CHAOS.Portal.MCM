using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Chaos.Mcm.Data;
using Chaos.Portal.Core.EmailService;

namespace Chaos.Mcm
{
	public static class EmailServiceExtensions
	{
		public static void SendWithXSLT(this IEmailService emailService, string from, string to, string subject, IMcmRepository repository, Guid templateObjectGuid, Guid templateMetadataSchemaGuid, string templateLanguageCode, string data)
		{
			emailService.SendWithXSLT(from, new List<string> { to }, subject, repository, templateObjectGuid, templateMetadataSchemaGuid, templateLanguageCode, data);
		}

		public static void SendWithXSLT(this IEmailService emailService, string from, IEnumerable<string> to, string subject, IMcmRepository repository, Guid templateObjectGuid, Guid templateMetadataSchemaGuid, string templateLanguageCode, string data)
		{
			var templateObject = repository.ObjectGet(templateObjectGuid, true);

			if(templateObject == null || templateObject.Metadatas == null || templateObject.Metadatas.Count == 0)
				throw new System.Exception(string.Format("Mail template not found. ObjectGuid: {0}", templateObjectGuid));

			var templateMetadata = templateObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == templateMetadataSchemaGuid && m.LanguageCode == templateLanguageCode);

			if(templateMetadata == null)
				throw new System.Exception(string.Format("Mail template not found on object. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", templateObjectGuid, templateMetadataSchemaGuid, templateLanguageCode));

			emailService.SendWithXSLT(from, to, subject, templateMetadata.MetadataXml.ToString(SaveOptions.DisableFormatting), data);
		}
	}
}