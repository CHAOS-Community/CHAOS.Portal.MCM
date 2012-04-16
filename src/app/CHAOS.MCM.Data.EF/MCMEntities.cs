using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Geckon;
using MySql.Data.MySqlClient;

namespace CHAOS.MCM.Data.EF
{
	public partial class MCMEntities
	{
	    private string ConnectionString
	    {
	        get { return (Connection as System.Data.EntityClient.EntityConnection).StoreConnection.ConnectionString; }
	    }

        public IEnumerable<Object> Object_Get( UUID guid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders  )
        {
            return Object_Get( guid.ToString().Replace("-",""), includeMetadata, includeFiles, includeObjectRelations, includeFolders );
        }

        public IEnumerable<Object> Object_Get( IEnumerable<UUID> guids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders  )
        {
            return Object_Get( ConvertToDBList( guids ), includeMetadata, includeFiles, includeObjectRelations, includeFolders );
        }

		private IEnumerable<Object> Object_Get( string guids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders )
		{
			using( MySqlConnection conn = new MySqlConnection( ConnectionString ) )
			using( MySqlCommand comm = new MySqlCommand("Object_GetByGUIDs", conn ) )
			{
				comm.CommandType = CommandType.StoredProcedure;

				var param = comm.CreateParameter();
				param.DbType        = DbType.String;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "GUIDs";
				param.Value         = guids;
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

                param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeFolders";
				param.Value         = includeFolders;
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

				if( includeFolders )
				{
                    reader.NextResult();
                    IEnumerable<Object_Folder_Join> folders = Translate<Object_Folder_Join>(reader).ToList();

					foreach( Object o in objects )
					{
						o.Folders = (from f in folders where f.ObjectGUID == o.GUID select f).ToList();
					}
				}

				return objects;
			}		
		}

		public IEnumerable<Object> Object_Get( uint? folderID, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, uint pageIndex, uint pageSize )
		{
			using( MySqlConnection conn = new MySqlConnection( ConnectionString ) )
			using( MySqlCommand comm = new MySqlCommand("Object_GetByFolderID", conn ) )
			{
				comm.CommandType = CommandType.StoredProcedure;

				var param = comm.CreateParameter();
				param.DbType        = DbType.UInt32;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "FolderID";
				param.Value         = folderID;
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

				param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeFolders";
				param.Value         = includeFolders;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.UInt32;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "PageIndex";
				param.Value         = pageIndex;
				comm.Parameters.Add( param );

				param = comm.CreateParameter();
				param.DbType        = DbType.UInt32;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "PageSize";
				param.Value         = pageSize;
				comm.Parameters.Add( param );

				conn.Open();

				IEnumerable<Object> objects;

				using( DbDataReader reader = comm.ExecuteReader( CommandBehavior.SequentialAccess ) )
				{
					objects = Translate<Object>( reader ).ToList();

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

					if( includeFolders )
					{
                        reader.NextResult();
                        IEnumerable<Object_Folder_Join> folders = Translate<Object_Folder_Join>(reader).ToList();

						foreach( Object o in objects )
						{
							o.Folders = (from f in folders where f.ObjectGUID == o.GUID select f).ToList();
						}
					}

					return objects;
				}	
			}		
		}

		private string ConvertToDBList(IEnumerable<UUID> guids)
		{
			return String.Join( ",", guids.Select( item => item.ToString().Replace("-","") ) );
		}
	}
}
