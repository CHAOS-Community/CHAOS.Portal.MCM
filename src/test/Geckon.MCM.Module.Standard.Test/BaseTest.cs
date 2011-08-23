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
        public Language Afrikaans { get; set; }
        public ObjectRelationType ObjectContains { get; set; }
        public FolderType FolderType { get; set; }
        public FormatType FormatType { get; set; }

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

                AssetObjectType = (from o in db.ObjectTypes where o.ID == 1 select o).First();
                Afrikaans       = (from l in db.Languages where l.CountryName == "South Africa" select l).First();
                ObjectContains  = (from or in db.ObjectRelationTypes where or.Value == "Contains" select or ).First();
                FolderType      = (from ft in db.FolderTypes where ft.Name == "Folder" select ft ).First();
                FormatType      = (from ft in db.FormatTypes where ft.Value == "Video" select  ft ).First();
            }
        }
    }
} 
