using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Mcm.Data.Dto;
using Moq;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Extension
{
	[TestFixture]
	public class UserProfileTest : TestBase
	{
		[Test]
		public void Get_UserHasProfile_ReturnProfile()
		{
			var extension = Make_UserProfileExtension();
			var schemaGuid = Guid.NewGuid();
			var user = SetupUser();
			var expected = Make_MetadataDto();
			expected.MetadataSchemaGuid = schemaGuid;
			expected.MetadataXml = XDocument.Parse("<UserProfil Name=\"Ben Dover\"/>");

			var userObject = Make_Object();
			userObject.Guid = user.Guid;
			userObject.Metadatas = new List<Metadata>
				{
					Make_MetadataDto(),
					expected
				};

			McmRepository.Setup(m => m.ObjectGet(user.Guid, true, false, false, false, false)).Returns(userObject);

			var result = extension.Get(schemaGuid);
			
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0].MetadataXml, Is.EqualTo(expected.MetadataXml));
		}

		[Test]
		public void Get_UserDoesNotHaveProfile_ReturnEmpty()
		{
			var extension = Make_UserProfileExtension();
			var schemaGuid = Guid.NewGuid();
			var user = SetupUser();

			var userObject = Make_Object();
			userObject.Guid = user.Guid;
			userObject.Metadatas = new List<Metadata>
				{
					Make_MetadataDto()
				};

			McmRepository.Setup(m => m.ObjectGet(user.Guid, true, false, false, false, false)).Returns(userObject);

			var result = extension.Get(schemaGuid);


			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void Set_UserHasUserObjectButNoMetadata_SetUserMetadata()
		{
			var extension = Make_UserProfileExtension();
			var schemaGuid = Guid.NewGuid();
			var user = SetupUser();
			var expected = Make_MetadataDto();
			expected.MetadataSchemaGuid = schemaGuid;
			expected.MetadataXml = XDocument.Parse("<UserProfil Name=\"Ben Dover\"/>");

			var userObject = Make_Object();
			userObject.Guid = user.Guid;
			userObject.Metadatas = new List<Metadata>
				{
					Make_MetadataDto()
				};

			McmRepository.Setup(m => m.ObjectGet(user.Guid, true, false, false, false, false)).Returns(userObject);
			McmRepository.Setup(m => m.MetadataSet(userObject.Guid, It.IsAny<Guid>(), schemaGuid, null, 0, expected.MetadataXml, user.Guid)).Returns(1);

			var result = extension.Set(schemaGuid, expected.MetadataXml);

			McmRepository.Verify(m => m.MetadataSet(userObject.Guid, It.IsAny<Guid>(), schemaGuid, null, 0, expected.MetadataXml, user.Guid));

			Assert.That(result.Value, Is.EqualTo(1));
		}

		[Test]
		public void Set_UserHasUserObjectAndMetadata_SetUserMetadata()
		{
			var extension = Make_UserProfileExtension();
			var schemaGuid = Guid.NewGuid();
			var user = SetupUser();
			var expected = Make_MetadataDto();
			expected.MetadataSchemaGuid = schemaGuid;
			expected.MetadataXml = XDocument.Parse("<UserProfil Name=\"Ben Dover\"/>");
			expected.RevisionID = 5;

			var userObject = Make_Object();
			userObject.Guid = user.Guid;
			userObject.Metadatas = new List<Metadata>
				{
					Make_MetadataDto(),
					expected
				};

			McmRepository.Setup(m => m.ObjectGet(user.Guid, true, false, false, false, false)).Returns(userObject);
			McmRepository.Setup(m => m.MetadataSet(userObject.Guid, It.IsAny<Guid>(), schemaGuid, null, 5, expected.MetadataXml, user.Guid)).Returns(1);

			var result = extension.Set(schemaGuid, expected.MetadataXml);

			McmRepository.Verify(m => m.MetadataSet(userObject.Guid, It.IsAny<Guid>(), schemaGuid, null, 5, expected.MetadataXml, user.Guid));

			Assert.That(result.Value, Is.EqualTo(1));
		}
	}
}