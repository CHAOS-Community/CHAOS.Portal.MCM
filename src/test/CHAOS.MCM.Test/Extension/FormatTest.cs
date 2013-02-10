namespace Chaos.Mcm.Test.Extension
{
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Format = Chaos.Mcm.Data.Dto.Standard.Format;

    [TestFixture]
    public class FormatTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnAListOfFormats()
        {
            var extension = Make_FormatExtension();
            var expected  = Make_Format();
            McmRepository.Setup(m => m.FormatGet(It.IsAny<uint?>(), It.IsAny<string>()));

            var result = extension.Get(CallContext.Object, null, null);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(expected.ID, result.First().ID);
        }

        [Test]
        public void Create_GivenMandatoryParameters_CallMcmRepositoryAndReturnIfOfCreatedFormat()
        {
            var extension = Make_FormatExtension();
            var expected  = Make_Format();

            var result = extension.Create(CallContext.Object, expected.FormatCategoryID, expected.Name, expected.FormatXml, expected.MimeType, expected.Extension);

            Assert.AreEqual(1, result.Value);
        }

        #region Helpers

        private static Format Make_Format()
        {
            return new Format { ID = 1 };
        }

        private Mcm.Extension.Format Make_FormatExtension()
        {
            return (Mcm.Extension.Format)new Mcm.Extension.Format().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object);
        }

        #endregion

    }
}