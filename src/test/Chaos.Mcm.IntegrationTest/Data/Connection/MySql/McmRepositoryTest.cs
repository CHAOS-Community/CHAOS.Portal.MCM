namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection;
    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto.Standard;

    using NUnit.Framework;

    [TestFixture]
    public class McmRepositoryTest : TestBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;

        #region ObjectRelationInfo_Get

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatDoesntExist_ReturnsAEmptyList()
        {
            var connection            = this.Make_StoredProcedure();
            var nonExistantObjectGuid = new Guid("77777777-7777-7777-7777-777777777777");

            var result = connection.ObjectRelationInfoGet(nonExistantObjectGuid);

            Assert.IsEmpty(result);
        }

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatExist_ReturnsAListWithOneObjectRelationInfo()
        {
            var connection                 = Make_StoredProcedure();
            var existentObjectRelationInfo = Make_ObjectRelationInfo();

            var result = connection.ObjectRelationInfoGet(existentObjectRelationInfo.Object1Guid);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(existentObjectRelationInfo, result.First());
        }

        #endregion
        #region Set

        [Test]
        public void ObjectRelationCreate_ExclutingMetadata_CreateInDatabaseAndReturnOne()
        {
            var connection             = Make_StoredProcedure();
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
            var connection             = Make_StoredProcedure();
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

        #endregion
    }
}