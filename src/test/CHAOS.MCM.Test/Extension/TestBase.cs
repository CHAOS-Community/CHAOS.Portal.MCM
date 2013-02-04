namespace Chaos.Mcm.Test.Extension
{
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal;

    using Moq;

    using NUnit.Framework;

    public class TestBase
    {
        #region Fields

        protected Mock<IPermissionManager> PermissionManager { get; set; }

        protected Mock<IMcmRepository> McmRepository { get; set; }

        protected Mock<ICallContext> CallContext { get; set; }

            #endregion
        #region Initialization

        [SetUp]
        public void SetUp()
        {
            PermissionManager = new Mock<IPermissionManager>();
            McmRepository     = new Mock<IMcmRepository>();
            CallContext       = new Mock<ICallContext>();

            McmRepository.Setup(m => m.WithConfiguration(It.IsAny<string>())).Returns(McmRepository.Object);
        }

        #endregion
    }
}