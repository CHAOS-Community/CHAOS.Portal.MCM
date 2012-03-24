using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Geckon;
using MySql.Data.MySqlClient;

namespace CHAOS.MCM.Data.EF
{
	public partial class MCMEntities
	{
		public IEnumerable<Object> Object_Get( IEnumerable<Guid> guids, bool includeMetadata, bool includeFiles, bool includeFolders, bool includeObjectRelations )
		{
			using( MySqlConnection conn = new MySqlConnection( "server=192.168.56.104;User Id=root;password=GECKONpbvu7000;Persist Security Info=True;database=MCM" ) )
			using( MySqlCommand comm = new MySqlCommand("Object_GetByGUIDs", conn ) )
			{
				comm.CommandType = CommandType.StoredProcedure;

				var param = comm.CreateParameter();
				param.DbType        = DbType.String;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "GUIDs";
				param.Value         = ConvertToDBList( guids );
				param.Size          = 21845;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeMetadata";
				param.Value         = includeMetadata;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeFiles";
				param.Value         = includeFiles;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeObjectRelations";
				param.Value         = includeObjectRelations;
				comm.Parameters.Add( param );

				conn.Open();
				DbDataReader        reader  = comm.ExecuteReader( CommandBehavior.SequentialAccess );
				IEnumerable<Object> objects = Translate<Object>( reader ).ToList();

				if( includeMetadata )
				{
					reader.NextResult();
					IEnumerable<Metadata> metadatas = Translate<Metadata>(reader).ToList();

				    foreach( Object o in objects )
				    {
				        o.pMetadatas = (from m in metadatas where m.ObjectGUID == o.GUID select m ).ToList();
				    }
				}

				if( includeFiles )
				{
					reader.NextResult();
					IEnumerable<FileInfo> files = Translate<FileInfo>(reader).ToList();

					foreach( Object o in objects )
					{
						o.pFiles = (from f in files where f.ObjectGUID == o.GUID select f).ToList();
					}
				}

				if( includeObjectRelations )
				{
					reader.NextResult();
					IEnumerable<Object_Object_Join> objectRelations = Translate<Object_Object_Join>(reader).ToList();

					foreach( Object o in objects )
					{
						o.ObjectRealtions = (from or in objectRelations where or.Object1GUID == o.GUID || or.Object2GUID == o.GUID select or).ToList();
					}
				}

				//if( includeFolders )
				//{
				//    foreach( Object o in objects )
				//    {
				//        o.Folders = Folder_Get(o.GUID, false).ToList();
				//        o.FolderTree = Folder_Get(o.ID, true).ToList();
				//    }
				//}

				return objects;
			}		
		}

		private string ConvertToDBList(IEnumerable<Guid> guids)
		{
			return String.Join( ",", guids.Select( item => item.ToUUID().ToString().Replace("-","") ) );
		}
	}
}
