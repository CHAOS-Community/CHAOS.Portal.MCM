using System;
using System.Collections.Generic;
using Chaos.Mcm.Data;
using Chaos.Mcm.Extension.v6;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using NUnit.Framework;
using Folder = Chaos.Mcm.Data.Dto.Standard.Folder;

namespace Chaos.Mcm.Test.Extension.v6
{
    [TestFixture]
    public class AMcmExtensionTest : TestBase
    {
        [Test]
        public void HasPermissionToObject_GivenObjectGuidWithPermission_ReturnTrue()
        {
            var extension  = Make_AMcmExtension();
            var objectGuid = new Guid("af40f5e3-2cbf-944f-b833-6c444ad760e1");
            var userInfo   = Make_User();
            var groups     = new [] { new Group { Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352") } };
            var folderDtos = new List<Folder> { new Folder { ID = 1 } };
            SetupHasPermissionToObject(userInfo, groups, objectGuid, folderDtos);

            var result = extension.HasPermissionToObject(objectGuid, FolderPermission.Read);

            Assert.IsTrue(result);
            McmRepository.Verify(m => m.FolderGet(null, null, objectGuid));
        }
    }

    public class AMcmExtensionStub : AMcmExtension
    {
        public AMcmExtensionStub(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager)
            : base(portalApplication, mcmRepository, permissionManager)
        {
        }
    }
}