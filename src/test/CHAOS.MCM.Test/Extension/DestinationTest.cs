namespace Chaos.Mcm.Test.Extension
{
    using System.Linq;

    using Chaos.Mcm.Data.Dto.Standard;
    using Chaos.Mcm.Extension;

    using NUnit.Framework;

    [TestFixture]
    public class DestinationTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnResults()
        {
            var extension = Make_DestinationExtension();
            var expected  = Make_Detination();
            McmRepository.Setup(m => m.DestinationGet(expected.ID)).Returns(new[] { expected });

            var result = extension.Destination_Get(CallContext.Object, expected.ID);

            Assert.AreEqual(expected, result.First());
            McmRepository.Verify(m => m.DestinationGet(expected.ID));
        }    

        #region Helper
        
        private Destination Make_DestinationExtension()
        {
            return (Destination)new Destination().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
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