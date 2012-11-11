﻿using CHAOS.MCM.Permission.Specification;
using NUnit.Framework;

namespace CHAOS.MCM.Test.Permission
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

            System.Threading.Thread.Sleep(20);

            Assert.IsTrue(wasRun);
        }
    }
}