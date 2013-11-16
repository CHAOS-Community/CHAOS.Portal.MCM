namespace Chaos.Mcm.Test
{
    using Mcm.Permission;
    using Portal.Core;
    using Portal.Core.Data;
    using Portal.Core.Indexing.View;
    using Configuration;
    using Data;
    using Mcm.Extension.v5.Download;
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class TestBase
    {
        protected Mock<IPortalApplication> PortalApplication { get; set; }
        protected Mock<IPortalRepository>  PortalRepository { get; set; }
        protected Mock<IViewManager>       ViewManager { get; set; }
        protected Mock<IDownloadStrategy>  DownloadStrategy { get; set; }

        protected Mock<IPermissionManager> PermissionManager { get; set; }
        protected Mock<IMcmRepository>     McmRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            PermissionManager = new Mock<IPermissionManager>();
            PortalApplication = new Mock<IPortalApplication>();
            PortalRepository  = new Mock<IPortalRepository>();
            ViewManager       = new Mock<IViewManager>();
            DownloadStrategy  = new Mock<IDownloadStrategy>();
            McmRepository = new Mock<IMcmRepository>();

            PortalApplication.SetupGet(p => p.ViewManager).Returns(ViewManager.Object);
            PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
        }

        protected McmModuleConfiguration Make_McmModuleConfiguration()
        {
            return new McmModuleConfiguration
            {
                ConnectionString = "connection string",
                ObjectCoreName = "object core name",
                Aws = new AwsConfiguration
                {
                    AccessKey = "access key",
                    SecretKey = "secret key"
                }
            };
        }
    }
}