namespace Chaos.Mcm.Test.View
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Permission.InMemory;
    using Chaos.Mcm.View;

    using Moq;

    using NUnit.Framework;

    using Newtonsoft.Json;

    using FileInfo = Chaos.Mcm.Data.Dto.FileInfo;

    [TestFixture]
    public class ObjectIndexTest : Test.TestBase
    {
        [SetUp]
        public void SetUp()
        {
            var folder = new Folder();
            PermissionManager.Setup(m => m.GetFolders(It.IsAny<uint>())).Returns(folder);
        }

        [Test]
        public void Index_GivenObject_ReturnObjectViewData()
        {
            var view = Make_ObjectView();
            var obj  = Make_Object();

            var result = (ObjectViewData) view.Index(obj).First();

            Assert.That(result.Id, Is.EqualTo(obj.Guid.ToString()));
            Assert.That(result.ObjectTypeId, Is.EqualTo(obj.ObjectTypeID));
            Assert.That(result.DateCreated, Is.EqualTo(obj.DateCreated));
            Assert.That(result.AccessPoints, Is.SameAs(obj.AccessPoints));
//            Assert.That(result.Files, Is.SameAs(obj.Files));
            Assert.That(result.Metadatas, Is.SameAs(obj.Metadatas));
            Assert.That(result.ObjectFolders, Is.SameAs(obj.ObjectFolders));
//            Assert.That(result.ObjectRelationInfos, Is.SameAs(obj.ObjectRelationInfos));
        }

        private ObjectView Make_ObjectView()
        {
            return new ObjectView(PermissionManager.Object);
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithAccessPoints_ReturnPublishStartAndEndDates()
        {
            var obj = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubStart")), Is.True);
            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubEnd")), Is.True);
        }

        [Test]
        public void GetIndexableFields_GivenObject_ReturnObjectTypeId()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "ObjectTypeId").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithFiles_ReturnFileCount()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "Files.Count").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithObjectRelations_ReturnRelationCount()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "ObjectRelations.Count").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithFolders_ReturnFolders()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.Any(item => item.Key.Contains("FolderId")), Is.True);
            Assert.That(results.Any(item => item.Key.Contains("FolderAncestors")), Is.True);
        }

        [Test]
        public void UnitUnderTest_Scenario_ExpectedResult()
        {
            var json =
"{"+
  "\"uniqueIdentifier\": {"+
    "\"Key\": \"Id\","+
    "\"Value\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\""+
  "},"+
  "\"id\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
  "\"objectTypeId\": 89,"+
  "\"dateCreated\": \"2013-05-23T12:56:57\","+
  "\"metadatas\": ["+
    "{"+
      "\"objectGuid\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
      "\"guid\": \"eecc051a-5917-41b6-a784-8f94bdefb406\","+
      "\"languageCode\": \"en\","+
      "\"metadataSchemaGuid\": \"ba50fe63-9cca-a445-97fd-7ff9ed5de022\","+
      "\"editingUserGuid\": \"31a94cce-0b10-8645-8ed8-94b97128c17e\","+
      "\"revisionID\": 1,"+
      "\"metadataXml\": {"+
        "\"File\": {"+
          "\"Title\": {"+
            "\"#cdata-section\": \"Synkronriller - ingen gentagelse\""+
          "},"+
          "\"Description\": {"+
            "\"#cdata-section\": \"Ole Fugl Hørkilde interviewer lydteknikeren Allan Mikkelsen om synkronriller, der tilbage i 1930'erne og 40'erne, blev brugt til at binde længere radioudsendelser sammen. Udsendelserne blev dengang optaget på og afspillet fra voks- og lakplader, hvorpå der kun kunne være få minutter pr. pladeside. Allan viser, hvordan han retter de overleveringsfejl, der har sneget sig ind, når en gammel radioudsendelse er blevet kopieret fra plade til spolebånd. Han sætter en ære i at få den nye digitale kopi til at fremtræde præcis som den oprindelige udsendelse blev sendt tilbage i 30’erne og 40’erne. I afsnittet citeres der fra DR-udsendelserne: Kong Frederik IX udråbes til konge (DR P1, 21/4-1947, kl. 12.00); Fra mikrofon til højtaler (DR P1, 10/1-1942, kl. 16.30)\""+
          "},"+
          "\"FilesizeKB\": {"+
            "\"#cdata-section\": \"8323\""+
          "}"+
        "}"+
      "},"+
      "\"dateCreated\": \"2013-05-23T12:57:27\","+
      "\"fullname\": \"Chaos.Mcm.Data.Dto.ObjectMetadata\""+
    "}"+
  "],"+
  "\"objectFolders\": ["+
    "{"+
      "\"iD\": 717,"+
      "\"parentID\": 486,"+
      "\"folderTypeID\": 1,"+
      "\"objectGuid\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
      "\"objectFolderTypeID\": 1,"+
      "\"subscriptionGuid\": null,"+
      "\"name\": \"Annotationer\","+
      "\"dateCreated\": \"2012-07-03T11:14:12\","+
      "\"fullname\": \"Chaos.Mcm.Data.Dto.ObjectFolder\""+
    "}"+
  "],"+
  "\"objectRelationInfos\": ["+
    "{"+
      "\"object1Guid\": \"941d8f7e-e548-3a44-a75f-ae7f1cddedd3\","+
      "\"object2Guid\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
      "\"objectRelationTypeID\": 16,"+
      "\"object1TypeID\": 24,"+
      "\"object2TypeID\": 89,"+
      "\"objectRelationType\": \"Part of\","+
      "\"metadataGuid\": null,"+
      "\"languageCode\": null,"+
      "\"metadataSchemaGuid\": null,"+
      "\"metadataXml\": null,"+
      "\"sequence\": 0,"+
      "\"fullname\": \"Chaos.Mcm.Data.Dto.ObjectRelationInfo\""+
    "}"+
  "],"+
  "\"files\": ["+
    "{"+
      "\"iD\": 4039613,"+
      "\"parentID\": null,"+
      "\"objectGuid\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
      "\"filename\": \"a55bf857-1049-614d-90a6-e6fcaa102914\","+
      "\"originalFilename\": \"#2 Synkronriller - Ingen gentagelser, tak! v3.mp3\","+
      "\"token\": \"HTTP Download\","+
      "\"uRL\": \"http://api.chaos-systems.com/v5/Download/Get?fileId={FILE_ID}&sessionGUID={SESSION_GUID_MISSING}\","+
      "\"formatID\": 55,"+
      "\"format\": null,"+
      "\"formatCategory\": \"Source Video\","+
      "\"formatType\": \"Video\","+
      "\"destinationID\": 16,"+
      "\"folderPath\": \"/2013/05/23/\","+
      "\"fileDateCreated\": \"2013-05-23T12:57:22\","+
      "\"basePath\": \"http://api.chaos-systems.com/v5/Download/Get\","+
      "\"accessProviderDateCreated\": \"2012-09-19T21:54:55\","+
      "\"formatXML\": null,"+
      "\"mimeType\": \"application/octet-stream\","+
      "\"formatCategoryID\": 50,"+
      "\"stringFormat\": \"{BASE_PATH}?fileId={FILE_ID}&sessionGUID={SESSION_GUID}\","+
      "\"formatTypeID\": 1,"+
      "\"formatTypeName\": \"Unknown Video\","+
      "\"sessionGUID\": null,"+
      "\"fullname\": \"Chaos.Mcm.Data.Dto.FileInfo\""+
    "},"+
    "{"+
      "\"iD\": 4039613,"+
      "\"parentID\": null,"+
      "\"objectGuid\": \"bf360100-4cd6-4221-a0d2-7abe1b6b6f04\","+
      "\"filename\": \"a55bf857-1049-614d-90a6-e6fcaa102914\","+
      "\"originalFilename\": \"#2 Synkronriller - Ingen gentagelser, tak! v3.mp3\","+
      "\"token\": \"S3\","+
      "\"uRL\": \"bucketname=chaosdata;key=/2013/05/23/a55bf857-1049-614d-90a6-e6fcaa102914\","+
      "\"formatID\": 55,"+
      "\"format\": null,"+
      "\"formatCategory\": \"Source Video\","+
      "\"formatType\": \"Video\","+
      "\"destinationID\": 16,"+
      "\"folderPath\": \"/2013/05/23/\","+
      "\"fileDateCreated\": \"2013-05-23T12:57:22\","+
      "\"basePath\": \"chaosdata\","+
      "\"accessProviderDateCreated\": \"2013-04-25T17:45:00\","+
      "\"formatXML\": null,"+
      "\"mimeType\": \"application/octet-stream\","+
      "\"formatCategoryID\": 50,"+
      "\"stringFormat\": \"bucketname={BASE_PATH};key={FOLDER_PATH}{FILENAME}\","+
      "\"formatTypeID\": 1,"+
      "\"formatTypeName\": \"Unknown Video\","+
      "\"sessionGUID\": null,"+
      "\"fullname\": \"Chaos.Mcm.Data.Dto.FileInfo\""+
    "}"+
  "],"+
  "\"accessPoints\": [],"+
  "\"fullname\": \"Chaos.Mcm.View.ObjectViewData\"" +
"}";

            var obj = JsonConvert.DeserializeObject<ObjectViewData>(json);

            Assert.That(obj, Is.Not.Null);
        }
        
        private static Object Make_Object()
        {
            return new Object{
                    ObjectTypeID = 1,
                    AccessPoints = new []
                        {
                            new ObjectAccessPoint
                                {
                                    AccessPointGuid = new System.Guid("00000000-0000-0000-0000-000000000001")
                                }
                        },
                        ObjectFolders = new []
                            {
                                new ObjectFolder
                                    {
                                        ID = 10
                                    }
                            },
                            Files = new List<FileInfo>()
                                {
                                    new FileInfo()
                                },
                                ObjectRealtionInfos = new List<ObjectRelationInfo>()
                                    {
                                        new ObjectRelationInfo()
                                    }
                };
        }
    }
}