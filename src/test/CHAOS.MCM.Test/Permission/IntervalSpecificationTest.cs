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
            var count = 0;

            spec.OnSynchronizationTrigger += (sender, args) => { count++; };

            for (var i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(10);

                if (count > 1) break;
            }

            System.Console.WriteLine(count);
            Assert.That(count, Is.GreaterThan(1));
        }
    }
}
