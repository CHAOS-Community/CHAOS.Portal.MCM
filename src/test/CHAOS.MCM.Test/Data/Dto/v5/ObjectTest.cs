namespace Chaos.Mcm.Test.Data.Dto.v5
{
    using System;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using NUnit.Framework;
    using Object = Mcm.Data.Dto.Object;

    [TestFixture]
    public class ObjectTest : TestBase
    {
        [Test]
        public void Create_GivenObjectModel_ReturnV5VersionOfObject()
        {
            var obj = Make_ObjectModel();

            var result = Mcm.Data.Dto.v5.Object.Create(obj);

            var xml = SerializerFactory.XMLSerializer.Serialize(result);
            Assert.That(xml.ToString(SaveOptions.DisableFormatting), Is.EqualTo(@"<Chaos.Mcm.Data.Dto.v5.Object><GUID>00000010-0000-0000-0000-000000000001</GUID><ObjectTypeID>1</ObjectTypeID><DateCreated>01-01-2000 00:00:00</DateCreated><Metadatas /><ObjectRelations /><Files /><AccessPoints /><Fullname>CHAOS.MCM.Data.Dto.Standard.Object</Fullname></Chaos.Mcm.Data.Dto.v5.Object>"));
        }

        private Object Make_ObjectModel()
        {
            return new Object
                {
                    Guid = new Guid("10000000-0000-0000-0000-000000000001"),
                    ObjectTypeID = 1u,
                    DateCreated = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc)
                };
        }
    }
}