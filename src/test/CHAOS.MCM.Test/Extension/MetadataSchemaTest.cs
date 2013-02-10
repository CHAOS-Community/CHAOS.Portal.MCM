﻿namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Mcm.Extension;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Data.Dto;
    using Chaos.Portal.Data.Dto.Standard;

    using NUnit.Framework;

    [TestFixture]
    public class MetadataSchemaTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnResults()
        {
            var extension  = this.Make_MetadMetadataSchemaExtension();
            var schema     = Make_MetadataSchema();
            var userGuid   = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352");
            var groupGuids = new Guid[0];
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { Guid = userGuid });
            CallContext.SetupGet(p => p.Groups).Returns(new IGroup[0]);
            McmRepository.Setup(m => m.MetadataSchemaGet(userGuid, groupGuids, null, MetadataSchemaPermission.Read)).Returns(new []{schema});

            var results = extension.Get(CallContext.Object, null);

            Assert.AreEqual(schema, results.First());
            McmRepository.Verify(m => m.MetadataSchemaGet(userGuid, groupGuids, null, MetadataSchemaPermission.Read));
        }

        [Test]
        public void Set_WithPermission_CallMcmRepositoryAndReturnTheSchema()
        {
            var extension = this.Make_MetadMetadataSchemaExtension();
            var schema    = Make_MetadataSchema();
            var userInfo  = new UserInfo
                {
                    SystemPermissonsEnum = SystemPermissons.Manage,
                    Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352")
                };
            CallContext.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup(m => m.MetadataSchemaGet(userInfo.Guid, null, schema.Guid, MetadataSchemaPermission.Read)).Returns(new [] {schema});

            var result = extension.Set(CallContext.Object, schema.Name, schema.SchemaXml, schema.Guid);

            Assert.AreEqual(schema, result);
            McmRepository.Verify(m => m.MetadataSchemaSet(schema.Name, schema.SchemaXml, userInfo.Guid, schema.Guid));
        }

        [Test]
        public void Delete_WithPermission_CallMcmRepositoryAndReturnOne()
        {
            var extension  = this.Make_MetadMetadataSchemaExtension();
            var schema     = Make_MetadataSchema();
            var userGuid   = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352");
            var groupGuids = new Guid[0];
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { Guid = userGuid });
            CallContext.SetupGet(p => p.Groups).Returns(new IGroup[0]);
            McmRepository.Setup(m => m.MetadataSchemaGet(userGuid, groupGuids, null, MetadataSchemaPermission.Delete));

            var result = extension.Delete(CallContext.Object, schema.Guid);

            Assert.AreEqual(1, result.Value);
            McmRepository.Verify(m => m.MetadataSchemaGet(userGuid, groupGuids, null, MetadataSchemaPermission.Delete));
            McmRepository.Verify(m => m.MetadataSchemaDelete(schema.Guid));
        }

        #region Helpers

        private MetadataSchema Make_MetadMetadataSchemaExtension()
        {
            return new MetadataSchema().WithConfiguration(this.PermissionManager.Object, this.McmRepository.Object) as MetadataSchema;
        }

        private static Data.Dto.Standard.MetadataSchema Make_MetadataSchema()
        {
            return new Data.Dto.Standard.MetadataSchema
                {
                    Guid        = new Guid("463c7500-a154-5a46-b11b-f96f9b3df920"),
                    Name        = "some name",
                    SchemaXml   = XDocument.Parse("<xml />"),
                    DateCreated = new DateTime(1990, 10, 01, 23, 59, 59)
                };
        }

        #endregion

    }
}