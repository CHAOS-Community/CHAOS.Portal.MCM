namespace Chaos.Mcm.Test.Extension
{
    using System.Collections.Generic;

    using Chaos.Mcm.Extension;
    using Chaos.Portal.Indexing.View;

    using Moq;

    using NUnit.Framework;


    [TestFixture]
    public class McmTest : TestBase
    {
        [Test]
        public void Index_AllObjects_CallMcmRepositoryToRetriveAllObjectsAndSendThemToViewManager()
        {
            var mcm         = Make_McmExtension();
            var viewManager = new Mock<IViewManager>();
            CallContext.SetupGet(p => p.ViewManager).Returns(viewManager.Object);
            McmRepository.Setup(m => m.ObjectGet(null, It.IsAny<uint>(), It.IsAny<uint>(), true, true, true, true, true)).Returns(new Data.Dto.Object[0]);

            mcm.Index(CallContext.Object);

            McmRepository.Verify(m => m.ObjectGet(null, It.IsAny<uint>(), It.IsAny<uint>(), true, true, true, true, true));
            viewManager.Verify(m => m.Index(It.IsAny<IEnumerable<object>>()));
            viewManager.Verify(m => m.Delete());
        }

        private Mcm Make_McmExtension()
        {
            return (Mcm)new Mcm().WithConfiguration(PermissionManager.Object, McmRepository.Object);
        }
    }
}