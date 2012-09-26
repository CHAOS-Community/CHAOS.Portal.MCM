using System;
using CHAOS.MCM.Data.DTO;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class FileTest : MCMTestBase
    {
		[Test]
		public void Create_File()
		{
		    File file = FileModule.Create(AdminCallContext, Object1.GUID, null, Format.ID, DestinationInfo.ID, "filename", "originalfilename", "/1/2/3/");
            
		    Assert.AreEqual( "filename", file.Filename );
		}

        [Test]
        public void Delete_File()
        {
            var result = FileModule.Delete(AdminCallContext, File.ID);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_Create_URL_In_FileInfo()
        {
            var file = new FileInfo( 1, System.Guid.NewGuid(), null, 1, "filename.ext", "originalname.ext", "/1/2/3/", DateTime.Now, "http://www.example.com", "{BASE_PATH}{FOLDER_PATH}{FILENAME}", DateTime.Now, "HTTP Download", 1, "formatname", "<xml />", "ext", 1, "formatcategoryname", 1, "formattypename", AdminSession.GUID );
            Assert.AreEqual("http://www.example.com/1/2/3/filename.ext", file.URL);

            file.BasePath = "C:\\some\\folder";
            Assert.AreEqual("C:\\some\\folder\\1\\2\\3\\filename.ext", file.URL);

            file.BasePath = "rtmp://streaming.server.com";
            Assert.AreEqual("rtmp://streaming.server.com/1/2/3/filename.ext", file.URL);

            file.BasePath     = "http://streaming.server.com";
            file.StringFormat = "{BASE_PATH}{FOLDER_PATH}{FILENAME}/Manifest";
            Assert.AreEqual( "http://streaming.server.com/1/2/3/filename.ext/Manifest", file.URL );
        }
    }
}
