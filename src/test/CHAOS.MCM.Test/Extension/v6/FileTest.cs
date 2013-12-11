using System;
using Chaos.Mcm.Data.Dto;
using NUnit.Framework;
using FolderPermission = Chaos.Mcm.Permission.FolderPermission;

namespace Chaos.Mcm.Test.Extension.v6
{
    using System.Collections.Generic;
    using Moq;

    [TestFixture]
    public class FileTest : TestBase
    {
        [Test]
        public void Set_UserHasPermission_CallMcmRepositoryToCreateAndRetrieveUsingId()
        {
            var extension = Make_FileExtension();
            var file      = Make_File();
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(), 
                                                                 It.IsAny<Guid>(), 
                                                                 It.IsAny<IEnumerable<Guid>>(), 
                                                                 It.IsAny<FolderPermission>())).Returns(true);
            McmRepository.Setup(m => m.FileCreate(file.ObjectGuid, file.ParentID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath, file.FormatID)).Returns(file.Id);
            McmRepository.Setup(m => m.FileGet(file.Id)).Returns(file);

            
            var result = extension.Create(file.ObjectGuid, file.ParentID, file.FormatID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath);

            Assert.AreEqual(file, result);
            McmRepository.Verify(m => m.FileCreate(file.ObjectGuid, file.ParentID, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath, file.FormatID));
            McmRepository.Verify(m => m.FileGet(file.Id));
        }

        [Test]
        public void Delete_UserHasPermission_CallMcmRepositoryReturnsOne()
        {
            var extension = Make_FileExtension();
            var file      = Make_File();
            PermissionManager.Setup(m => m.HasPermissionToObject(It.IsAny<Guid>(),
                                                                 It.IsAny<Guid>(),
                                                                 It.IsAny<IEnumerable<Guid>>(),
                                                                 It.IsAny<FolderPermission>())).Returns(true);
            McmRepository.Setup(m => m.FileDelete(file.Id)).Returns(1u);
            McmRepository.Setup(m => m.FileGet(file.Id)).Returns(file);

            var result = extension.Delete(file.Id);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.FileDelete(file.Id));
            McmRepository.Verify(m => m.FileGet(file.Id));
        }

        #region Helpers

        private static File Make_File()
        {
            return new File
                {
                    Id               = 1,
                    ObjectGuid       = new Guid("00000000-0000-0000-0000-000000000002"),
                    FormatID         = 1,
                    DestinationID    = 1,
                    Filename         = "file.ext",
                    OriginalFilename = "orig.ext",
                    FolderPath       = "/"
                };
        }

        #endregion

    }
}