namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Data.Connection.MySql;
    using Chaos.Mcm.Data.Dto.Standard;

    using NUnit.Framework;

    [TestFixture]
    public class StoredProceduresTest : TestBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;

        #region ObjectRelationInfo_Get

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatDoesntExist_ReturnsAEmptyList()
        {
            var connection = this.Make_StoredProcedure();
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
        #region ObjectRelation_Create

        [Test]
        public void ObjectRelationCreate_GivenCorrectParemeters_CreateInDatabaseAndReturnOne()
        {
            var connection             = Make_StoredProcedure();
            var expectedObjectRelation = Make_ObjectRelation();

            var result = connection.ObjectRelationCreate(expectedObjectRelation);

            Assert.Greater(result, 0);
            var resultObjectRealtionInfo = connection.ObjectRelationInfoGet(expectedObjectRelation.Object1Guid);
            Assert.IsNotEmpty(resultObjectRealtionInfo);
            Assert.AreEqual(expectedObjectRelation.Object1Guid, resultObjectRealtionInfo.First().Object1Guid);
            Assert.AreEqual(expectedObjectRelation.Object2Guid, resultObjectRealtionInfo.First().Object2Guid);
            Assert.AreEqual(expectedObjectRelation.MetadataGuid, resultObjectRealtionInfo.First().MetadataGuid);
            Assert.AreEqual(expectedObjectRelation.Sequence, resultObjectRealtionInfo.First().Sequence);
        }

        #endregion

        #region Helpers

        private StoredProcedures Make_StoredProcedure()
        {
            return new StoredProcedures(this._connectionString);
        }

        private ObjectRelation Make_ObjectRelation()
        {
            return new ObjectRelation
                {
                    Object1Guid          = new Guid("00000000-0000-0000-0000-000000000003"),
                    Object2Guid          = new Guid("00000000-0000-0000-0000-000000000002"),
                    MetadataGuid         = new Guid("00000000-0000-0000-0000-000000000010"),
                    Sequence             = null,
                    ObjectRelationTypeID = 1
                };
        }

        private static ObjectRelationInfo Make_ObjectRelationInfo()
        {
            return new ObjectRelationInfo
            {
                Object1Guid        = new Guid("00000000-0000-0000-0000-000000000001"),
                Object2Guid        = new Guid("00000000-0000-0000-0000-000000000002"),
                MetadataGuid       = new Guid("00000000-0000-0000-0000-000000000010"),
                Sequence           = null,
                ObjectRelationType = "test relation type",
                LanguageCode       = "en",
                MetadataSchemaGuid = new Guid("00000000-0000-0000-0000-000000000100"),
                MetadataXml        = XDocument.Parse("<xml>test xml</xml>")
            };
        }

        #endregion
    }
}