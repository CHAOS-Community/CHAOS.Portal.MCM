namespace Chaos.Mcm.Test.Extension
{
    using System;

    using Chaos.Mcm.Permission;

    using NUnit.Framework;

    using File = Chaos.Mcm.Data.Dto.File;

    [TestFixture]
    public class FileTest : TestBase
    {
        [Test]
        public void Set_UserHasPermission_CallMcmRepositoryToCreateAndRetrieveUsingId()
        {
            var extension = Make_FileExtension();
            var file      = Make_File();
            SetupHasPermissionToObject(FolderPermission.CreateUpdateObjects);
            McmRepository.Setup(m => m.FileCreate(file.ObjectGuid, file.ParentID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath, file.FormatID)).Returns(file.ID);
            McmRepository.Setup(m => m.FileGet(file.ID)).Returns(new [] { file });

            var result = extension.Create(CallContext.Object, file.ObjectGuid, file.ParentID, file.FormatID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath);

            Assert.AreEqual(file, result);
            McmRepository.Verify(m => m.FileCreate(file.ObjectGuid, file.ParentID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath, file.FormatID));
            McmRepository.Verify(m => m.FileGet(file.ID));
        }

        [Test]
        public void Delete_UserHasPermission_CallMcmRepositoryReturnsOne()
        {
            var extension = Make_FileExtension();
            var file      = Make_File();
            SetupHasPermissionToObject(FolderPermission.CreateUpdateObjects);
            McmRepository.Setup(m => m.FileDelete(file.ID)).Returns(1u);
            McmRepository.Setup(m => m.FileGet(file.ID)).Returns(new[] { file });

            var result = extension.Delete(CallContext.Object, file.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.FileDelete(file.ID));
            McmRepository.Verify(m => m.FileGet(file.ID));
        }

        #region Helpers

        private static File Make_File()
        {
            return new File
                {
                    ID               = 1,
                    ObjectGuid       = new Guid("00000000-0000-0000-0000-000000000002"),
                    FormatID         = 1,
                    DestinationID    = 1,
                    Filename         = "file.ext",
                    OriginalFilename = "orig.ext",
                    FolderPath       = "/"
                };
        }

        private Chaos.Mcm.Extension.File Make_FileExtension()
        {
            return (Chaos.Mcm.Extension.File)new Chaos.Mcm.Extension.File().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion

    }
}