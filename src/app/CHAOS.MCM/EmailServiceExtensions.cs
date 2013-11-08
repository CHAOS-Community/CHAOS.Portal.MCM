﻿namespace Chaos.Mcm
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using Data;
	using Data.Dto;
	using Portal.Core.EmailService;

	public static class EmailServiceExtensions
	{
		public static void SendTemplate(this IEmailService emailService, string from, string to, string subject, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			emailService.SendTemplate(from, new List<string> { to }, null, subject, repository, template, data);
		}

		public static void SendTemplate(this IEmailService emailService, string from, IEnumerable<string> to, IEnumerable<string> bcc, string subject, IMcmRepository repository, MetadataIdentifier template, XElement data)
		{
			var templateMetadata = GetMetadata(repository, template);

			if (templateMetadata == null)
				throw new System.Exception(string.Format("Failed to get template. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", template.ObjectGuid, template.MetadataSchemaGuid, template.LanguageCode));

			emailService.SendTemplate(from, to, bcc, subject, templateMetadata, data);
		}

		public static void SendTemplate(this IEmailService emailService, string from, string to, string subject, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			emailService.SendTemplate(from, new List<string> { to }, null, subject, repository, template, datas);
		}

		public static void SendTemplate(this IEmailService emailService, string from, IEnumerable<string> to, IEnumerable<string> bcc, string subject, IMcmRepository repository, MetadataIdentifier template, IList<MetadataIdentifier> datas)
		{
			var templateMetadata = GetMetadata(repository, template);

			if (templateMetadata == null)
				throw new System.Exception(string.Format("Failed to get template. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", template.ObjectGuid, template.MetadataSchemaGuid, template.LanguageCode));

			var dataMetadatas = GetDatas(repository, datas);

			emailService.SendTemplate(from, to, bcc, subject, templateMetadata, dataMetadatas);
		}

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

		private static XElement GetMetadata(IMcmRepository repository, MetadataIdentifier metadata)
		{
			var templateObject = repository.ObjectGet(metadata.ObjectGuid, true);

			if (templateObject == null || templateObject.Metadatas == null || templateObject.Metadatas.Count == 0)
				return null;

			var templateMetadata = templateObject.Metadatas.FirstOrDefault(m => m.MetadataSchemaGuid == metadata.MetadataSchemaGuid && m.LanguageCode == metadata.LanguageCode);

			if (templateMetadata == null)
				return null;

			return templateMetadata.MetadataXml.Root;
		}

		private static IList<XElement> GetDatas(IMcmRepository repository, IEnumerable<MetadataIdentifier> metadatas)
		{
			var dataMetadatas = new List<XElement>();

			foreach (var metadataIdentifier in metadatas)
			{
				var metadata = GetMetadata(repository, metadataIdentifier);

				if (metadata == null)
					throw new System.Exception(string.Format("Failed to data. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", metadataIdentifier.ObjectGuid, metadataIdentifier.MetadataSchemaGuid, metadataIdentifier.LanguageCode));

				dataMetadatas.Add(metadata);
			}

			return dataMetadatas;
		}

		private static EmailTemplate GetEmailTemplate(IMcmRepository repository, MetadataIdentifier template)
		{
			var templateMetadata = GetMetadata(repository, template);

			if (templateMetadata == null)
				throw new System.Exception(string.Format("Failed to get template. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", template.ObjectGuid, template.MetadataSchemaGuid, template.LanguageCode));

			var from = templateMetadata.Element("From");
			var subject = templateMetadata.Element("Subject");
			var body = templateMetadata.Element("Body");

			if(from == null || subject == null || body == null)
				throw new System.Exception(string.Format("Metadata did not match email template. ObjectGuid: {0} SchemaGuid: {1} LanguageCode: {2}", template.ObjectGuid, template.MetadataSchemaGuid, template.LanguageCode));

			return new EmailTemplate
						{
							From = from.Value,
							Subject = subject.Value,
							Body = body.Elements().First()
						};
		}
	}
}