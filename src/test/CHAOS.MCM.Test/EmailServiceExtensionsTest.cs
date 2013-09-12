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
		public void SendTemplate_GivenValidParameter_SendMail()
		{
			var senderMock = new Mock<IEmailSender>();
			var mcmRepository = new Mock<IMcmRepository>();
			var service = new Portal.EmailService.EmailService(senderMock.Object);

			const string to = "to@test.com";
			const string from = "from@test.com";
			const string subject = "Test Email";
			var templateMetadataIdentifier = new MetadataIdentifier(Guid.NewGuid(), Guid.NewGuid(), null);
			var templateObject = new Object
			{
				Metadatas = new List<Metadata>
				{
					new Metadata
					{
						MetadataSchemaGuid = templateMetadataIdentifier.MetadataSchemaGuid,
						LanguageCode = templateMetadataIdentifier.LanguageCode,
						MetadataXml = XDocument.Load("Data/EmailTemplate01.xml")
					}
				}
			};
			var data = XElement.Parse("<Person><Name>Albert Einstein</Name></Person>");

			SendEmailRequest request = null;

			mcmRepository.Setup(r => r.ObjectGet(templateMetadataIdentifier.ObjectGuid, true, false, false, false, false)).Returns(templateObject);
			senderMock.Setup(s => s.Send(It.IsAny<SendEmailRequest>())).Callback<SendEmailRequest>(r => request = r);

			service.SendTemplate(from, to, subject, mcmRepository.Object, templateMetadataIdentifier, data);

			senderMock.Verify(s => s.Send(It.IsAny<SendEmailRequest>()), Times.Once());

			Assert.That(request, Is.Not.Null);
			Assert.That(request.Message.Body.Html.Data, Is.EqualTo("<html>\r\n  <body>\r\n    <h1>Hallo</h1>\r\n    <p>\r\n\t\t\t\t\tHow are you Albert Einstein?\r\n\t\t\t\t</p>\r\n  </body>\r\n</html>"));
		}
	}
}