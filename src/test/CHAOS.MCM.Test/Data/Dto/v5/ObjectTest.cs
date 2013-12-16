namespace Chaos.Mcm.Test.Data.Dto.v5
{
    using System;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using Mcm.Data.Dto;
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
            Assert.That(xml.ToString(SaveOptions.DisableFormatting), Is.EqualTo("<Result FullName=\"CHAOS.MCM.Data.DTO.Object\"><GUID>00000010-0000-0000-0000-000000000001</GUID><ObjectTypeID>1</ObjectTypeID><DateCreated>01-01-2000 00:00:00</DateCreated>" +
                                                                                "<Metadatas><Metadata><GUID>00000020-0000-0000-0000-000000000002</GUID><EditingUserGUID>00000030-0000-0000-0000-000000000003</EditingUserGUID><LanguageCode>en</LanguageCode><MetadataSchemaGUID>00000040-0000-0000-0000-000000000004</MetadataSchemaGUID><RevisionID>0</RevisionID><MetadataXML><![CDATA[<xml>some metadata</xml>]]></MetadataXML><DateCreated>01-01-2000 00:00:00</DateCreated></Metadata></Metadatas>" +
                                                                                "<ObjectRelations /><Files /><AccessPoints /></Result>"));
        }

        private static Object Make_ObjectModel()
        {
            return new Object
                {
                    Guid = new Guid("10000000-0000-0000-0000-000000000001"),
                    ObjectTypeID = 1u,
                    DateCreated = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc),
                    Metadatas = new[]
                    {
                        new Metadata
                        {
                            Guid  = new Guid("20000000-0000-0000-0000-000000000002"),
                            DateCreated = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc),
                            EditingUserGuid = new Guid("30000000-0000-0000-0000-000000000003"),
                            MetadataSchemaGuid = new Guid("40000000-0000-0000-0000-000000000004"),
                            LanguageCode = "en",
                            RevisionID = 0u,
                            MetadataXml = XDocument.Parse("<xml>some metadata</xml>")

                        }
                    }
                };
        }
    }
}