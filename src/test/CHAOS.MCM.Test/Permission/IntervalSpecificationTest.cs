using Chaos.Mcm.Permission.Specification;
using NUnit.Framework;

namespace Chaos.Mcm.Test.Permission
{
    [TestFixture]
    public class IntervalSpecificationTest
    {
        [Test]
        public void Should_synchronize_on_interval()
        {
            var spec   = new IntervalSpecification(10);
            var wasRun = false;

            spec.OnSynchronizationTrigger += (sender, args) => { wasRun = true; };

            for (var i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(10);
            }

            Assert.IsTrue(wasRun);
        }
    }
}
