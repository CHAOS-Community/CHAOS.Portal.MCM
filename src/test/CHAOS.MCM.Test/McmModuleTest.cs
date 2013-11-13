namespace Chaos.Mcm.Test
{
    using Mcm.View;
    using Portal.Core.Data.Model;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class McmModuleTest : TestBase
    {
        [Test]
        public void Load_Default_ObjectViewShouldBeLoaded()
        {
            var module = new McmModule();
            PortalRepository.Setup(m => m.ModuleGet("MCM")).Returns(new Module { Configuration = "<Settings ConnectionString=\"connectionstring\" ObjectCoreName=\"objectcorename\"><AWS AccessKey=\"accesskey\" SecretKey=\"secretkey\"></AWS></Settings>" });

            module.Load(PortalApplication.Object);

            ViewManager.Verify(p => p.AddView(It.IsAny<ObjectView>()));
        } 
    }
}