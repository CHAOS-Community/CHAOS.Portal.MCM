namespace Chaos.Mcm.Test.Extension
{
    using System;

    using CHAOS.Extensions;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal.Data.Dto.Standard;

    using Moq;

    using NUnit.Framework;

    using ObjectRelation = Chaos.Mcm.Extension.ObjectRelation;

    [TestFixture]
    public class ObjectRelationTest : TestBase
    {
        #region Set

        [Test]
        public void Set_WithoutMetadata_ShouldCallMcmRepository()
        {
            var objectRelation = this.Make_ObjectRelation();
            var object1Guid    = new Guid("00000000-0000-0000-0000-000000000001");
            var object2Guid    = new Guid("00000000-0000-0000-0000-000000000002");
            var sequence       = 0;
            uint objectRelationTypeID = 1;
            
            McmRepository.Setup(m => m.ObjectRelationSet(object1Guid, object2Guid, objectRelationTypeID, sequence)).Returns(1);
            
            var result = objectRelation.Set(CallContext.Object, object1Guid, object2Guid, new NewMetadata(), objectRelationTypeID, sequence);

            McmRepository.Verify(m => m.ObjectRelationSet(object1Guid, object2Guid, objectRelationTypeID, sequence));
            Assert.AreEqual(1, result.Value);
        }
        
        [Test]
        public void Set_WithMetadata_ShouldCallMcmRepository()
        {
            var objectRelation        = this.Make_ObjectRelation();
            var object1Guid           = new Guid("00000000-0000-0000-0000-000000000001");
            var object2Guid           = new Guid("00000000-0000-0000-0000-000000000002");
            var editingUserGuid       = new Guid("00000000-0000-0000-0000-000000000010");
            var sequence              = 0;
            uint objectRelationTypeID = 1;
            var metadata              = new NewMetadata{Guid = new Guid("00000000-0000-0000-0000-000000000100")};
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { GUID = editingUserGuid.ToUUID() });
            McmRepository.Setup(m => m.ObjectRelationSet(It.IsAny<ObjectRelationInfo>(), editingUserGuid)).Returns(1);

            var result = objectRelation.Set(CallContext.Object, object1Guid, object2Guid, metadata, objectRelationTypeID, sequence);

            McmRepository.Verify(m => m.ObjectRelationSet(It.IsAny<ObjectRelationInfo>(), editingUserGuid));
            Assert.AreEqual(1, result.Value);
        }


        #endregion
        #region Delete

        [Test]
        public void Delete_WithPermission_CallMcmRepositoryReturnOne()
        {
            var objectRelation        = this.Make_ObjectRelation();
            var object1Guid           = new Guid("00000000-0000-0000-0000-000000000001");
            var object2Guid           = new Guid("00000000-0000-0000-0000-000000000002");
            uint objectRelationTypeID = 1;
            
            McmRepository.Setup(m => m.ObjectRelationDelete(object1Guid, object2Guid, objectRelationTypeID)).Returns(1);
            
            var result = objectRelation.Delete(CallContext.Object, object1Guid, object2Guid, objectRelationTypeID);

            McmRepository.Verify(m => m.ObjectRelationDelete(object1Guid, object2Guid, objectRelationTypeID));
            Assert.AreEqual(1, result.Value);
        }

        #endregion
        #region Helpers

        private ObjectRelation Make_ObjectRelation()
        {
            return (ObjectRelation) new ObjectRelation().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion
    }
}