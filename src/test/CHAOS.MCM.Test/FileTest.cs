using CHAOS.MCM.Data.DTO;
using CHAOS.MCM.Module;
using NUnit.Framework;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class FileTest : MCMTestBase
    {
		[Test]
		public void Create_File()
		{
		    File file = MCMModule.File_Create(AdminCallContext, Object1.GUID, null, Format.ID, DestinationInfo.ID, "filename", "originalfilename", "/1/2/3/");

		    Assert.AreEqual( "filename", file.Filename );
		}
    }
}
