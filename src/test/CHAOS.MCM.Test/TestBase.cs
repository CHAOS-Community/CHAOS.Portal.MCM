namespace Chaos.Mcm.Test
{
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data;
    using Chaos.Portal.Core.Indexing.View;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class TestBase
    {
        protected Mock<IPortalApplication> PortalApplication { get; set; }
        protected Mock<IPortalRepository>  PortalRepository { get; set; }
        protected Mock<IViewManager>       ViewManager { get; set; }

        protected Mock<IPermissionManager> PermissionManager { get; set; }

        [SetUp]
        public void SetUp()
        {
            PermissionManager = new Mock<IPermissionManager>();
            PortalApplication = new Mock<IPortalApplication>();
            PortalRepository  = new Mock<IPortalRepository>();
            ViewManager       = new Mock<IViewManager>();

            PortalApplication.SetupGet(p => p.ViewManager).Returns(ViewManager.Object);
            PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
        }
    }
}