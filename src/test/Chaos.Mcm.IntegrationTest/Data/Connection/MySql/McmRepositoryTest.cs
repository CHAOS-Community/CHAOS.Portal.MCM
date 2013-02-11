namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Data.Dto.Standard;

    using NUnit.Framework;

    using Object = Chaos.Mcm.Data.Dto.Object;

    [TestFixture]
    public class McmRepositoryTest : TestBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;

        #region ObjectRelationInfo

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatDoesntExist_ReturnsAEmptyList()
        {
            var connection            = Make_McmRepository();
            var nonExistantObjectGuid = new Guid("77777777-7777-7777-7777-777777777777");

            var result = connection.ObjectRelationInfoGet(nonExistantObjectGuid);

            Assert.IsEmpty(result);
        }

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatExist_ReturnsAListWithOneObjectRelationInfo()
        {
            var connection = Make_McmRepository();
            var expected   = Make_ObjectRelationInfo();

            var result = connection.ObjectRelationInfoGet(expected.Object1Guid);
            var actual = result.First();

            Assert.IsNotEmpty(result);
            Assert.AreEqual(expected.Object1Guid, actual.Object1Guid);
            Assert.AreEqual(expected.Object2Guid, actual.Object2Guid);
            Assert.AreEqual(expected.MetadataGuid, actual.MetadataGuid);
            Assert.AreEqual(expected.Sequence, actual.Sequence);
            Assert.AreEqual(expected.ObjectRelationType, actual.ObjectRelationType);
            Assert.AreEqual(expected.ObjectRelationTypeID, actual.ObjectRelationTypeID);
            Assert.AreEqual(expected.LanguageCode, actual.LanguageCode);
            Assert.AreEqual(expected.MetadataSchemaGuid, actual.MetadataSchemaGuid);
        }

        [Test]
        public void ObjectRelationCreate_ExclutingMetadata_CreateInDatabaseAndReturnOne()
        {
            var connection             = Make_McmRepository();
            var repo                   = Make_McmRepository();
            var expectedObjectRelation = Make_ObjectRelation();

            var result = repo.ObjectRelationSet(expectedObjectRelation.Object1Guid, expectedObjectRelation.Object2Guid, expectedObjectRelation.ObjectRelationTypeID, expectedObjectRelation.Sequence);

            Assert.AreEqual(result, 1);
            var resultObjectRealtionInfo = connection.ObjectRelationInfoGet(expectedObjectRelation.Object1Guid);
            Assert.IsNotEmpty(resultObjectRealtionInfo);
            Assert.AreEqual(expectedObjectRelation.Object1Guid, resultObjectRealtionInfo.First().Object1Guid);
            Assert.AreEqual(expectedObjectRelation.Object2Guid, resultObjectRealtionInfo.First().Object2Guid);
            Assert.AreEqual(expectedObjectRelation.MetadataGuid, resultObjectRealtionInfo.First().MetadataGuid);
            Assert.AreEqual(expectedObjectRelation.Sequence, resultObjectRealtionInfo.First().Sequence);
        }

        [Test]
        public void ObjectRelationCreate_IncludingMetadata_CreateRelationAndMetadataInDatabaseAndReturnOne()
        {
            var connection             = Make_McmRepository();
            var repo                   = Make_McmRepository();
            var expectedObjectRelation = Make_ObjectRelationInfoNonExistent();
            var someUserGuid           = new Guid("00000000-0000-0000-0000-000000001000");

            var result = repo.ObjectRelationSet(expectedObjectRelation, someUserGuid);

            Assert.AreEqual(result, 1);
            var resultObjectRealtionInfo = connection.ObjectRelationInfoGet(expectedObjectRelation.Object1Guid);
            Assert.IsNotEmpty(resultObjectRealtionInfo);
            Assert.AreEqual(expectedObjectRelation.Object1Guid, resultObjectRealtionInfo.First().Object1Guid);
            Assert.AreEqual(expectedObjectRelation.Object2Guid, resultObjectRealtionInfo.First().Object2Guid);
            Assert.AreEqual(expectedObjectRelation.MetadataGuid, resultObjectRealtionInfo.First().MetadataGuid);
            Assert.AreEqual(expectedObjectRelation.MetadataSchemaGuid, resultObjectRealtionInfo.First().MetadataSchemaGuid);
            Assert.AreEqual(expectedObjectRelation.MetadataXml.Root.Value, resultObjectRealtionInfo.First().MetadataXml.Root.Value);
            Assert.AreEqual(expectedObjectRelation.Sequence, resultObjectRealtionInfo.First().Sequence);
        }

        #endregion
        #region Metadata

        [Test]
        public void MetadataGet_GivenAGuidThatExist_ReturnsTheRequestedMetadata()
        {
            var repository       = Make_McmRepository();
            var existingMetadata = Make_Metadata();

            var result = repository.MetadataGet(existingMetadata.Guid);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(existingMetadata.Guid, result.First().Guid,"Guid");
            Assert.AreEqual(existingMetadata.MetadataSchemaGuid, result.First().MetadataSchemaGuid, "MetadataSchemaGuid");
            Assert.AreEqual(existingMetadata.EditingUserGuid, result.First().EditingUserGuid, "EditingUserGuid");
            Assert.AreEqual(existingMetadata.RevisionID, result.First().RevisionID, "RevisionID");
            Assert.AreEqual(existingMetadata.LanguageCode, result.First().LanguageCode, "LanguageCode");
            Assert.AreEqual(existingMetadata.MetadataXml.Root.Value, result.First().MetadataXml.Root.Value, "MetadataXml");
            Assert.AreEqual(existingMetadata.DateCreated, result.First().DateCreated, "DateCreated");
        }

        [Test]
        public void MetadataSet_CalledForTheFirstTime_ShouldCreateTheMetadataInTheDatabase()
        {
            var repository           = Make_McmRepository();
            var nonexistentMetadata  = Make_Metadata();
            var existingObjectGuid   = new Guid("00000000-0000-0000-0000-000000000003");
            var someUserGuid         = new Guid("00000010-0000-0000-0000-000000000000");
            nonexistentMetadata.Guid = new Guid("00000000-0000-0000-0000-000000000030");

            var result = repository.MetadataSet(existingObjectGuid, nonexistentMetadata.Guid, nonexistentMetadata.MetadataSchemaGuid, nonexistentMetadata.LanguageCode, nonexistentMetadata.RevisionID, nonexistentMetadata.MetadataXml, someUserGuid);

            Assert.AreEqual(1, result);
            var rowSavedInDB = repository.MetadataGet(nonexistentMetadata.Guid).First();
            Assert.AreEqual(nonexistentMetadata.Guid, rowSavedInDB.Guid, "Guid");
            Assert.AreEqual(nonexistentMetadata.MetadataSchemaGuid, rowSavedInDB.MetadataSchemaGuid, "MetadataSchemaGuid");
            Assert.AreEqual(someUserGuid, rowSavedInDB.EditingUserGuid, "EditingUserGuid");
            Assert.AreEqual(nonexistentMetadata.RevisionID, rowSavedInDB.RevisionID, "RevisionID");
            Assert.AreEqual(nonexistentMetadata.LanguageCode, rowSavedInDB.LanguageCode, "LanguageCode");
            Assert.AreEqual(nonexistentMetadata.MetadataXml.Root.Value, rowSavedInDB.MetadataXml.Root.Value, "MetadataXml");
        }
        
        #endregion
        #region Object

        [Test]
        public void ObjectCrate_WithValidObjectTypeAndFolder_ReturnOneAndObjectShouldBeCreatedInTheDatabase()
        {
            var repository     = Make_McmRepository();
            var objToCreate    = Make_ObjectTheDoesntExist();
            var existingFolder = Make_FolderTheExist();

            var result = repository.ObjectCreate(objToCreate.Guid, objToCreate.ObjectTypeID, existingFolder.ID);

            Assert.AreEqual(1, result);
            var actual = repository.ObjectGet(objToCreate.Guid);
            Assert.AreEqual(objToCreate.Guid, actual.Guid);
            Assert.AreEqual(objToCreate.ObjectTypeID, actual.ObjectTypeID);
        }

        [Test]
        public void ObjectDelete_ByGuidNoRelationsToOtherTables_ReturnsOneAndObjectShouldBeMissingFromDatabaseAfterwards()
        {
            var repository  = Make_McmRepository();
            var objToDelete = Make_ObjectWithNoRelations();

            var result = repository.ObjectDelete(objToDelete.Guid);

            Assert.AreEqual(1, result);
            var shouldBeNull = repository.ObjectGet(objToDelete.Guid);
            Assert.IsNull(shouldBeNull);
        }

        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeMetadata_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository         = Make_McmRepository();
            var objectGuid         = new Guid("00000000-0000-0000-0000-000000000002");
            var metadataGuid       = new Guid("00000000-0000-0000-0000-000000000050");
            var metadataSchemaGuid = new Guid("00000000-0000-0000-0000-000000000100");
            var languageCode       = "en";
            var metadataXml        = "<xml>test xml</xml>";
            
            var result = repository.ObjectGet(objectGuid, true);

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(metadataGuid, result.Metadatas[0].Guid);
            Assert.AreEqual(metadataSchemaGuid, result.Metadatas[0].MetadataSchemaGuid);
            Assert.AreEqual(languageCode, result.Metadatas[0].LanguageCode);
            Assert.AreEqual(metadataXml, result.Metadatas[0].MetadataXml.ToString());
        }

        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeFiles_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository   = this.Make_McmRepository();
            var objectGuid   = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedFile = Make_File();

            var result = repository.ObjectGet(objectGuid, false, true);
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFile.ID, result.Files[0].ID);
            Assert.AreEqual(expectedFile.ObjectGUID, result.Files[0].ObjectGUID);
            Assert.AreEqual(expectedFile.FormatID, result.Files[0].FormatID);
            Assert.AreEqual(expectedFile.DestinationID, result.Files[0].DestinationID);
            Assert.AreEqual(expectedFile.Filename, result.Files[0].Filename);
            Assert.AreEqual(expectedFile.OriginalFilename, result.Files[0].OriginalFilename);
            Assert.AreEqual(expectedFile.FolderPath, result.Files[0].FolderPath);
            Assert.AreEqual(expectedFile.BasePath, result.Files[0].BasePath);
            Assert.AreEqual(expectedFile.StringFormat, result.Files[0].StringFormat);
            Assert.AreEqual(expectedFile.Token, result.Files[0].Token);
            Assert.AreEqual(expectedFile.FormatID, result.Files[0].FormatID);
            Assert.AreEqual(expectedFile.FormatTypeName, result.Files[0].FormatTypeName);
            Assert.AreEqual(expectedFile.FormatXML, result.Files[0].FormatXML);
            Assert.AreEqual(expectedFile.MimeType, result.Files[0].MimeType);
            Assert.AreEqual(expectedFile.FormatCategoryID, result.Files[0].FormatCategoryID);
            Assert.AreEqual(expectedFile.FormatCategory, result.Files[0].FormatCategory);
            Assert.AreEqual(expectedFile.FormatTypeID, result.Files[0].FormatTypeID);
            Assert.AreEqual(expectedFile.FormatType, result.Files[0].FormatType);
        }
        
        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeObjectRelations_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository                 = Make_McmRepository();
            var objectGuid                 = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedObjectRelationInfo = Make_ObjectRelationInfo();

            var result = repository.ObjectGet(objectGuid, false, false, true);
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual( expectedObjectRelationInfo.Object1Guid, result.ObjectRelationInfos[0].Object1Guid );
            Assert.AreEqual( expectedObjectRelationInfo.Object2Guid, result.ObjectRelationInfos[0].Object2Guid );
            Assert.AreEqual( expectedObjectRelationInfo.ObjectRelationTypeID, result.ObjectRelationInfos[0].ObjectRelationTypeID );
            Assert.AreEqual( expectedObjectRelationInfo.ObjectRelationType, result.ObjectRelationInfos[0].ObjectRelationType );
            Assert.AreEqual( expectedObjectRelationInfo.MetadataGuid, result.ObjectRelationInfos[0].MetadataGuid );
            Assert.AreEqual( expectedObjectRelationInfo.MetadataSchemaGuid, result.ObjectRelationInfos[0].MetadataSchemaGuid );
            Assert.AreEqual( expectedObjectRelationInfo.MetadataXml.ToString(), result.ObjectRelationInfos[0].MetadataXml.ToString() );
        }

        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeFolders_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository         = Make_McmRepository();
            var objectGuid         = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedFolderInfo = this.Make_FolderTheExist();

            var result = repository.ObjectGet(objectGuid, false, false, false, true);
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFolderInfo.ID, result.ObjectFolders[0].ID);
            Assert.AreEqual(expectedFolderInfo.ParentID, result.ObjectFolders[0].ParentID);
            Assert.AreEqual(expectedFolderInfo.FolderTypeID, result.ObjectFolders[0].FolderTypeID);
            Assert.AreEqual(expectedFolderInfo.Name, result.ObjectFolders[0].Name);
            Assert.AreEqual(expectedFolderInfo.SubscriptionGUID, result.ObjectFolders[0].SubscriptionGUID, "SubscriptionGuid");
        }
        
        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeAccessPoints_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository          = Make_McmRepository();
            var objectGuid          = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedAccessPoint = Make_AccessPoint();

            var result = repository.ObjectGet(objectGuid, false, false, false, false, true);

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedAccessPoint.AccessPointGuid, result.AccessPoints[0].AccessPointGuid);
            Assert.AreEqual(expectedAccessPoint.ObjectGuid, result.AccessPoints[0].ObjectGuid);
            Assert.AreEqual(expectedAccessPoint.StartDate, result.AccessPoints[0].StartDate);
            Assert.AreEqual(expectedAccessPoint.EndDate, result.AccessPoints[0].EndDate);
            Assert.AreEqual(expectedAccessPoint.DateCreated, result.AccessPoints[0].DateCreated);
            Assert.AreEqual(expectedAccessPoint.DateModified, result.AccessPoints[0].DateModified);
        }

        [Test]
        public void ObjectGet_ByObjectGuidsIncludingMetadata_TwoObjectDtoCreatedFromMultipleDataResults()
        {
            var repository         = Make_McmRepository();
            var objectGuids        = new[]{ new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000004")};

            var result = repository.ObjectGet(objectGuids, true);

            Assert.AreEqual(2, result.Count, "expecting two results");
            Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000050"), result[0].Metadatas[0].Guid);
            Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000100"), result[0].Metadatas[0].MetadataSchemaGuid);
            Assert.AreEqual("en", result[0].Metadatas[0].LanguageCode);
            Assert.AreEqual("<xml>test xml</xml>", result[0].Metadatas[0].MetadataXml.ToString());
            Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000060"), result[1].Metadatas[0].Guid);
            Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000100"), result[1].Metadatas[0].MetadataSchemaGuid);
            Assert.AreEqual("en", result[1].Metadatas[0].LanguageCode);
            Assert.AreEqual("<xml>test xml 2</xml>", result[1].Metadatas[0].MetadataXml.ToString());
        }

        [Test]
        public void ObjectGet_ByFolderID_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository = Make_McmRepository();
            var objectGuid = new Guid("00000000-0000-0000-0000-000000000002");
            var folderID   = (uint?) 1;

            var result = repository.ObjectGet(folderID).First();

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
        }
        
        [Test]
        public void ObjectGet_ByFolderIDAndMetadata_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository         = Make_McmRepository();
            var objectGuid         = new Guid("00000000-0000-0000-0000-000000000002");
            var folderID           = (uint?) 1;
            var metadataGuid       = new Guid("00000000-0000-0000-0000-000000000050");
            var metadataSchemaGuid = new Guid("00000000-0000-0000-0000-000000000100");
            var languageCode       = "en";
            var metadataXml        = "<xml>test xml</xml>";

            var result = repository.ObjectGet(folderID, includeMetadata: true ).First();

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(metadataGuid, result.Metadatas[0].Guid);
            Assert.AreEqual(metadataSchemaGuid, result.Metadatas[0].MetadataSchemaGuid);
            Assert.AreEqual(languageCode, result.Metadatas[0].LanguageCode);
            Assert.AreEqual(metadataXml, result.Metadatas[0].MetadataXml.ToString());
        }

        [Test]
        public void ObjectGet_ByFolderIDAndIncludeFiles_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository   = this.Make_McmRepository();
            var objectGuid   = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedFile = Make_File();
            var folderID     = (uint?)1;

            var result = repository.ObjectGet(folderID, includeFiles: true).First();

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFile.ID, result.Files[0].ID);
            Assert.AreEqual(expectedFile.ObjectGUID, result.Files[0].ObjectGUID);
            Assert.AreEqual(expectedFile.FormatID, result.Files[0].FormatID);
            Assert.AreEqual(expectedFile.DestinationID, result.Files[0].DestinationID);
            Assert.AreEqual(expectedFile.Filename, result.Files[0].Filename);
            Assert.AreEqual(expectedFile.OriginalFilename, result.Files[0].OriginalFilename);
            Assert.AreEqual(expectedFile.FolderPath, result.Files[0].FolderPath);
            Assert.AreEqual(expectedFile.BasePath, result.Files[0].BasePath);
            Assert.AreEqual(expectedFile.StringFormat, result.Files[0].StringFormat);
            Assert.AreEqual(expectedFile.Token, result.Files[0].Token);
            Assert.AreEqual(expectedFile.FormatID, result.Files[0].FormatID);
            Assert.AreEqual(expectedFile.FormatTypeName, result.Files[0].FormatTypeName);
            Assert.AreEqual(expectedFile.FormatXML, result.Files[0].FormatXML);
            Assert.AreEqual(expectedFile.MimeType, result.Files[0].MimeType);
            Assert.AreEqual(expectedFile.FormatCategoryID, result.Files[0].FormatCategoryID);
            Assert.AreEqual(expectedFile.FormatCategory, result.Files[0].FormatCategory);
            Assert.AreEqual(expectedFile.FormatTypeID, result.Files[0].FormatTypeID);
            Assert.AreEqual(expectedFile.FormatType, result.Files[0].FormatType);
        }

        [Test]
        public void ObjectGet_ByFolderIDAndIncludeObjectRelations_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository = Make_McmRepository();
            var objectGuid = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedObjectRelationInfo = Make_ObjectRelationInfo();
            var folderID = (uint?)1;

            var result = repository.ObjectGet(folderID, includeObjectRelations: true).First();

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedObjectRelationInfo.Object1Guid, result.ObjectRelationInfos[0].Object1Guid);
            Assert.AreEqual(expectedObjectRelationInfo.Object2Guid, result.ObjectRelationInfos[0].Object2Guid);
            Assert.AreEqual(expectedObjectRelationInfo.ObjectRelationTypeID, result.ObjectRelationInfos[0].ObjectRelationTypeID);
            Assert.AreEqual(expectedObjectRelationInfo.ObjectRelationType, result.ObjectRelationInfos[0].ObjectRelationType);
            Assert.AreEqual(expectedObjectRelationInfo.MetadataGuid, result.ObjectRelationInfos[0].MetadataGuid);
            Assert.AreEqual(expectedObjectRelationInfo.MetadataSchemaGuid, result.ObjectRelationInfos[0].MetadataSchemaGuid);
            Assert.AreEqual(expectedObjectRelationInfo.MetadataXml.ToString(), result.ObjectRelationInfos[0].MetadataXml.ToString());
        }

        [Test]
        public void ObjectGet_ByFolderIDAndIncludeFolders_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository         = Make_McmRepository();
            var objectGuid         = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedFolderInfo = this.Make_FolderTheExist();
            var folderID = (uint?)1;

            var result = repository.ObjectGet(folderID, includeFolders: true).First();
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFolderInfo.ID, result.ObjectFolders[0].ID);
            Assert.AreEqual(expectedFolderInfo.ParentID, result.ObjectFolders[0].ParentID);
            Assert.AreEqual(expectedFolderInfo.FolderTypeID, result.ObjectFolders[0].FolderTypeID);
            Assert.AreEqual(expectedFolderInfo.Name, result.ObjectFolders[0].Name);
            Assert.AreEqual(expectedFolderInfo.SubscriptionGUID, result.ObjectFolders[0].SubscriptionGUID, "SubscriptionGuid");
        }

        [Test]
        public void ObjectGet_ByFolderIDAndIncludeAccessPoints_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository = Make_McmRepository();
            var objectGuid = new Guid("00000000-0000-0000-0000-000000000002");
            var expected   = Make_AccessPoint();
            var folderID   = (uint?)1;

            var result = repository.ObjectGet(folderID, includeAccessPoints: true).First();

            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expected.AccessPointGuid, result.AccessPoints[0].AccessPointGuid);
            Assert.AreEqual(expected.ObjectGuid, result.AccessPoints[0].ObjectGuid);
            Assert.AreEqual(expected.StartDate, result.AccessPoints[0].StartDate);
            Assert.AreEqual(expected.EndDate, result.AccessPoints[0].EndDate);
            Assert.AreEqual(expected.DateCreated, result.AccessPoints[0].DateCreated);
            Assert.AreEqual(expected.DateModified, result.AccessPoints[0].DateModified);
        }

        #endregion
        #region Folders

        #endregion
        #region Helpers

        private Object Make_ObjectWithNoRelations()
        {
            return new Object
                {
                    Guid         = new Guid("00000000-0000-0000-0000-000000000005"),
                    ObjectTypeID = 1,
                    DateCreated  = new DateTime(1990, 10, 01, 23, 59, 59)
                };
        }

        private Object Make_ObjectTheDoesntExist()
        {
            return new Object
            {
                Guid         = new Guid("00000000-0000-0000-0000-000000000009"),
                ObjectTypeID = 1,
                DateCreated  = new DateTime(2000, 10, 01, 23, 59, 59)
            };
        }

        private ObjectAccessPoint Make_AccessPoint()
        {
            return new ObjectAccessPoint
                       {
                           AccessPointGuid = new Guid("00001000-0010-0000-0000-000000000000"),
                           ObjectGuid      = new Guid("00000000-0000-0000-0000-000000000002"),
                           StartDate       = new DateTime(1990, 10, 01, 23, 59, 59),
                           EndDate         = null,
                           DateCreated     = new DateTime(1990, 10, 01, 23, 59, 59),
                           DateModified    = null
                       };
        }

        private FolderInfo Make_FolderTheExist()
        {
            return new FolderInfo
                       {
                           ID = 1,
                           ParentID = null,
                           FolderTypeID = 1,
                           Name = "test",
                           SubscriptionGUID = new Guid("00000001-0000-0000-0000-000000000000")
                       };
        }

        private FileInfo Make_File()
        {
            return new FileInfo
                {
                    ID               = 1,
                    ObjectGUID       = new Guid("00000000-0000-0000-0000-000000000002"),
                    FormatID         = 1,
                    DestinationID    = 1,
                    Filename         = "file.ext",
                    OriginalFilename = "orig.ext",
                    FolderPath       = "/",
                    BasePath         = "http://bogus.com",
                    StringFormat     = "{BASE_PATH}{FOLDER_PATH}{FILENAME}",
                    Token            = "HTTP Download",
                    FormatTypeName   = "Unknown Video",
                    FormatXML        = null,
                    MimeType         = "application/octet-stream",
                    FormatCategoryID = 1,
                    FormatCategory   = "Video Source",
                    FormatTypeID     = 1,
                    FormatType       = "Video"
                };
        }

        private McmRepository Make_McmRepository()
        {
            return (McmRepository)new McmRepository().WithConfiguration(this._connectionString);
        }

        private ObjectRelation Make_ObjectRelation()
        {
            return new ObjectRelation
            {
                Object1Guid          = new Guid("00000000-0000-0000-0000-000000000003"),
                Object2Guid          = new Guid("00000000-0000-0000-0000-000000000002"),
                MetadataGuid         = null,
                Sequence             = null,
                ObjectRelationTypeID = 1
            };
        }

        private static ObjectRelationInfo Make_ObjectRelationInfoNonExistent()
        {
            return new ObjectRelationInfo
            {
                Object1Guid          = new Guid("00000000-0000-0000-0000-000000000003"),
                Object2Guid          = new Guid("00000000-0000-0000-0000-000000000002"),
                MetadataGuid         = new Guid("00000000-0000-0000-0000-000000000020"),
                Sequence             = null,
                ObjectRelationTypeID = 1,
                ObjectRelationType   = "Contains",
                LanguageCode         = "en",
                MetadataSchemaGuid   = new Guid("00000000-0000-0000-0000-000000000100"),
                MetadataXml          = XDocument.Parse("<xml>test xml</xml>")
            };
        }

        private static ObjectRelationInfo Make_ObjectRelationInfo()
        {
            return new ObjectRelationInfo
            {
                Object1Guid          = new Guid("00000000-0000-0000-0000-000000000001"),
                Object2Guid          = new Guid("00000000-0000-0000-0000-000000000002"),
                MetadataGuid         = new Guid("00000000-0000-0000-0000-000000000010"),
                Sequence             = null,
                ObjectRelationTypeID = 1,
                ObjectRelationType   = "Contains",
                LanguageCode         = "en",
                MetadataSchemaGuid   = new Guid("00000000-0000-0000-0000-000000000100"),
                MetadataXml          = XDocument.Parse("<xml>test xml</xml>")
            };
        }

        private static Metadata Make_Metadata()
        {
            return new Metadata
                {
                    Guid               = new Guid("00000000-0000-0000-0000-000000000010"),
                    MetadataSchemaGuid = new Guid("00000000-0000-0000-0000-000000000100"),
                    EditingUserGuid    = new Guid("00000000-0000-0000-0000-000000000000"),
                    RevisionID         = 0,
                    LanguageCode       = "en",
                    MetadataXml        = XDocument.Parse("<xml>test xml</xml>"),
                    DateCreated        = new DateTime(1990, 10, 01, 23, 59, 59) 
                };
        }

        #endregion
    }
}