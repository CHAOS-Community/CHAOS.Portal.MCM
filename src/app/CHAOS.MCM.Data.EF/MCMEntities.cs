using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace CHAOS.MCM.Data.EF
{
	public partial class MCMEntities
	{
		public void Test()
		{
			using( MySqlConnection conn = new MySqlConnection( "server=192.168.56.104;User Id=root;password=GECKONpbvu7000;Persist Security Info=True;database=MCM" ) )
			using( MySqlCommand comm = new MySqlCommand("Object_GetByGUIDs", conn ) )
			{
				comm.CommandType = CommandType.StoredProcedure;

				var param = comm.CreateParameter();
				param.DbType        = DbType.String;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "GUIDs";
				param.Value         = "bb738610744311e189cc08002723312d";
				param.Size          = 65535;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeMetadata";
				param.Value         = false;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeFiles";
				param.Value         = false;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeObjectRelations";
				param.Value         = false;
				comm.Parameters.Add( param );

				conn.Open();
				DbDataReader reader = comm.ExecuteReader(CommandBehavior.SequentialAccess);
				System.Console.WriteLine( reader.GetName(0) );
				EF.Object ob = Translate<Object>(reader).First();
			}		
		}
	}
}
