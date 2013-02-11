namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;

    using Chaos.Mcm.Extension;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Data.Dto;
    using Chaos.Portal.Data.Dto.Standard;

    using Moq;

    using NUnit.Framework;

    using Folder = Chaos.Mcm.Data.Dto.Standard.Folder;

    [TestFixture]
    public class AMcmExtensionTest : TestBase
    {
        [Test]
        public void HasPermissionToObject_GivenObjectGuidWithPermission_ReturnTrue()
        {
            var extension  = Make_AMcmExtension();
            var objectGuid = new Guid("af40f5e3-2cbf-944f-b833-6c444ad760e1");
            var userInfo   = new UserInfo { Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352") };
            var groups     = new IGroup[] { new Group { Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352") } };
            var folderDtos = new List<Data.Dto.Standard.Folder> { new Data.Dto.Standard.Folder { ID = 1 } };
            SetupHasPermissionToObject(userInfo, groups, objectGuid, folderDtos);

            var result = extension.HasPermissionToObject(CallContext.Object, objectGuid, FolderPermission.Read);

            Assert.IsTrue(result);
            McmRepository.Verify(m => m.FolderGet(null, objectGuid));
        }

        private AMcmExtensionStub Make_AMcmExtension()
        {
            return (AMcmExtensionStub)new AMcmExtensionStub().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }
    }

    public class AMcmExtensionStub : AMcmExtension
    {
    }
}