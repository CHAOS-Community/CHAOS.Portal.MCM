namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectTest : TestBase
    {
        [Test]
        public void Get_WithSingleGuid_ShouldCallMcmRepositoryWithGuid()
        {
            var extension  = Make_ObjectExtension();
            var objectGuid = new List<Guid>{Guid.NewGuid()};

            extension.Get(CallContext.Object, objectGuid, true, true, true, true, true);

            McmRepository.Verify(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), true, true, true, true, true ));
        }

        [Test]
        public void Get_WithSingleGuid_ShouldReturnObjectRecievedFromRepository()
        {
            var extension  = Make_ObjectExtension();
            var expected   = new NewObject();
            var objectGuid = new List<Guid> { Guid.NewGuid() };
            McmRepository.Setup(m => m.ObjectGet(It.IsAny<IEnumerable<Guid>>(), false, false, false, false, false)).Returns(new[] { expected });

            var result = extension.Get( CallContext.Object, objectGuid, false, false, false, false, false);

            Assert.AreEqual(expected, result.First());
        }

        #region Helpers

        private Mcm.Extension.Object Make_ObjectExtension()
        {
            return (Mcm.Extension.Object)new Mcm.Extension.Object().WithConfiguration(PermissionManager.Object, McmRepository.Object);
        }

        #endregion

    }
}