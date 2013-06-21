﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Folder = Chaos.Mcm.Data.Dto.Standard.Folder;

namespace Chaos.Mcm.Test.Extension
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

			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void GetUserFolder_UserDoesNotHaveFolderDontCreate_ReturnNull()
		{
			var extension = Make_UserManagementExtension();
			var userInfo = SetupUser();

			McmRepository.Setup(m => m.FolderGet(null, null, null)).Returns(new List<Folder>
				{
					new Folder(1, 1, null, null, "Users", DateTime.Now)
				});

			var result = extension.GetUserFolder(null, false);

			Assert.That(result, Is.Null);
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
			McmRepository.Setup(m => m.ObjectCreate(userInfo.Guid, 0, 2)).Returns(1);

			var result = extension.GetUserFolder();

			McmRepository.Verify(m => m.ObjectCreate(userInfo.Guid, 0, 2));

			Assert.That(result, Is.EqualTo(expected));
		}
	}
}