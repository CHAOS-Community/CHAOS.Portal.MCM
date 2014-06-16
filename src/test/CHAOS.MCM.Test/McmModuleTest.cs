namespace Chaos.Mcm.Test
{
    using Configuration;
    using Mcm.View;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class McmModuleTest : TestBase
    {
        [Test]
        public void Load_Default_ObjectViewShouldBeLoaded()
        {
            var module = new McmModule();
            var settings = Make_McmModuleConfiguration();
            PortalApplication.Setup(m => m.GetSettings<McmModuleConfiguration>("MCM")).Returns(settings);

            module.Load(PortalApplication.Object);

            PortalApplication.Verify(p => p.AddView(It.IsAny<ObjectView>(), settings.ObjectCoreName, false));
        } 
    }
}