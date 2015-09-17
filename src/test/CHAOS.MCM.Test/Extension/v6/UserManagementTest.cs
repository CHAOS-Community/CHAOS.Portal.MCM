using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto.Standard;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Extension.v6
{
	[TestFixture]
	public class UserManagementTest : TestBase
	{
		[Test]
		public void GetUserFolder_UserHasFolder_ReturnFolder()
		{
			var extension = Make_UserManagementExtension();
			var userInfo = SetupUser();
			var expected = new Folder(2, 0, 1, null, userInfo.Guid.ToString(), DateTime.Now);

			McmRepository.Setup(m => m.FolderGet(null, null, null)).Returns(new List<Folder>
				{
					new Folder(1, 1, null, null, "Users", DateTime.Now),
					expected
				});

			var result = extension.GetUserFolder();

			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0], Is.EqualTo(expected));
		}

		[Test]
		public void GetUserFolder_UserDoesNotHaveFolderDontCreate_ReturnEmptyList()
		{
			var extension = Make_UserManagementExtension();
			var userInfo = SetupUser();

			McmRepository.Setup(m => m.FolderGet(null, null, null)).Returns(new List<Folder>
				{
					new Folder(1, 1, null, null, "Users", DateTime.Now)
				});

			var result = extension.GetUserFolder(null, false);

			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetUserFolder_UserDoesNotHaveFolder_CreateAndReturnFolder()
		{
			var extension = Make_UserManagementExtension();
			var userInfo = SetupUser();
			var expected = new Folder(2, 0, 1, null, userInfo.Guid.ToString(), DateTime.Now);

			McmRepository.Setup(m => m.FolderGet(null, null, null)).Returns(() => new List<Folder>
				{
					new Folder(1, 1, null, null, "Users", DateTime.Now)
				});
			McmRepository.Setup(m => m.FolderCreate(userInfo.Guid, null, userInfo.Guid.ToString(), 1, 0)).Returns(2);
			McmRepository.Setup(m => m.FolderGet(2, null, null)).Returns(new List<Folder>{expected});

			var result = extension.GetUserFolder();

			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0], Is.EqualTo(expected));
		}
	}
}