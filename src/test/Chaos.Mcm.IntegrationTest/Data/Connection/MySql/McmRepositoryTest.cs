namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection;
    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto;

    using NUnit.Framework;

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
            var connection                 = Make_McmRepository();
            var existentObjectRelationInfo = Make_ObjectRelationInfo();

            var result = connection.ObjectRelationInfoGet(existentObjectRelationInfo.Object1Guid);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(existentObjectRelationInfo, result.First());
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
            var someUserGuid         = new Guid("10000000-0000-0000-0000-000000000000");
            nonexistentMetadata.Guid = new Guid("00000000-0000-0000-0000-000000000030"); // change to nonexistent guid

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
        
        

        #endregion
        #region Helpers

        private Gateway Make_StoredProcedure()
        {
            return new Gateway(this._connectionString);
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
                ObjectRelationType   = "test relation type",
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
                ObjectRelationType   = "test relation type",
                LanguageCode         = "en",
                MetadataSchemaGuid   = new Guid("00000000-0000-0000-0000-000000000100"),
                MetadataXml          = XDocument.Parse("<xml>test xml</xml>")
            };
        }

        private static NewMetadata Make_Metadata()
        {
            return new NewMetadata
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