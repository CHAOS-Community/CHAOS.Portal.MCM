namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core.Data.Model;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MetadataSchemaTest : TestBase
    {
        [Test]
        public void Get_All_CallMcmRepositoryAndReturnResults()
        {
            var extension  = this.Make_MetadMetadataSchemaExtension();
            var schema     = Make_MetadataSchema();
            var user       = Make_User();
            var groupGuids = new Guid[0];
            PortalRequest.SetupGet(p => p.User).Returns(user);
            McmRepository.Setup(m => m.MetadataSchemaGet(user.Guid, groupGuids, null, MetadataSchemaPermission.Read)).Returns(new []{schema});

            var results = extension.Get(null);

            Assert.AreEqual(schema, results.First());
        }

        [Test]
        public void Create_WithPermission_CallMcmRepositoryAndReturnTheSchema()
        {
            var extension = this.Make_MetadMetadataSchemaExtension();
            var schema    = Make_MetadataSchema();
            var userInfo  = new UserInfo
                {
                    SystemPermissonsEnum = SystemPermissons.Manage,
                    Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352")
                };
            var groupGuids = new Guid[0];
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup(m => m.MetadataSchemaGet(userInfo.Guid, groupGuids, schema.Guid, MetadataSchemaPermission.Read)).Returns(new[] { schema });

            var result = extension.Create(schema.Name, schema.SchemaXml, schema.Guid);

            Assert.AreEqual(schema, result);
        }

        [Test]
        public void Create_WithPermissionAndWithoutGuid_CallMcmRepositoryWithAGuid()
        {
            var extension = this.Make_MetadMetadataSchemaExtension();
            var schema    = Make_MetadataSchema();
            var userInfo  = new UserInfo
                {
                    SystemPermissonsEnum = SystemPermissons.Manage,
                    Guid = new Guid("c0b231e9-7d98-4f52-885e-af4837faa352")
                };
            var groupGuids = new Guid[0];
            PortalRequest.SetupGet(p => p.User).Returns(userInfo);
            McmRepository.Setup( m => m.MetadataSchemaGet( userInfo.Guid, groupGuids, It.Is<Guid?>( item => item.HasValue ), MetadataSchemaPermission.Read ) ).Returns( new[] { schema } );

            var result = extension.Create(schema.Name, schema.SchemaXml, null);

            Assert.AreEqual(schema, result);
            McmRepository.Verify( m => m.MetadataSchemaGet( userInfo.Guid, groupGuids, It.Is<Guid?>( item => item.HasValue ), MetadataSchemaPermission.Read ) );
        }

        [Test]
        public void Delete_WithPermission_CallMcmRepositoryAndReturnOne()
        {
            var extension  = this.Make_MetadMetadataSchemaExtension();
            var schema     = Make_MetadataSchema();
            var user       = Make_User();
            var groupGuids = new Guid[0];
            PortalRequest.SetupGet(p => p.User).Returns(user);
            McmRepository.Setup(m => m.MetadataSchemaGet(user.Guid, groupGuids, schema.Guid, MetadataSchemaPermission.Delete)).Returns(new []{schema});
            McmRepository.Setup(m => m.MetadataSchemaDelete(schema.Guid)).Returns(1);

            var result = extension.Delete(schema.Guid);

            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Update_WithPermission_CallMcmRepositoryAndReturnOne()
        {
            var extension  = this.Make_MetadMetadataSchemaExtension();
            var schema     = Make_MetadataSchema();
            var user       = Make_User();
            var groupGuids = new Guid[0];
            PortalRequest.SetupGet(p => p.User).Returns(user);
            McmRepository.Setup(m => m.MetadataSchemaGet(user.Guid, groupGuids, schema.Guid, It.IsAny<MetadataSchemaPermission>())).Returns(new []{schema});
            McmRepository.Setup(m => m.MetadataSchemaUpdate(schema.Name, schema.SchemaXml, user.Guid, schema.Guid)).Returns(1);

            var result = extension.Update(schema.Name, schema.SchemaXml, schema.Guid);

            Assert.AreEqual(schema, result);
        }

    }
}