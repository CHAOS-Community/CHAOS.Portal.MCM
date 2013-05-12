﻿namespace Chaos.Mcm.Test.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Format = Chaos.Mcm.Data.Dto.Format;

    [TestFixture]
    public class FormatTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnAListOfFormats()
        {
            var extension = Make_FormatExtension();
            var expected  = Make_Format();
            McmRepository.Setup(m => m.FormatGet(null, null)).Returns(new []{expected});

            var result = extension.Get(null, null);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(expected.ID, result.First().ID);
        }

        [Test]
        public void Create_GivenMandatoryParameters_CallMcmRepositoryAndReturnIfOfCreatedFormat()
        {
            var extension = Make_FormatExtension();
            var expected  = Make_Format();
            var id        = 1u;

            McmRepository.Setup(m => m.FormatCreate(expected.FormatCategoryID, expected.Name, expected.FormatXml, expected.MimeType, expected.Extension)).Returns(id);
            McmRepository.Setup(m => m.FormatGet(id, null)).Returns(new[] { expected });

            var result = extension.Create(expected.FormatCategoryID, expected.Name, expected.FormatXml, expected.MimeType, expected.Extension);

            Assert.AreEqual(expected.Name, result.Name);
        }

        #region Helpers

        private static Format Make_Format()
        {
            return new Format { ID = 1 };
        }

        #endregion

    }
}