using System.Linq;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Extension.v6;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Extension.v6
{
    [TestFixture]
    public class DestinationTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnResults()
        {
            var extension = Make_DestinationExtension();
            var expected  = Make_Detination();
            McmRepository.Setup(m => m.DestinationGet(expected.ID)).Returns(new[] { expected });

            var result = extension.Get(expected.ID);

            Assert.AreEqual(expected, result.First());
            McmRepository.Verify(m => m.DestinationGet(expected.ID));
        }    

        #region Helper
        
        private Destination Make_DestinationExtension()
        {
            return (Destination)new Destination(PortalApplication.Object, McmRepository.Object, PermissionManager.Object);
        }

        private DestinationInfo Make_Detination()
        {
            return new DestinationInfo
            {
                ID = 1u
            };
        }

        #endregion
    }
}