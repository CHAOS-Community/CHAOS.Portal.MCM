namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;

    using Moq;

    using NUnit.Framework;

    using ObjectType = Chaos.Mcm.Data.Dto.ObjectType;

    [TestFixture]
    public class ObjectTypeTest : TestBase
    {
        [Test]
        public void Set_GivenName_CallMcmRepositoryAndReturnIdAndCallMcmRepositoryAgainToRetrieveTheDto()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            var userInfo  = Make_User();
            userInfo.SystemPermissonsEnum = SystemPermissons.Manage;
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup(m => m.ObjectTypeSet(expected.Name, null)).Returns(expected.ID);
            McmRepository.Setup(m => m.ObjectTypeGet(expected.ID, null)).Returns(new []{expected});

            var result = extension.Set(expected.ID, expected.Name);

            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.Name, result.Name);
            McmRepository.Verify(m => m.ObjectTypeGet(expected.ID, null));
        }

        [Test, ExpectedException(typeof(InsufficientPermissionsException))]
        public void Set_WithoutPermission_ThrowException()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            var userInfo = Make_User();
            userInfo.SystemPermissonsEnum = SystemPermissons.None;
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
 
            extension.Set(expected.ID, expected.Name);
        }

        [Test]
        public void Get_GivenNoParameters_CallMcmRepositoryAndReturnTheListOfObjectTypesReceived()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            McmRepository.Setup(m => m.ObjectTypeGet(null, null)).Returns(new List<ObjectType>{expected});

            var results   = extension.Get();

            Assert.AreEqual(expected, results.First());
            McmRepository.Verify(m => m.ObjectTypeGet(null, null));
        }

        [Test]
        public void Delete_GivenID_CallMcmRepositoryWithIdAndReturnOneWhenSuccessful()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            var userInfo = Make_User();
            userInfo.SystemPermissonsEnum = SystemPermissons.Manage;
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup(m => m.ObjectTypeDelete(expected.ID)).Returns(expected.ID);

            var result = extension.Delete(expected.ID);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.ObjectTypeDelete(expected.ID));
        }

        [Test, ExpectedException( typeof( InsufficientPermissionsException ) )]
        public void Delete_WithoutPermission_ThrowException()
        {
            var extension = Make_ObjectTypeExtension();
            var expected  = Make_ObjectType();
            var userInfo = Make_User();
            userInfo.SystemPermissonsEnum = SystemPermissons.None;
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);

            extension.Delete(expected.ID);
        }

        #region Helpers

        #endregion
    }
}