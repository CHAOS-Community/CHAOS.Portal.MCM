﻿using System.Linq;
using System.Xml.Linq;
using CHAOS.MCM.Data.EF;
using CHAOS.MCM.Module.Standard;
using Geckon.Portal.Test;
using NUnit.Framework;
using FolderType         = CHAOS.MCM.Data.DTO.FolderType;
using Folder             = CHAOS.MCM.Data.DTO.Folder;
using ObjectType         = CHAOS.MCM.Data.DTO.ObjectType;
using Language           = CHAOS.MCM.Data.DTO.Language;
using ObjectRelationType = CHAOS.MCM.Data.DTO.ObjectRelationType;
using Format             = CHAOS.MCM.Data.DTO.Format;
using FormatType         = CHAOS.MCM.Data.DTO.FormatType;
using FormatCategory     = CHAOS.MCM.Data.DTO.FormatCategory;
using DestinationInfo    = CHAOS.MCM.Data.DTO.DestinationInfo;
using MetadataSchema     = CHAOS.MCM.Data.DTO.MetadataSchema;
using Object             = CHAOS.MCM.Data.DTO.Object;

namespace Geckon.MCM.Module.Standard.Test
{
    public class BaseTest  : TestBase
    {
        #region Properties

		public MCMModule          MCMModule { get; set; }
		public ObjectType         AssetObjectType { get; set; }
		public ObjectType         DemoObjectType { get; set; }
		public Language           Afrikaans { get; set; }
		public ObjectRelationType ObjectContains { get; set; }
		public FolderType         FolderType { get; set; }
		public FolderType         FolderTestType { get; set; }
		public FormatType         FormatType { get; set; }
		public FormatCategory     FormatCategory { get; set; }
		public Folder			  TopFolder { get; set; }
		public Folder             EmptyFolder { get; set; }
		public Object             Object1 { get; set; }
	    public Object             Object2 { get; set; }
		public MetadataSchema     MetadataSchema { get; set; }
		public Format             Format { get; set; }
		public DestinationInfo        DestinationInfo { get; set; }
		//public ObjectFolderType   ObjectFolderLink { get; set; }

        #endregion

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

			using( MCMEntities db = new MCMEntities() )
			{
				db.PreTest();
				
				int folderTypeID         = db.FolderType_Create( "Folder" ).First().Value;
				int folderTestTypeID     = db.FolderType_Create("TEST").First().Value;
				int topFolderID          = db.Folder_Create( UserAdministrator.GUID.ToByteArray(), SubscriptionInfo.GUID.ToByteArray(), "top folder", null, folderTypeID, 0 ).First().Value;
				int emptyFolderID        = db.Folder_Create( UserAdministrator.GUID.ToByteArray(),null, "empty folder", topFolderID, folderTypeID, 0 ).First().Value;
				int assetObjectTypeID    = db.ObjectType_Create( "Asset" ).First().Value;
				int demoObjectTypeID     = db.ObjectType_Create( "Demo" ).First().Value;
				int objectContainsID     = db.ObjectRelationType_Create( "Contains" ).First().Value;
				int formatTypeID	     = db.FormatType_Create( "Video" ).First().Value;
				int formatCategoryID     = db.FormatCategory_Create( formatTypeID, "Video Source").First().Value;
				int formatID		     = db.Format_Create( formatCategoryID, "H.264 vb:896 ab:128", null, "video/mp4" ).First().Value;
				int destinationID	     = db.Destination_Create( SubscriptionInfo.GUID.ToByteArray(), "CHAOS Source" ).First().Value;
				int accessProvider       = db.AccessProvider_Create( destinationID, "http://example.com", "{BASE_PATH}{FOLDER_PATH}{FILENAME}", "HTTP Download" ).First().Value;
				int objectFolderTypeID   = db.ObjectFolderType_Create( "Physical" ).First().Value;
				int languageResult       = db.Language_Create( "Afrikaans", "af" ).First().Value;
				int metadataSchemaResult = db.MetadataSchema_Create( new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray(), "demo schema", "<xml />" );
				int object1Result        = db.Object_Create( new UUID("bb738610-7443-11e1-89cc-08002723312d").ToByteArray(), demoObjectTypeID, topFolderID ).First().Value;
				int object2Result        = db.Object_Create( new UUID("d7207ba4-7443-11e1-89cc-08002723312d").ToByteArray(), demoObjectTypeID, topFolderID ).First().Value;
				int metadataResult       = db.Metadata_Set( new UUID("dd68f458-3b20-4afe-92b4-a60ad3e0cc1e").ToByteArray(), new UUID("bb738610-7443-11e1-89cc-08002723312d").ToByteArray(), new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray(), "af", "<xml />", UserAdministrator.GUID.ToByteArray() ).First().Value;
				
				Afrikaans       = db.Language_Get( null, "af" ).First().ToDTO();
				FolderType      = db.FolderType_Get( folderTypeID, null ).First().ToDTO();
				FolderTestType  = db.FolderType_Get( folderTestTypeID, null ).First().ToDTO();
				TopFolder       = db.Folder_Get( topFolderID, null ).First().ToDTO();
				EmptyFolder     = db.Folder_Get( emptyFolderID, null ).First().ToDTO();
				AssetObjectType = db.ObjectType_Get( assetObjectTypeID, null ).First().ToDTO();
				DemoObjectType  = db.ObjectType_Get( demoObjectTypeID, null ).First().ToDTO();
				ObjectContains  = db.ObjectRelationType_Get( objectContainsID, null ).First().ToDTO();
				FormatType      = db.FormatType_Get( formatTypeID, null ).First().ToDTO();
				FormatCategory  = db.FormatCategory_Get( formatID, null ).First().ToDTO();
				Format          = db.Format_Get( formatID, null ).First().ToDTO();
				DestinationInfo = db.DestinationInfo_Get( destinationID ).First().ToDTO();
				MetadataSchema  = db.MetadataSchema_Get( new UUID("2df25b70-7442-11e1-89cc-08002723312d").ToByteArray() ).First().ToDTO();
				Object1         = db.Object_Get( new[]{ new UUID("bb738610-7443-11e1-89cc-08002723312d").ToGuid() }, true, true, true, true ).First().ToDTO();
				Object2         = db.Object_Get( new[]{ new UUID("d7207ba4-7443-11e1-89cc-08002723312d").ToGuid() }, true, true, true, true ).First().ToDTO();

				MCMModule = new MCMModule();
				MCMModule.Init(XElement.Parse("<Settings ConnectionString=\"metadata=res://*/MCM.csdl|res://*/MCM.ssdl|res://*/MCM.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=192.168.56.104;User Id=root;password=GECKONpbvu7000;Persist Security Info=True;database=MCM&quot;\"/>"));
			}

			//using( MCMDataContext db = new MCMDataContext( "Data Source=192.168.56.101;Initial Catalog=MCM;Persist Security Info=True;User ID=sa;Password=GECKONpbvu7000" ) )
			//{
			//   db.PopulateDefaultData();

			//    AssetObjectType  = (from o in db.ObjectTypes where o.Value == "Asset" select o).First();
			//    DemoObjectType   = (from o in db.ObjectTypes where o.Value == "demo" select o).First();
			//    Afrikaans        = (from l in db.Languages where l.LanguageCode == "af" select l).First();
			//    ObjectContains   = (from or in db.ObjectRelationTypes where or.Value == "Contains" select or ).First();
			//    FolderType       = (from ft in db.FolderTypes where ft.Name == "Folder" select ft ).First();
			//    FolderTestType   = (from ft in db.FolderTypes where ft.Name == "TEST" select ft ).First();
			//    FormatType       = (from ft in db.FormatTypes where ft.Value == "Video" select  ft ).First();
			//    TopFolder        = (from f in db.FolderInfos where f.Title == "Geckon" select f ).First();
			//    EmptyFolder      = (from f in db.FolderInfos where f.Title == "Private" select f).First();
			//    Object1          = (from o in db.Objects where o.GUID.ToString() == "0876EBF6-E30F-4A43-9B6E-F8A479F38428" select o ).First();
			//    Object2          = (from o in db.Objects where o.GUID.ToString() == "0876EBF6-E30F-4A43-9B6E-F8A479F38433" select o ).First();
			//    MetadataSchema   = (from ms in db.MetadataSchemas where ms.name == "demo" select ms ).First();
			//    Format           = (from f in db.Formats where f.Name == "Unknown format" select f).First();
			//    Destination      = (from d in db.Destinations where d.Title == "DMB Source"  select d).First();
			//    ObjectFolderLink = (from oft in db.ObjectFolderTypes where oft.Name == "Link" select oft).First();
			//}
        }

		//[Test]
		//public void TEST()
		//{
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]", FolderType.ID, FolderType.Name, FolderType.DateCreated );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]", FolderTestType.ID, FolderTestType.Name, FolderTestType.DateCreated );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]", TopFolder.ID, TopFolder.FolderTypeID, TopFolder.ParentID, TopFolder.SubscriptionGUID, TopFolder.Name, TopFolder.DateCreated );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]", EmptyFolder.ID, EmptyFolder.FolderTypeID, EmptyFolder.ParentID, EmptyFolder.SubscriptionGUID, EmptyFolder.Name, EmptyFolder.DateCreated );
		//    System.Console.WriteLine( "[{0}]\t[{1}]", AssetObjectType.ID, AssetObjectType.Name );
		//    System.Console.WriteLine( "[{0}]\t[{1}]", DemoObjectType.ID, DemoObjectType.Name );
		//    System.Console.WriteLine( "[{0}]\t[{1}]", ObjectContains.ID, ObjectContains.Name );
		//    System.Console.WriteLine( "[{0}]\t[{1}]", FormatType.ID, FormatType.Name );
		//    System.Console.WriteLine( "[{0}]\t[{1}]", FormatCategory.ID, FormatCategory.Name );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]", Format.ID, Format.FormatCategoryID, Format.Name, Format.FormatXML, Format.MimeType );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\t[{6}]", DestinationInfo.ID, DestinationInfo.SubscriptionGUID, DestinationInfo.Name, DestinationInfo.BasePath, DestinationInfo.StringFormat, DestinationInfo.Token, DestinationInfo.DateCreated );
		//    System.Console.WriteLine( "[{0}]\t[{1}]\t[{2}]\t[{3}]", MetadataSchema.GUID, MetadataSchema.Name, MetadataSchema.SchemaXML, MetadataSchema.DateCreated );
		//    System.Console.WriteLine( "[{0}]", Object1.GUID );
		//    System.Console.WriteLine( "[{0}]", Object2.GUID );
		//}
    }
} 
