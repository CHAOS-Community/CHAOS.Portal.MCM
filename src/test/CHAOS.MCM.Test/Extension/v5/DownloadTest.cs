namespace Chaos.Mcm.Test.Extension.v5
{
    using System.IO;
    using System.Text;
    using Mcm.Extension.v5;
    using NUnit.Framework;

    [TestFixture]
    public class DownloadTest : TestBase
    {
        [Test]
        public void Get_FileWithS3Token_CallS3AdapterAndReturnAttachmentWithFileStream()
        {
            var extension = Make_DownloadExtension();
            var file = Make_File();
            var obj = Make_Object();
            var fileinfo = Make_FileInfo();
            var data = Make_StreamFromString("some data");
            obj.Files = new []{fileinfo};
            McmRepository.Setup(m => m.FileGet(file.Id)).Returns(file);
            McmRepository.Setup(m => m.ObjectGet(file.ObjectGuid, false, true, false, false, false)).Returns(obj);

            var result = extension.Get(file.Id);

            Assert.That(result.FileName, Is.EqualTo(fileinfo.OriginalFilename));
            Assert.That(result.ContentType, Is.EqualTo(fileinfo.MimeType));
            Assert.That(result.Stream, Is.EqualTo(data));
        }

        private Stream Make_StreamFromString(string someData)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(someData));
        }

        private Download Make_DownloadExtension()
        {
            return (Download) new Download(PortalApplication.Object, McmRepository.Object, "MCM").WithPortalRequest(PortalRequest.Object);
        }
    }
}