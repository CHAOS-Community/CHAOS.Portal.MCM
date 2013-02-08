namespace Chaos.Mcm.IntegrationTest.Data.Connection
{
    using System.Configuration;

    using Chaos.Deployment.UI.Console.Action.Database.Import;
    using NUnit.Framework;

    public class TestBase
    {
        [SetUp] 
        public void SetUp()
        {
            var importer = new ImportDeployment();

            importer.Parameters.ConnectionString = ConfigurationManager.ConnectionStrings["mcm"].ConnectionString;
            importer.Parameters.Path = @"..\..\..\..\..\sql\6.data\initial.sql";

            importer.Run();

            importer.Parameters.Path = @"..\..\..\..\..\sql\6.data\optional_types.sql";

            importer.Run();

            importer.Parameters.Path = "integraion_tests_base_data.sql";

            importer.Run();
        }
    }
}