namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Exceptions;

    using NUnit.Framework;

    using Object = Chaos.Mcm.Data.Dto.Object;

    [TestFixture]
    public class McmRepositoryTest : TestBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;

        #region ObjectRelation

        [Test]
        public void ObjectRelationDelete_GivenGuidsThatExist_ReturnsOneAndDeletesTheObjectRelation()
        {
            var repository     = Make_McmRepository();
            var objectRelation = Make_ObjectRelationExistent();

            var result = repository.ObjectRelationDelete(objectRelation.Object1Guid, objectRelation.Object2Guid, objectRelation.ObjectRelationTypeID);
            Assert.AreEqual(1, result);
            var shouldBeEmpty = repository.ObjectRelationInfoGet(objectRelation.Object1Guid);
            Assert.IsEmpty(shouldBeEmpty);
        }

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
            var expected   = Make_ObjectRelationExistent();

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
            var existingFolder = this.Make_FolderThatExist();

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
            Assert.AreEqual(expectedFile.ObjectGuid, result.Files[0].ObjectGuid);
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
            var expectedObjectRelationInfo = Make_ObjectRelationExistent();

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
            var expectedFolderInfo = this.Make_FolderThatExist();

            var result = repository.ObjectGet(objectGuid, false, false, false, true);
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFolderInfo.ID, result.ObjectFolders[0].ID);
            Assert.AreEqual(expectedFolderInfo.ParentID, result.ObjectFolders[0].ParentID);
            Assert.AreEqual(expectedFolderInfo.FolderTypeID, result.ObjectFolders[0].FolderTypeID);
            Assert.AreEqual(expectedFolderInfo.Name, result.ObjectFolders[0].Name);
            Assert.AreEqual(expectedFolderInfo.SubscriptionGuid, result.ObjectFolders[0].SubscriptionGuid, "SubscriptionGuid");
        }
        
        [Test]
        public void ObjectGet_ByObjectGuidAndIncludeAccessPoints_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository          = Make_McmRepository();
            var objectGuid          = new Guid("00000000-0000-0000-0000-000000000002");
            var expectedAccessPoint = this.Make_ObjectAccessPoint();

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
            Assert.AreEqual(expectedFile.ObjectGuid, result.Files[0].ObjectGuid);
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
            var expectedObjectRelationInfo = Make_ObjectRelationExistent();
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
            var expectedFolderInfo = this.Make_FolderThatExist();
            var folderID = (uint?)1;

            var result = repository.ObjectGet(folderID, includeFolders: true).First();
            
            Assert.AreEqual(objectGuid, result.Guid);
            Assert.AreEqual(1, result.ObjectTypeID);
            Assert.AreEqual(new DateTime(1990, 10, 01, 23, 59, 59), result.DateCreated);
            Assert.AreEqual(expectedFolderInfo.ID, result.ObjectFolders[0].ID);
            Assert.AreEqual(expectedFolderInfo.ParentID, result.ObjectFolders[0].ParentID);
            Assert.AreEqual(expectedFolderInfo.FolderTypeID, result.ObjectFolders[0].FolderTypeID);
            Assert.AreEqual(expectedFolderInfo.Name, result.ObjectFolders[0].Name);
            Assert.AreEqual(expectedFolderInfo.SubscriptionGuid, result.ObjectFolders[0].SubscriptionGuid, "SubscriptionGuid");
            Assert.AreEqual(1, result.ObjectFolders[0].ObjectFolderTypeID, "ObjectFolderTypeID");
        }

        [Test]
        public void ObjectGet_ByFolderIDAndIncludeAccessPoints_ASingleObjectDtoCreatedFromMultipleDataResults()
        {
            var repository = Make_McmRepository();
            var objectGuid = new Guid("00000000-0000-0000-0000-000000000002");
            var expected   = this.Make_ObjectAccessPoint();
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

        [Test]
        public void FolderGet_GivenObjectGuid_ReturnFoldersWhereTheObjectIsLocated()
        {
            var repository     = Make_McmRepository();
            var objectInFolder = Make_ObjectWithRelations();
            var expected       = Make_FolderThatExist();

            var results = repository.FolderGet(null, null, objectInFolder.Guid);
            
            Assert.AreEqual(expected.ID, results[0].ID);
        }

        [Test]
        public void FolderGet_GivenNoParameters_ReturnAllFolders()
        {
            var repository = Make_McmRepository();
            var expected   = Make_FolderThatExist();

            var results = repository.FolderGet();
            
            Assert.AreEqual(expected.ID, results[0].ID);
        }

        [Test]
        public void FolderDelete_GivenIDAndNoSubfolders_ReturnOneAndFolderShouldBeDeletedInDatabase()
        {
            var repository     = Make_McmRepository();
            var folderToDelete = Make_FolderThatExistAndIsEmpty();

            var result = repository.FolderDelete(folderToDelete.ID);

            Assert.AreEqual(1, result);
            var results = repository.FolderGet(folderToDelete.ID);
            Assert.IsEmpty(results);
        }

        [Test]
        public void FolderCreate_GivenAllParameters_ReturnIDAndFolderShouldBeCreatedInDatabase()
        {
            var repository = Make_McmRepository();
            var expected   = Make_FolderThatDoesntExist();
            var userGuid   = new Guid("674c7562-dabc-49fb-8baa-e48cb865f851");

            var id = repository.FolderCreate(userGuid, expected.SubscriptionGuid, expected.Name, expected.ParentID, expected.FolderTypeID);

            var results = repository.FolderGet(id, null, null);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(id, results[0].ID);
        }

        [Test]
        public void FolderUpdate_GivenAllParameters_ReturnOneAndFolderShouldBeUpdatedInDatabase()
        {
            var repository = Make_McmRepository();
            var folder     = Make_FolderThatExist();
            var newTitle   = "new name";

            var result = repository.FolderUpdate(folder.ID, newTitle, folder.ParentID, folder.FolderTypeID);

            Assert.AreEqual(1, result);
            var results = repository.FolderGet(folder.ID, null, null);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(newTitle, results[0].Name);
        }

        [Test]
        public void FolderInfoGet_GivenAnArrayOfIDs_ReturnAListOfFolderInfos()
        {
            var repository = Make_McmRepository();
            var folderIDs  = new[] { 1u, 2u, 3u };

            var results = repository.FolderInfoGet(folderIDs);

            Assert.IsNotEmpty(results);
            Assert.AreEqual(folderIDs[0], results[0].ID);
            Assert.AreEqual(folderIDs[1], results[1].ID);
        }

        [Test]
        public void FolderInfoGet_GivenAnEmptyArray_ReturnAListOfFolderInfos()
        {
            var repository = Make_McmRepository();
            var folderIDs = new uint[0];

            var results = repository.FolderInfoGet(folderIDs);

            Assert.IsEmpty(results);
        }

        #endregion
        #region Format

        [Test]
        public void FormatGet_GivenID_ReturnFormat()
        {
            var repository = Make_McmRepository();
            var expected   = Make_FormatThatExist();

            var results = repository.FormatGet(expected.ID, null);

            Assert.IsNotEmpty(results);
            Assert.AreEqual(expected.Name, results[0].Name);
        }

        [Test]
        public void FormatCreate_GivenAllParameters_ReturnIDAndShouldBeGreatedInDatabase()
        {
            var repository = Make_McmRepository();
            var expected   = Make_FormatThatDoesntExist();

            var id = repository.FormatCreate(expected.FormatCategoryID, expected.Name, expected.FormatXml, expected.MimeType, expected.Extension);

            var results = repository.FormatGet(id, null);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(expected.Name, results[0].Name);
        }

        #endregion
        #region ObjectType

        [Test]
        public void ObjectTypeGet_GivenNoParameters_ReturnObjectType()
        {
            var repository = Make_McmRepository();

            var results = repository.ObjectTypeGet(null, null);

            Assert.IsNotEmpty(results);
            Assert.AreEqual("Asset", results[0].Name);
        }

        [Test]
        public void ObjectTypeSet_GivenName_ReturnIDShouldSetObjectTypeInDatabase()
        {
            var repository   = Make_McmRepository();
            var expectedName = "name";

            var id = repository.ObjectTypeSet(expectedName);

            var results = repository.ObjectTypeGet(id, null);
            Assert.IsNotEmpty(results);
            Assert.AreEqual(expectedName, results[0].Name);
        }

        [Test]
        public void ObjectTypeDelete_GivenID_ReturnOneAndShouldDeleteOnDatabase()
        {
            var repository   = Make_McmRepository();
            var typeToDelete = Make_ObjectTypeThatExist();

            var result = repository.ObjectTypeDelete(typeToDelete.ID);

            Assert.AreEqual(1, result);
            var results = repository.ObjectTypeGet(typeToDelete.ID, null);
            Assert.IsEmpty(results);
        }

        #endregion
        #region MetadataSchema

        [Test]
        public void MetadataSchemaGet_GivenUserGuid_ReturnTheMetadataSchemasTheUserHasReadPermissionTo()
        {
            var repository = Make_McmRepository();
            var userGuid   = new Guid("00000000-0000-0000-0000-000000001000");
            var groupGuids = new Guid[0];
            var expected   = Make_MetadataSchemaThatExist();

            var results = repository.MetadataSchemaGet(userGuid, groupGuids, null, MetadataSchemaPermission.Read);

            Assert.IsNotEmpty(results);
            Assert.AreEqual(expected.Name, results[0].Name);
        }

        [Test]
        public void MetadataDelete_GivenGuid_ReturnOneAndShouldDeleteFromDatabase()
        {
            var repository = Make_McmRepository();
            var userGuid   = new Guid("00000000-0000-0000-0000-000000001000");
            var groupGuids = new Guid[0];
            var expected   = Make_MetadataSchemaThatExist();

            var result = repository.MetadataSchemaDelete(expected.Guid);

            Assert.AreEqual(1, result);
            var shouldBeEmpty = repository.MetadataSchemaGet(userGuid, groupGuids, expected.Guid, MetadataSchemaPermission.Read);
            Assert.IsEmpty(shouldBeEmpty);
        }

        [Test]
        public void MetadataSchemaCreate_GivenAllParametersCalledForTheFirstTime_ReturnOneAndShouldBeCreatedOnTheDatabase()
        {
            var repository = Make_McmRepository();
            var userGuid   = new Guid("00000000-0000-0000-0000-000000001000");
            var groupGuids = new Guid[0];
            var expected   = Make_MetadataSchemaThatDoesntExist();

            var result = repository.MetadataSchemaCreate(expected.Name, expected.SchemaXml, userGuid, expected.Guid);

            Assert.AreEqual(1, result);
            var actual = repository.MetadataSchemaGet(userGuid, groupGuids, expected.Guid, MetadataSchemaPermission.Read);
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected.Name, actual[0].Name);
        }

        [Test]
        public void MetadataSchemaUpdate_GivenAllParametersAndAlreadyExist_ReturnOneAndSchemaNameShouldBeUpdatedInTheDatabase()
        {
            var repository    = Make_McmRepository();
            var userGuid      = new Guid("00000000-0000-0000-0000-000000001000");
            var groupGuids    = new Guid[0];
            var expected      = Make_MetadataSchemaThatExist();
            var exptectedName = "new name";

            var result = repository.MetadataSchemaUpdate(exptectedName, expected.SchemaXml, userGuid, expected.Guid);

            Assert.AreEqual(1, result);
            var actual = repository.MetadataSchemaGet(userGuid, groupGuids, expected.Guid, MetadataSchemaPermission.Read);
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(exptectedName, actual[0].Name);
        }

        #endregion
        #region Folder Permission

        [Test]
        public void FolderPermissionGet_GivenNoParameters_ReturnAllFromDatabase()
        {
            var repository        = Make_McmRepository();
            var expectedUserGuid  = new Guid("00000000-0000-0000-0000-000000001000");
            var expectedGroupGuid = new Guid("00000000-0000-0000-0000-000000010000");

            var result = repository.FolderPermissionGet();

            Assert.IsNotEmpty(result);
            Assert.AreEqual(1,result[0].FolderID);
            Assert.AreEqual(expectedUserGuid, result[0].UserPermissions[0].Guid);
            Assert.AreEqual(expectedGroupGuid, result[0].GroupPermissions[0].Guid);
        }

        [Test]
        public void FolderUserJoinSet_GivenUserGuidWithNoPermissionToFolder_ReturnOne()
        {
            var repository       = Make_McmRepository();
            var expectedUserGuid = new Guid("00000000-0000-0000-0000-000000001000");
            var folder           = Make_FolderThatExist();
            var somePermission   = 15u;

            var result = repository.FolderUserJoinSet(expectedUserGuid, folder.ID, somePermission);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void FolderGroupJoinSet_GivenGroupGuidWithNoPerission_ReturnOne()
        {
            var repository        = Make_McmRepository();
            var expectedGroupGuid = new Guid("00000000-0000-0000-0000-000000001000");
            var folder            = Make_FolderThatExist();
            var somePermission    = 15u;

            var result = repository.FolderGroupJoinSet(expectedGroupGuid, folder.ID, somePermission);

            Assert.AreEqual(1, result);
        }

        #endregion
        #region AccessPoint

        [Test]
        public void AccessPointGet_GivenReadPermission_ReturnAnAccessPoint()
        {
            var repository  = Make_McmRepository();
            var accessPoint = Make_AccessPoint();
            var userGuid    = new Guid("00000000-0000-0000-0000-000000001000");

            var results = repository.AccessPointGet(accessPoint.Guid, userGuid, new Guid[0], 1);

            Assert.IsNotEmpty(results);
            Assert.AreEqual(accessPoint.Guid, results[0].Guid);
        }

        [Test]
        public void AccessPointPublishSettingsSet_GivenAStartDate_ReturnOne()
        {
            var repository  = Make_McmRepository();
            var accessPoint = Make_AccessPoint();
            var obj         = Make_ObjectWithNoRelations();
            var startDate   = new DateTime(1990, 10, 01, 23, 59, 59);

            var result = repository.AccessPointPublishSettingsSet(accessPoint.Guid, obj.Guid, startDate, null);

            Assert.AreEqual(1, result);
        }

        #endregion
        #region Link

        [Test, ExpectedException(typeof(InsufficientPermissionsException))]
        public void LinkCreate_TryingToCreateAPhysicalLink_ThrowInsufficientPermissionsException()
        {
            var repository       = Make_McmRepository();
            var obj              = Make_ObjectWithNoRelations();
            var folder           = Make_FolderThatExistAndIsEmpty();
            var objectFolderType = 1;

            repository.LinkCreate(obj.Guid, folder.ID, objectFolderType);
        }

        [Test]
        public void LinkCreate_AddObjectToFolder_ReturnOne()
        {
            var repository       = Make_McmRepository();
            var obj              = Make_ObjectWithNoRelations();
            var folder           = Make_FolderThatExistAndIsEmpty();
            var objectFolderType = 2;

            var result = repository.LinkCreate(obj.Guid, folder.ID, objectFolderType);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void LinkUpdate_MoveToAnotherFolder_ReturnOne()
        {
            var repository   = Make_McmRepository();
            var obj          = Make_ObjectWithRelations();
            var fromFolderID = 3u;
            var toFolderID   = 2u;

            var result = repository.LinkUpdate(obj.Guid, fromFolderID, toFolderID);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void LinkDelete_RemoveReferenceFromFolder_ReturnOne()
        {
            var repository   = Make_McmRepository();
            var obj          = Make_ObjectWithRelations();
            var fromFolderID = 3u;

            var result = repository.LinkDelete(obj.Guid, fromFolderID);

            Assert.AreEqual(1, result);
        }

        #endregion
        #region Destination

        [Test]
        public void DestinationGet_All_ReturnListOfDestinationInfos()
        {
            var repository    = Make_McmRepository();
            var destinationID = 1u;

            var results = repository.DestinationGet(null);

            Assert.IsNotEmpty(results);
            Assert.AreEqual(destinationID, results[0].ID);
        }

        #endregion
        #region File

        [Test]
        public void FileCreate_NoParentID_ReturnTheNewFolderID()
        {
            var repository = Make_McmRepository();
            var file       = Make_FileTheDoesntExist();

            var result = repository.FileCreate(file.ObjectGuid, null, file.DestinationID, file.Filename, file.OriginalFilename, file.FolderPath, file.FormatID);

            Assert.Greater(result, 0);
        }

        [Test]
        public void FileDelete_NoChildFiles_ReturnOne()
        {
            var repository = Make_McmRepository();
            var file       = Make_File();

            var result = repository.FileDelete(file.ID);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void FileGet_GivenID_ReturnFileWithID()
        {
            var repository = Make_McmRepository();
            var file       = Make_File();

            var result = repository.FileGet(file.ID);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(file.Filename, result[0].Filename);
        }

        #endregion
        #region Helpers

        private MetadataSchema Make_MetadataSchemaThatExist()
        {
            return new MetadataSchema
            {
                Guid = new Guid("00000000-0000-0000-0000-000000000200"),
                Name = "test schema",
                SchemaXml = XDocument.Parse("<xml/>"),
                DateCreated = new DateTime(1990, 10, 01, 23, 59, 59),
            };
        }

        private MetadataSchema Make_MetadataSchemaThatDoesntExist()
        {
            return new MetadataSchema
            {
                Guid = new Guid("00000000-0000-0000-0000-000000000300"),
                Name = "test schema 2",
                SchemaXml = XDocument.Parse("<xml2/>"),
                DateCreated = new DateTime(1990, 10, 01, 23, 59, 59),
            };
        }

        private ObjectType Make_ObjectTypeThatExist()
        {
            return new ObjectType
            {
                ID = 10,
                Name = "testtype"
            };
        }

        private Format Make_FormatThatExist()
        {
            return new Format
            {
                ID               = 5,
                Name             = "PNG",
                FormatCategoryID = 9,
                FormatXml        = null,
                MimeType         = "image/png",
                Extension        = ".png"
            };
        }
        
        private Format Make_FormatThatDoesntExist()
        {
            return new Format
            {
                Name             = "doesntexist",
                FormatCategoryID = 9,
                FormatXml        = null,
                MimeType         = "image/png",
                Extension        = ".png"
            };
        }

        private Object Make_ObjectWithNoRelations()
        {
            return new Object
                {
                    Guid         = new Guid("00000000-0000-0000-0000-000000000005"),
                    ObjectTypeID = 1,
                    DateCreated  = new DateTime(1990, 10, 01, 23, 59, 59)
                };
        }

        private Object Make_ObjectWithRelations()
        {
            return new Object
            {
                Guid = new Guid("00000000-0000-0000-0000-000000000002"),
                ObjectTypeID = 1,
                DateCreated = new DateTime(1990, 10, 01, 23, 59, 59)
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

        private ObjectAccessPoint Make_ObjectAccessPoint()
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

        private AccessPoint Make_AccessPoint()
        {
            return new AccessPoint
            {
                Guid             = new Guid("00001000-0010-0000-0000-000000000000"),
                SubscriptionGuid = new Guid("01000000-0000-0000-0000-000000000000"),
                Name             = "test",
                DateCreated      = new DateTime(1990, 10, 01, 23, 59, 59),
            };
        }

        private FolderInfo Make_FolderThatExist()
        {
            return new FolderInfo
                       {
                           ID = 1,
                           ParentID = null,
                           FolderTypeID = 1,
                           Name = "test",
                           SubscriptionGuid = new Guid("00000001-0000-0000-0000-000000000000")
                       };
        }

        private FolderInfo Make_FolderThatDoesntExist()
        {
            return new FolderInfo
            {
                ID               = 100,
                ParentID         = null,
                FolderTypeID     = 1,
                Name             = "test",
                SubscriptionGuid = new Guid("00000001-0000-0000-0000-000000000000")
            };
        }

        private FolderInfo Make_FolderThatExistAndIsEmpty()
        {
            return new FolderInfo
            {
                ID = 2,
                ParentID = 1,
                FolderTypeID = 1,
                Name = "sub test"
            };
        }

        private FileInfo Make_File()
        {
            return new FileInfo
                {
                    ID               = 1,
                    ObjectGuid       = new Guid("00000000-0000-0000-0000-000000000002"),
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

        private FileInfo Make_FileTheDoesntExist()
        {
            return new FileInfo
                {
                    ID               = 2,
                    ObjectGuid       = new Guid("00000000-0000-0000-0000-000000000002"),
                    FormatID         = 1,
                    DestinationID    = 1,
                    Filename         = "file2.ext",
                    OriginalFilename = "orig2.ext",
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

        private static ObjectRelationInfo Make_ObjectRelationExistent()
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