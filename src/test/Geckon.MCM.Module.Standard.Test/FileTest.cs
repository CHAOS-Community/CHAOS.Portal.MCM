using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geckon.MCM.Data.Linq;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    [TestFixture]
    public class FileTest : BaseTest
    {
        [Test]
        public void Create_File()
        {
            File file = MCMModule.File_Create( AdminCallContext, Object1.GUID, null, Format.ID, Destination.ID, "filename", "originalfilename", "/1/2/3/" );

            Assert.AreEqual( "filename", file.Filename );
        }
    }
}
