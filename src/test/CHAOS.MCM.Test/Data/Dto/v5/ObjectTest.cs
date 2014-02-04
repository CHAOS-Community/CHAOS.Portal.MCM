namespace Chaos.Mcm.Test.Data.Dto.v5
{
    using System;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using Mcm.Data.Dto;
    using Mcm.View;
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
                                                                                "<Metadatas><Result FullName=\"CHAOS.MCM.Data.DTO.Metadata\"><GUID>00000020-0000-0000-0000-000000000002</GUID><EditingUserGUID>00000030-0000-0000-0000-000000000003</EditingUserGUID><LanguageCode>en</LanguageCode><MetadataSchemaGUID>00000040-0000-0000-0000-000000000004</MetadataSchemaGUID><RevisionID>0</RevisionID><MetadataXML><![CDATA[<xml>some metadata</xml>]]></MetadataXML><DateCreated>01-01-2000 00:00:00</DateCreated></Result></Metadatas>" +
                                                                                "<ObjectRelations><Result FullName=\"CHAOS.MCM.Data.Dto.Standard.Object_Object_Join\"><Object1GUID>00000050-0000-0000-0000-000000000005</Object1GUID><Object2GUID>00000060-0000-0000-0000-000000000006</Object2GUID><ObjectRelationTypeID>1</ObjectRelationTypeID><Sequence>1</Sequence></Result></ObjectRelations>" +
                                                                                "<Files><Result FullName=\"CHAOS.MCM.Data.DTO.FileInfo\"><ID>1</ID><Filename>file.ext</Filename><OriginalFilename>Some original filename</OriginalFilename><Token>Some token</Token><URL>http://some.url/file.ext</URL><FormatID>1</FormatID><Format>Some format</Format><FormatCategory>Some format category</FormatCategory><FormatType>Some format type</FormatType></Result></Files>" +
                                                                                "<AccessPoints><AccessPoint_Object_Join FullName=\"Chaos.Mcm.Data.Dto.v5.ObjectAccessPoint\"><AccessPointGUID>00000070-0000-0000-0000-000000000007</AccessPointGUID><ObjectGUID>00000080-0000-0000-0000-000000000008</ObjectGUID><StartDate>01-01-2000 00:00:00</StartDate><EndDate>01-01-2001 00:00:00</EndDate><DateCreated>01-01-2000 00:00:00</DateCreated><DateModified>01-01-2000 00:00:00</DateModified></AccessPoint_Object_Join></AccessPoints></Result>"));
        }

        private ObjectViewData Make_ObjectViewData()
        {
            var model = Make_ObjectModel();

            return new ObjectViewData(model);
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
                },
                Files = new []
                {
                    new FileInfo
                    {
                        Id = 1,
                        FormatID = 1,
                        Filename = "file.ext",
                        OriginalFilename = "Some original filename",
                        FolderPath = "/",
                        Token = "Some token",
                        Format = "Some format",
                        FormatCategory = "Some format category",
                        FormatType = "Some format type",
                        StringFormat = "{BASE_PATH}{FOLDER_PATH}{FILENAME}",
                        BasePath = "http://some.url"
                    }
                },
                ObjectRelationInfos = new []
                {
                    new ObjectRelationInfo 
                    {
                        Object1Guid          = new Guid("50000000-0000-0000-0000-000000000005"),
                        Object2Guid          = new Guid("60000000-0000-0000-0000-000000000006"),
                        Sequence             = 1,
                        ObjectRelationTypeID = 1
                    }
                },
                AccessPoints = new []
                {
                    new ObjectAccessPoint
                    {
                        AccessPointGuid = new Guid("70000000-0000-0000-0000-000000000007"),
                        ObjectGuid = new Guid("80000000-0000-0000-0000-000000000008"),
                        StartDate = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc),
                        EndDate = new DateTime(2001,01,01, 00,00,00, DateTimeKind.Utc),
                        DateCreated = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc),
                        DateModified = new DateTime(2000,01,01, 00,00,00, DateTimeKind.Utc),
                    }
                }
            };
        }
    }
}