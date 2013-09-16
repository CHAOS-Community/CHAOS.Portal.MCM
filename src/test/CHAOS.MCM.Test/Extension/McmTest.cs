namespace Chaos.Mcm.Test.Extension
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;


    [TestFixture]
    public class McmTest : TestBase
    {
        [Test]
        public void Index_AllObjects_CallMcmRepositoryToRetriveAllObjectsAndSendThemToViewManager()
        {
            var mcm         = Make_McmExtension();
            McmRepository.Setup(m => m.ObjectGet(null, It.IsAny<uint>(), It.IsAny<uint>(), true, true, true, true, true)).Returns(new Data.Dto.Object[0]);

            mcm.Index(null, null, true);

            ViewManager.Verify(m => m.Index(It.IsAny<IEnumerable<object>>()));
            ViewManager.Verify(m => m.Delete());
        }
    }
}