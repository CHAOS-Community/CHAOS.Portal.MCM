﻿namespace Chaos.Mcm.Test.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Extension;
    using Chaos.Portal.Data.Dto.Standard;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectTypeTest : TestBase
    {
        [Test]
        public void Set_GivenName_CallMcmRepositoryAndReturnIdAndCallMcmRepositoryAgainToRetrieveTheDto()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { SystemPermissonsEnum = SystemPermissons.Manage });
            McmRepository.Setup(m => m.ObjectTypeSet(expected.Name)).Returns(expected.ID);

            var result = extension.Set(CallContext.Object, expected.Name);

            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.Name, result.Name);
            McmRepository.Verify(m => m.ObjectTypeGet(expected.ID, null));
        }

        [Test]
        public void Get_GivenNoParameters_CallMcmRepositoryAndReturnTheListOfObjectTypesReceived()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            McmRepository.Setup(m => m.ObjectTypeGet(null, null)).Returns(new List<Data.Dto.Standard.ObjectType>{expected});

            var results   = extension.Get(CallContext.Object);

            Assert.AreEqual(expected, results.First());
            McmRepository.Verify(m => m.ObjectTypeGet(null, null));
        }

        [Test]
        public void Delete_GivenID_CallMcmRepositoryWithIdAndReturnOneWhenSuccessful()
        {
            var extension = Make_ObjectTypeExtension();
            var expected = Make_ObjectType();
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { SystemPermissonsEnum = SystemPermissons.Manage });
            McmRepository.Setup(m => m.ObjectTypeDelete(expected.ID)).Returns(expected.ID);

            var result = extension.Delete(CallContext.Object, expected.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.ObjectTypeDelete(expected.ID));
            CallContext.Verify(p => p.User.SystemPermissonsEnum.HasFlag(SystemPermissons.Manage));
        }

        #region Helpers

        private Data.Dto.Standard.ObjectType Make_ObjectType()
        {
            return new Data.Dto.Standard.ObjectType
                {
                    ID = 1,
                    Name = "some type"
                };
        }

        private ObjectType Make_ObjectTypeExtension()
        {
            return (ObjectType)new ObjectType().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object); ;
        }

        #endregion
    }
}