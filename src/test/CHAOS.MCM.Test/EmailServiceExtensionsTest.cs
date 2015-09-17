using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Amazon.SimpleEmail.Model;
using Chaos.Mcm.Data;
using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.EmailService;
using Moq;
using NUnit.Framework;
using Object = Chaos.Mcm.Data.Dto.Object;

namespace Chaos.Mcm.Test
{
	[TestFixture]
	public class EmailServiceExtensionsTest
	{
		[Test]
		public void SendTemplate_GivenValidSimpleTemplate_SendMail()
		{
			var senderMock = new Mock<IEmailSender>();
			var mcmRepository = new Mock<IMcmRepository>();
			var service = new Portal.EmailService.EmailService(senderMock.Object);

			const string to = "to@test.com";
			const string from = "from@test.com";
			const string subject = "Test Email";

			var templateMetadataIdentifier = new MetadataIdentifier(Guid.NewGuid(), Guid.NewGuid(), null);
			var templateObject = CreateObjectWithMetadata(templateMetadataIdentifier,
				"<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"><xsl:template match=\"/\"><html><body><h1>Hallo</h1><p>How are you <xsl:value-of select=\"//Name\"/>?</p></body></html></xsl:template></xsl:stylesheet>");
			
			var data = XElement.Parse("<Person><Name>Albert Einstein</Name></Person>");

			SendEmailRequest request = null;

			mcmRepository.Setup(r => r.ObjectGet(templateMetadataIdentifier.ObjectGuid, true, false, false, false, false)).Returns(templateObject);
			senderMock.Setup(s => s.Send(It.IsAny<SendEmailRequest>())).Callback<SendEmailRequest>(r => request = r);

			service.SendTemplate(from, to, subject, mcmRepository.Object, templateMetadataIdentifier, data);

			senderMock.Verify(s => s.Send(It.IsAny<SendEmailRequest>()), Times.Once());

			Assert.That(request, Is.Not.Null);
			Assert.That(request.Message.Body.Html.Data, Is.EqualTo("<html>\r\n  <body>\r\n    <h1>Hallo</h1>\r\n    <p>How are you Albert Einstein?</p>\r\n  </body>\r\n</html>"));
		}

		[Test]
		public void SendTemplate_GivenValidEmailTemplate_SendMail()
		{
			var senderMock = new Mock<IEmailSender>();
			var mcmRepository = new Mock<IMcmRepository>();
			var service = new Portal.EmailService.EmailService(senderMock.Object);

			const string to = "to@test.com";
			const string from = "from@test.com";
			const string subject = "Test Email";

			var templateMetadataIdentifier = new MetadataIdentifier(Guid.NewGuid(), Guid.NewGuid(), null);
			var dataMetadataIdentifier = new MetadataIdentifier(Guid.NewGuid(), Guid.NewGuid(), null);
			var templateObject = CreateObjectWithMetadata(templateMetadataIdentifier, 
				string.Format("<EmailTemplate><From>{0}</From><Subject>{1}</Subject><Body><xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"><xsl:template match=\"/\"><html><body><h1>Hallo</h1><p>How are you <xsl:value-of select=\"//Person/Name\"/>?</p></body></html></xsl:template></xsl:stylesheet></Body></EmailTemplate>",
					@from, subject));
			var dataObject = CreateObjectWithMetadata(dataMetadataIdentifier, "<Person><Name>Albert Einstein</Name></Person>");

			SendEmailRequest request = null;

			mcmRepository.Setup(r => r.ObjectGet(templateMetadataIdentifier.ObjectGuid, true, false, false, false, false)).Returns(templateObject);
			mcmRepository.Setup(r => r.ObjectGet(dataMetadataIdentifier.ObjectGuid, true, false, false, false, false)).Returns(dataObject);
			senderMock.Setup(s => s.Send(It.IsAny<SendEmailRequest>())).Callback<SendEmailRequest>(r => request = r);

			service.SendFromEmailSchema(to, mcmRepository.Object, templateMetadataIdentifier, new List<MetadataIdentifier> { dataMetadataIdentifier });

			senderMock.Verify(s => s.Send(It.IsAny<SendEmailRequest>()), Times.Once());

			Assert.That(request, Is.Not.Null);
			Assert.That(request.Message.Body.Html.Data, Is.EqualTo("<html>\r\n  <body>\r\n    <h1>Hallo</h1>\r\n    <p>How are you Albert Einstein?</p>\r\n  </body>\r\n</html>"));
		}

		private static Object CreateObjectWithMetadata(MetadataIdentifier metadataIdentifier, string metadata)
		{
			return new Object
			{
				Metadatas = new List<Metadata>
				{
					new Metadata
					{
						MetadataSchemaGuid = metadataIdentifier.MetadataSchemaGuid,
						LanguageCode = metadataIdentifier.LanguageCode,
						MetadataXml = XDocument.Parse(metadata)
					}
				}
			};
		}
	}
}