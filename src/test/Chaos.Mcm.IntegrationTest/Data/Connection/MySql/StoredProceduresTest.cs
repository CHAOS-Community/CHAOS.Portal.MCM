namespace Chaos.Mcm.IntegrationTest.Data.Connection.MySql
{
    using System;
    using System.Configuration;
    using System.Linq;

    using Chaos.Mcm.Data.Connection.MySql;

    using NUnit.Framework;

    [TestFixture]
    public class StoredProceduresTest
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatDoesntExist_ReturnsAEmptyList()
        {
            var connection            = this.Make_StoredProcedure();
            var nonExistantObjectGuid = new Guid("bcfb5b11-f02b-4188-89d1-73357670fc44");

            var result = connection.ObjectRelationInfoGet(nonExistantObjectGuid);

            Assert.IsEmpty(result);
        }

        [Test]
        public void ObjectRelationInfoGet_GivenAnObjectGuidThatExist_ReturnsAListWithOneObjectRelationInfo()
        {
            var connection          = Make_StoredProcedure();
            var existantObject1Guid = new Guid("00000000-0000-0000-0000-000000000001");

            var result = connection.ObjectRelationInfoGet(existantObject1Guid);
            
            Assert.IsNotEmpty(result);
            Assert.AreEqual(existantObject1Guid, result.First().Object1Guid);
        }

        #region Helpers

        private StoredProcedures Make_StoredProcedure()
        {
            return new StoredProcedures(this._connectionString);
        }

        #endregion
    }
}