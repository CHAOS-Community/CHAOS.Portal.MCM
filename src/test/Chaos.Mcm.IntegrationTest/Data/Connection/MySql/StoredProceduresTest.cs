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
            var existentObjectRelationInfo = new ObjectRelationInfo
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

            var result = connection.ObjectRelationInfoGet(existentObjectRelationInfo.Object1Guid);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(existentObjectRelationInfo, result.First());
        }

        #endregion

        #region Helpers

        private StoredProcedures Make_StoredProcedure()
        {
            return new StoredProcedures(this._connectionString);
        }

        #endregion
    }
}