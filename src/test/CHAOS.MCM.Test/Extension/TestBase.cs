namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Xml.Linq;

    using Chaos.Mcm.Data;
    using Chaos.Mcm.Data.Dto;
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

        protected NewMetadata Make_MetadataDto()
        {
            return new NewMetadata
                {
                    Guid               = new Guid("00000000-0000-0000-0000-000000000010"),
                    MetadataSchemaGuid = new Guid("00000000-0000-0000-0000-000000000100"),
                    EditingUserGuid    = new Guid("00000000-0000-0000-0000-000000000000"),
                    RevisionID         = 0,
                    LanguageCode       = "en",
                    MetadataXml        = XDocument.Parse("<xml>test xml</xml>"),
                    DateCreated        = new DateTime(1990, 10, 01, 23, 59, 59) 
                };
        }

        #endregion
    }
}