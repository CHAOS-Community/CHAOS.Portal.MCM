namespace Chaos.Mcm.Test.Extension.v5
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Mcm.Extension.v5.Download;
    using NUnit.Framework;
    using Portal.Core.Data.Model;

    [TestFixture]
    public class DownloadTest : TestBase
    {
        [Test]
        public void Get_FileHasTokenThatSupportDownload_CallS3AdapterAndReturnAttachmentWithFileStream()
        {
            var extension = Make_DownloadExtension();
            var file = Make_File();
            var obj = Make_Object();
            var fileinfo = Make_FileInfo();
            var data = Make_StreamFromString("some data");
            fileinfo.Token = "Test";
            obj.Files = new []{fileinfo};
            McmRepository.Setup(m => m.FileGet(file.Id)).Returns(file);
            McmRepository.Setup(m => m.ObjectGet(file.ObjectGuid, false, true, false, false, false)).Returns(obj);
            DownloadStrategy.Setup(m => m.GetStream(fileinfo)).Returns(new Attachment
                                                                        {
                                                                            ContentType = fileinfo.MimeType,
                                                                            FileName = fileinfo.OriginalFilename,
                                                                            Stream = data
                                                                        });

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
            IDictionary<string, IDownloadStrategy> downloadStrategies = new Dictionary<string, IDownloadStrategy>();
            downloadStrategies.Add("Test", DownloadStrategy.Object);

            return (Download)Download.CreateWithDownloadStrategy(PortalApplication.Object, McmRepository.Object, Make_McmModuleConfiguration(), downloadStrategies).WithPortalRequest(PortalRequest.Object);
        }

        
    }
}