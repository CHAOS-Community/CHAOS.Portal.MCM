using System.Linq;
using System.Xml.Linq;
using Geckon.MCM.Data.Linq;
using NUnit.Framework;

namespace Geckon.MCM.Module.Standard.Test
{
    public class BaseTest : Portal.Extensions.Standard.Test.TestBase
    {
        #region Properties

        public MCMModule MCMModule { get; set; }
        public ObjectType AssetObjectType { get; set; }
        public ObjectType DemoObjectType { get; set; }
        public Language Afrikaans { get; set; }
        public ObjectRelationType ObjectContains { get; set; }
        public FolderType FolderType { get; set; }
        public FolderType FolderTestType { get; set; }
        public FormatType FormatType { get; set; }
        public FolderInfo TopFolder { get; set; }
        public FolderInfo EmptyFolder { get; set; }
        public Object     Object { get; set; }

        #endregion

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            MCMModule = new MCMModule();
            MCMModule.Init( XElement.Parse( "<Settings ConnectionString=\"Data Source=192.168.56.101;Initial Catalog=MCM;Persist Security Info=True;User ID=sa;Password=GECKONpbvu7000\"/>" ) );

            using( MCMDataContext db = new MCMDataContext( "Data Source=192.168.56.101;Initial Catalog=MCM;Persist Security Info=True;User ID=sa;Password=GECKONpbvu7000" ) )
            {
                db.PopulateDefaultData();

                AssetObjectType = (from o in db.ObjectTypes where o.Value == "Asset" select o).First();
                DemoObjectType  = (from o in db.ObjectTypes where o.Value == "demo" select o).First();
                Afrikaans       = (from l in db.Languages where l.CountryName == "South Africa" select l).First();
                ObjectContains  = (from or in db.ObjectRelationTypes where or.Value == "Contains" select or ).First();
                FolderType      = (from ft in db.FolderTypes where ft.Name == "Folder" select ft ).First();
                FolderTestType  = (from ft in db.FolderTypes where ft.Name == "TEST" select ft ).First();
                FormatType      = (from ft in db.FormatTypes where ft.Value == "Video" select  ft ).First();
                TopFolder       = (from f in db.FolderInfos where f.Title == "Geckon" select f ).First();
                EmptyFolder     = (from f in db.FolderInfos where f.Title == "sub Test" select f).First();
                Object          = (from o in db.Objects where o.GUID.ToString() == "0876EBF6-E30F-4A43-9B6E-F8A479F38427" select o ).First();
            }
        }
    }
} 
