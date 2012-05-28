using System;
using System.Linq;
using System.Xml.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module;
using CHAOS.Portal.Test;
using NUnit.Framework;
using DestinationInfo = CHAOS.MCM.Data.DTO.DestinationInfo;
using Folder = CHAOS.MCM.Data.DTO.Folder;
using FolderType = CHAOS.MCM.Data.DTO.FolderType;
using Format = CHAOS.MCM.Data.DTO.Format;
using FormatCategory = CHAOS.MCM.Data.DTO.FormatCategory;
using FormatType = CHAOS.MCM.Data.DTO.FormatType;
using Language = CHAOS.MCM.Data.DTO.Language;
using MetadataSchema = CHAOS.MCM.Data.DTO.MetadataSchema;
using Object = CHAOS.MCM.Data.DTO.Object;
using ObjectRelationType = CHAOS.MCM.Data.DTO.ObjectRelationType;
using ObjectType = CHAOS.MCM.Data.DTO.ObjectType;

namespace CHAOS.MCM.Test
{
    [TestFixture]
    public class MCMTestBase : TestBase
    {
        #region Properties

        public MCMModule          MCMModule { get; set; }
        public ObjectModule       ObjectModule { get; set; }
        public FolderModule       FolderModule { get; set; }
        public AccessPoint        AccessPoint { get; set; }
        public ObjectType         AssetObjectType { get; set; }
		public ObjectType         DemoObjectType { get; set; }
		public Language           Afrikaans { get; set; }
		public ObjectRelationType ObjectContains { get; set; }
		public FolderType         FolderType { get; set; }
		public FolderType         FolderTestType { get; set; }
		public FormatType         FormatType { get; set; }
		public FormatCategory     FormatCategory { get; set; }
		public Folder			  TopFolder { get; set; }
        public Folder			  SubFolder { get; set; }
		public Folder             EmptyFolder { get; set; }
		public Object             Object1 { get; set; }
	    public Object             Object2 { get; set; }
		public MetadataSchema     MetadataSchema { get; set; }
		public Format             Format { get; set; }
		public DestinationInfo    DestinationInfo { get; set; }

        #endregion
        
        [SetUp]
        public void SetUp()
        {
            base.SetUp();

            using( var db = new MCMEntities() )
			{
				db.PreTest();

                int folderTypeID         = db.FolderType_Create("Folder").First().Value;
                int folderTestTypeID     = db.FolderType_Create("TEST").First().Value;
                int topFolderID          = db.Folder_Create(UserAdministrator.GUID.ToByteArray(), SubscriptionInfo.GUID.ToByteArray(), "top folder", null, folderTypeID ).First().Value;
                int emptyFolderID        = db.Folder_Create(UserAdministrator.GUID.ToByteArray(), null, "empty folder", topFolderID, folderTypeID ).First().Value;
                int subFolderID          = db.Folder_Create(UserAdministrator.GUID.ToByteArray(), null, "sub folder", emptyFolderID, folderTypeID ).First().Value;
                int assetObjectTypeID    = db.ObjectType_Create("Asset").First().Value;
                int demoObjectTypeID     = db.ObjectType_Create("Demo").First().Value;
                int objectContainsID     = db.ObjectRelationType_Create("Contains").First().Value;
                int formatTypeID         = db.FormatType_Create("Video").First().Value;
                int formatCategoryID     = db.FormatCategory_Create(formatTypeID, "Video Source").First().Value;
                int formatID             = db.Format_Create(formatCategoryID, "H.264 vb:896 ab:128", null, "video/mp4").First().Value;
                int destinationID        = db.Destination_Create(SubscriptionInfo.GUID.ToByteArray(), "CHAOS Source").First().Value;
                int accessProvider       = db.AccessProvider_Create(destinationID, "http://example.com", "{BASE_PATH}{FOLDER_PATH}{FILENAME}", "HTTP Download").First().Value;
                int objectFolderTypeID   = db.ObjectFolderType_Create(1, "Physical").First().Value;
                int objectFolderTypeID2  = db.ObjectFolderType_Create(2, "Link").First().Value;
                int languageResult       = db.Language_Create("Afrikaans", "af").First().Value;
                int metadataSchemaResult = db.MetadataSchema_Create(new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray(), "demo schema", "<xml />");
                int object1Result        = db.Object_Create(new UUID("bb738610-7443-11e1-89cc-08002723312d").ToByteArray(), demoObjectTypeID, topFolderID).First().Value;
                int object2Result        = db.Object_Create(new UUID("d7207ba4-7443-11e1-89cc-08002723312d").ToByteArray(), demoObjectTypeID, topFolderID).First().Value;
                int object3Result        = db.Object_Create(new UUID("c7207ba4-7443-11e1-89cc-08002723312d").ToByteArray(), demoObjectTypeID, subFolderID).First().Value;
                int metadataResult       = db.Metadata_Set(new UUID("dd68f458-3b20-4afe-92b4-a60ad3e0cc1e").ToByteArray(), new UUID("bb738610-7443-11e1-89cc-08002723312d").ToByteArray(), new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray(), "af", null, "<xml />", UserAdministrator.GUID.ToByteArray()).First().Value;
                
                db.AccessPoint_Create( new UUID("11991199-7443-11e1-89cc-08002723312d").ToByteArray(), SubscriptionInfo.GUID.ToByteArray(), "Test accesspoint" );
                db.AccessPoint_Object_Join_Create( new UUID("11991199-7443-11e1-89cc-08002723312d").ToByteArray(), new UUID("bb738610-7443-11e1-89cc-08002723312d").ToByteArray(), DateTime.Now, DateTime.Now.AddDays( 1 ) );
                db.AccessPoint_User_Join_Set( new UUID("11991199-7443-11e1-89cc-08002723312d").ToByteArray(), UserAdministrator.GUID.ToByteArray(), 0x1 );

                Afrikaans       = db.Language_Get(null, "af").First().ToDTO();
                FolderType      = db.FolderType_Get(folderTypeID, null).First().ToDTO();
                FolderTestType  = db.FolderType_Get(folderTestTypeID, null).First().ToDTO();
                TopFolder       = db.Folder_Get(topFolderID, null).First().ToDTO();
                SubFolder       = db.Folder_Get(subFolderID, null).First().ToDTO();
                EmptyFolder     = db.Folder_Get(emptyFolderID, null).First().ToDTO();
                AssetObjectType = db.ObjectType_Get(assetObjectTypeID, null).First().ToDTO();
                DemoObjectType  = db.ObjectType_Get(demoObjectTypeID, null).First().ToDTO();
                ObjectContains  = db.ObjectRelationType_Get(objectContainsID, null).First().ToDTO();
                FormatType      = db.FormatType_Get(formatTypeID, null).First().ToDTO();
                FormatCategory  = db.FormatCategory_Get(formatID, null).First().ToDTO();
                Format          = db.Format_Get(formatID, null).First().ToDTO();
                DestinationInfo = db.DestinationInfo_Get(destinationID).First().ToDTO();
                MetadataSchema  = db.MetadataSchema_Get(new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray()).First().ToDTO();
                Object1         = db.Object_Get(new[] { new UUID("bb738610-7443-11e1-89cc-08002723312d") }, true, true, true, true, true ).First().ToDTO();
                Object2         = db.Object_Get(new[] { new UUID("d7207ba4-7443-11e1-89cc-08002723312d") }, true, true, true, true, true ).First().ToDTO();
                AccessPoint     = db.AccessPoint_Get( new UUID("11991199-7443-11e1-89cc-08002723312d").ToByteArray(), UserAdministrator.GUID.ToByteArray(), "", 1 ).First();

                MCMModule = new MCMModule();
                MCMModule.Initialize("<Settings ConnectionString=\"metadata=res://*/MCM.csdl|res://*/MCM.ssdl|res://*/MCM.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=MCM&quot;\"/>");

                FolderModule = new FolderModule();
                FolderModule.Initialize("<Settings ConnectionString=\"metadata=res://*/MCM.csdl|res://*/MCM.ssdl|res://*/MCM.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=MCM&quot;\"/>");

                ObjectModule = new ObjectModule();
                ObjectModule.Initialize("<Settings ConnectionString=\"metadata=res://*/MCM.csdl|res://*/MCM.ssdl|res://*/MCM.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=MCM&quot;\"/>");
			}
        }
    }
}
