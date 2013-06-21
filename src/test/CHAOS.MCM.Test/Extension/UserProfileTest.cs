using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Chaos.Mcm.Data.Dto;
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
			
			
			Assert.That(result.MetadataXml, Is.EqualTo(expected.MetadataXml));
		}
	}
}