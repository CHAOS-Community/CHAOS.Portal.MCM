namespace Chaos.Mcm
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using Data;
	using Data.Dto;
	using Portal.Core.EmailService;

	public static class EmailServiceExtensions
	{
		#region SendTemplate

		public static void SendTemplate(this IEmailService emailService, string from, string to, string subject, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			emailService.SendTemplate(from, new List<string> { to }, null, subject, repository, template, data);
		}

		public static void SendTemplate(this IEmailService emailService, string from, IEnumerable<string> to, IEnumerable<string> bcc, string subject, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			var templateMetadata = GetMetadata(repository, template, "Failed to get template");

			emailService.SendTemplate(from, to, bcc, subject, templateMetadata, data);
		}

		public static void SendTemplate(this IEmailService emailService, string from, string to, string subject, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			emailService.SendTemplate(from, new List<string> { to }, null, subject, repository, template, datas);
		}

		public static void SendTemplate(this IEmailService emailService, string from, IEnumerable<string> to, IEnumerable<string> bcc, string subject, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			var templateMetadata = GetMetadata(repository, template, "Failed to get template");

			var dataMetadatas = GetDatas(repository, datas);

			emailService.SendTemplate(from, to, bcc, subject, templateMetadata, dataMetadatas);
		}

		#endregion
		#region SendFromEmailSchema
		#region Data from XElement

		public static void SendFromEmailSchema(this IEmailService emailService, string to, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			emailService.SendFromEmailSchema(new List<string>{to}, null, repository, template, data);
		}

		public static void SendFromEmailSchema(this IEmailService emailService, IList<string> to, IEnumerable<string> bcc, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			emailService.SendFromEmailSchema(to, bcc, repository, template, new List<XElement> { data });
		}

		public static void SendFromEmailSchema(this IEmailService emailService, string to, IMcmRepository repository, MetadataIdentifier template, IList<XElement> datas)
		{
			emailService.SendFromEmailSchema(new List<string> { to }, null, repository, template, datas);
		}

		public static void SendFromEmailSchema(this IEmailService emailService, IList<string> to, IEnumerable<string> bcc, IMcmRepository repository, MetadataIdentifier template, IList<XElement> datas)
		{
			var templateMetadata = GetEmailTemplate(repository, template);

			emailService.SendTemplate(templateMetadata.From, to, bcc, templateMetadata.Subject, templateMetadata.Body, datas);
		}

		#endregion
		#region Data from Objects

		public static void SendFromEmailSchema(this IEmailService emailService, string to, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			var templateMetadata = GetEmailTemplate(repository, template);

			var dataMetadatas = GetDatas(repository, datas);

			emailService.SendTemplate(templateMetadata.From, to, templateMetadata.Subject, templateMetadata.Body, dataMetadatas);
		}

		public static void SendFromEmailSchema(this IEmailService emailService, IEnumerable<string> to, IEnumerable<string> bcc, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			var templateMetadata = GetEmailTemplate(repository, template);

			var dataMetadatas = GetDatas(repository, datas);

			emailService.SendTemplate(templateMetadata.From, to, bcc, templateMetadata.Subject, templateMetadata.Body, dataMetadatas);
		}
		
		#endregion
		#endregion
		#region Utility

		private static XElement GetMetadata(IMcmRepository repository, MetadataIdentifier metadata, string errorMessage = "Failed to get metadata")
		{
			var templateObject = repository.ObjectGet(metadata.ObjectGuid, true);

			if(templateObject == null)
				throw new System.Exception(string.Format("{0}, object not found. ObjectGuid: {1}", errorMessage, metadata.ObjectGuid));

			if (templateObject.Metadatas == null || templateObject.Metadatas.Count == 0)
				throw new System.Exception(string.Format("{0}, object has no metadata. ObjectGuid: {1}", errorMessage, metadata.ObjectGuid));

			var templateMetadata = templateObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadata.MetadataSchemaGuid && m.LanguageCode == metadata.LanguageCode);

			if (templateMetadata == null)
				throw new System.Exception(string.Format("{0}, no matching schema and language code. ObjectGuid: {1} SchemaGuid: {2} LanguageCode: {3}", errorMessage, metadata.ObjectGuid, metadata.MetadataSchemaGuid, metadata.LanguageCode));

			return templateMetadata.MetadataXml.Root;
		}

		private static IList<XElement> GetDatas(IMcmRepository repository, IEnumerable<MetadataIdentifier> metadatas)
		{
			return metadatas.Select(metadataIdentifier => GetMetadata(repository, metadataIdentifier, "Failed to data")).ToList();
		}

		private static EmailTemplate GetEmailTemplate(IMcmRepository repository, MetadataIdentifier template)
		{
			var templateMetadata = GetMetadata(repository, template, "Failed to get template");

			var from = templateMetadata.Element("From");
			var subject = templateMetadata.Element("Subject");
			var body = templateMetadata.Element("Body");

			if (from == null || subject == null || body == null)
				throw new System.Exception(string.Format("Metadata did not match email template. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", template.ObjectGuid, template.MetadataSchemaGuid, template.LanguageCode));

			return new EmailTemplate
			{
				From = from.Value,
				Subject = subject.Value,
				Body = body.Elements().First()
			};
		}

		#endregion
	}
}