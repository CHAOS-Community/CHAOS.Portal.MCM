using Chaos.Mcm.Data.Configuration;
using Chaos.Mcm.Extension.v6;

namespace Chaos.Mcm.Test.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Chaos.Mcm.Data.Dto;
    using Chaos.Portal.Core.Cache;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Request;
    using Mcm.Extension.Domain.Object;
    using Moq;

    using NUnit.Framework;
    using Metadata = Chaos.Mcm.Data.Dto.Metadata;
    using MetadataSchema = MetadataSchema;
    using Object = Chaos.Mcm.Data.Dto.Object;
    using ObjectRelation = ObjectRelation;
    using ObjectType = Chaos.Mcm.Data.Dto.ObjectType;

    public class TestBase : Test.TestBase
    {
        #region Fields

        protected Mock<IPortalRequest>     PortalRequest { get; set; }
        protected Mock<ICache>             Cache { get; set; }
        

            #endregion
        #region Initialization

        [SetUp]
        public void SetUp()
        {
            
            PortalRequest = new Mock<IPortalRequest>();
            Cache         = new Mock<ICache>();

            McmRepository.Setup(m => m.WithConfiguration(It.IsAny<string>())).Returns(McmRepository.Object);
            PortalRequest.SetupGet(p => p.User).Returns(Make_User());
        }

        protected Metadata Make_MetadataDto()
        {
            return new Metadata
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

        protected Object Make_Object()
        {
            return new Object
            {
                Guid         = new Guid("10000000-0000-0000-0000-000000000001"),
                ObjectTypeID = 1u,
                ObjectFolders = new List<ObjectFolder>{Make_ObjectFolder()}
            };
        }

        protected File Make_File()
        {
            return new File
            {
                Id = 1u,
                Filename = "actualfile.name",
                OriginalFilename = "originalfile.name",
                ObjectGuid = new Guid("30000000-0000-0000-0000-000000000003"),
                FolderPath = "/1/2/3/"
            };
        }

        protected FileInfo Make_FileInfo()
        {
            var file = Make_File();

            return new FileInfo
            {
                Id = file.Id,
                Filename = file.Filename,
                OriginalFilename = file.OriginalFilename,
                ObjectGuid = file.ObjectGuid,
                FolderPath = file.FolderPath,
                BasePath = "mybucket",
                StringFormat = "bucketname={BASE_PATH};key={FOLDER_PATH}{FILENAME}",
                Token = "S3",
                MimeType = "mp4/video"
            };
        }

        protected ObjectFolder Make_ObjectFolder()
        {
            return new ObjectFolder
                {
                    ID           = 1,
                    ParentID     = null,
                    Name         = "some name",
                    FolderTypeID = 1,
                    DateCreated  = new DateTime(1990, 10, 01, 23, 59, 59) 
                };
        }

        protected FolderInfo Make_FolderInfo()
        {
            return new FolderInfo
            {
                ID = 1,
                ParentID = null,
                FolderTypeID = 1,
                Name = "test",
                SubscriptionGuid = new Guid("00000001-0000-0000-0000-000000000000")
            };
        }

        protected Session Make_Session()
        {
            return new Session
            {
                Guid = new Guid("00001000-0000-0000-0000-000000010000"),
                UserGuid = Make_User().Guid,
                DateCreated = new DateTime(2000, 01, 01)
            };
        }

        protected UserInfo Make_User()
        {
            return new UserInfo
            {
                Guid = new Guid("00100000-0000-0000-0000-000000000100"),
                Email = "test@test.test",
                SystemPermissions = (uint?)SystemPermissons.All
            };
        }

        #endregion
        #region Helpers

		protected UserInfo SetupUser()
		{
			var userInfo = Make_User();

			PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
			PortalRequest.SetupGet(p => p.Session).Returns(Make_Session());
			PortalRequest.SetupGet(p => p.User).Returns(userInfo);

			return userInfo;
		}

        #endregion

        protected Mcm.Extension.v6.Folder Make_FolderExtension()
        {
            return (Mcm.Extension.v6.Folder)new Mcm.Extension.v6.Folder(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v6.ObjectType Make_ObjectTypeExtension()
        {
            return (Mcm.Extension.v6.ObjectType)new Mcm.Extension.v6.ObjectType(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected ObjectType Make_ObjectType()
        {
            return new ObjectType
                {
                    ID = 1,
                    Name = "some type"
                };
        }

        protected Mcm.Extension.v5.Object Make_ObjectV5Extension()
        {
            return (Mcm.Extension.v5.Object)new Mcm.Extension.v5.Object(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v5.Object Make_ObjectV5Extension(IObjectCreate objectCreate)
        {
            return (Mcm.Extension.v5.Object)new Mcm.Extension.v5.Object(PortalApplication.Object, McmRepository.Object, PermissionManager.Object, objectCreate, null, null).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v5.Object Make_ObjectV5Extension(IObjectDelete objectDelete)
        {
            return (Mcm.Extension.v5.Object)new Mcm.Extension.v5.Object(PortalApplication.Object, McmRepository.Object, PermissionManager.Object, null, objectDelete, null).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v6.Object Make_ObjectV6Extension()
        {
            return (Mcm.Extension.v6.Object)new Mcm.Extension.v6.Object(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected ObjectRelation Make_ObjectRelation()
        {
            return (ObjectRelation)new ObjectRelation(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v6.Metadata Make_MetadataExtension()
        {
            return (Mcm.Extension.v6.Metadata)new Mcm.Extension.v6.Metadata(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected MetadataSchema Make_MetadMetadataSchemaExtension()
        {
            return new MetadataSchema(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object) as MetadataSchema;
        }

        protected static Mcm.Data.Dto.MetadataSchema Make_MetadataSchema()
        {
            return new Mcm.Data.Dto.MetadataSchema
                {
                    Guid        = new Guid("463c7500-a154-5a46-b11b-f96f9b3df920"),
                    Name        = "some name",
                    SchemaXml   = XDocument.Parse("<xml />"),
                    DateCreated = new DateTime(1990, 10, 01, 23, 59, 59)
                };
        }

        protected Mcm.Extension.v6.Mcm Make_McmExtension()
        {
            return (Mcm.Extension.v6.Mcm)new Mcm.Extension.v6.Mcm(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Link Make_LinkExtension()
        {
            return (Link)new Link(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v6.Format Make_FormatExtension()
        {
            return (Mcm.Extension.v6.Format)new Mcm.Extension.v6.Format(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected Mcm.Extension.v6.File Make_FileExtension()
        {
            return (Mcm.Extension.v6.File)new Mcm.Extension.v6.File(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
        }

		protected UserManagement Make_UserManagementExtension()
		{
			return (UserManagement)new UserManagement(PortalApplication.Object, McmRepository.Object, PermissionManager.Object, new UserManagementConfiguration { UsersFolderName = "Users", UserFolderTypeId = 0, UserObjectTypeId = 0}).WithPortalRequest(PortalRequest.Object);
		}

	    protected Mcm.Extension.v6.UserProfile Make_UserProfileExtension()
	    {
			return (Mcm.Extension.v6.UserProfile)new Mcm.Extension.v6.UserProfile(PortalApplication.Object, McmRepository.Object, PermissionManager.Object).WithPortalRequest(PortalRequest.Object);
	    }
    }
}