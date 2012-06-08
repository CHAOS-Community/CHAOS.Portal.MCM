using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using MySql.Data.MySqlClient;

namespace CHAOS.MCM.Data.EF
{
	public partial class MCMEntities
	{
	    private string ConnectionString
	    {
	        get { return (Connection as System.Data.EntityClient.EntityConnection).StoreConnection.ConnectionString; }
	    }

        public IEnumerable<Object> Object_Get( UUID guid, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoints  )
        {
            return Object_Get( guid.ToString().Replace("-",""), includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints, new List<DTO.MetadataSchema>() );
        }

        public IEnumerable<Object> Object_Get(IEnumerable<UUID> guids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoints )
        {
            return Object_Get( ConvertToDBList( guids ), includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints, new List<DTO.MetadataSchema>() );
        }

		private IEnumerable<Object> Object_Get( string guids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoints, IEnumerable<DTO.MetadataSchema> metadataSchemas )
		{
			using( var conn = new MySqlConnection( ConnectionString ) )
			using( var comm = new MySqlCommand("Object_GetByGUIDs", conn ) )
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

                param = comm.CreateParameter();
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
                param.ParameterName = "IncludeAccessPoints";
				param.Value         = includeAccessPoints;
				comm.Parameters.Add( param );

				conn.Open();
			    using( var reader = comm.ExecuteReader( CommandBehavior.SequentialAccess ) )
			    {
                    var objects = Translate<Object>(reader).ToList();

			        if( includeMetadata )
			        {
			            reader.NextResult();
			            var metadatas = Translate<Metadata>(reader).ToList();

			            metadataSchemas = metadataSchemas.ToList();

			            foreach( var o in objects )
			            {
			                o.pMetadatas = (from m in metadatas
                                            where m.ObjectGUID == o.GUID && metadataSchemas.Any() || metadataSchemas.Any( meta => meta.GUID.ToByteArray() == m.GUID.ToByteArray() )
                                            select m ).ToList();
			            }
			        }

			        if( includeFiles )
			        {
			            reader.NextResult();
			            var files = Translate<FileInfo>(reader).ToList();

			            foreach( var o in objects )
			            {
			                o.pFiles = (from f in files where f.ObjectGUID == o.GUID select f).ToList();
			            }
			        }

			        if( includeObjectRelations )
			        {
			            reader.NextResult();
			            var objectRelations = Translate<Object_Object_Join>(reader).ToList();

			            foreach( var o in objects )
			            {
			                o.ObjectRealtions = (from or in objectRelations where or.Object1GUID == o.GUID || or.Object2GUID == o.GUID select or).ToList();
			            }
			        }

			        if( includeFolders )
			        {
			            reader.NextResult();
			            var folders = Translate<Object_Folder_Join>(reader).ToList();

			            foreach( var o in objects )
			            {
			                o.Folders = (from f in folders where f.ObjectGUID == o.GUID select f).ToList();
			            }
			        }

			        if( includeAccessPoints )
			        {
			            reader.NextResult();
			            var accessPoints = Translate<AccessPoint_Object_Join>(reader).ToList();

			            foreach( var o in objects )
			            {
			                o.AccessPoints = (from ap in accessPoints where ap.ObjectGUID == o.GUID select ap ).ToList();
			            }
			        }

                    return objects;
			    }
			}		
		}

		public IEnumerable<Object> Object_Get( uint? folderID, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoints, uint pageIndex, uint pageSize )
		{
			using( var conn = new MySqlConnection( ConnectionString ) )
			using( var comm = new MySqlCommand("Object_GetByFolderID", conn ) )
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
				param.DbType        = DbType.Boolean;
				param.Direction     = ParameterDirection.Input;
				param.ParameterName = "IncludeAccessPoints";
				param.Value         = includeAccessPoints;
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

			    using( var reader = comm.ExecuteReader( CommandBehavior.SequentialAccess ) )
				{
					var objects = Translate<Object>( reader ).ToList();

					if( includeMetadata )
					{
						reader.NextResult();
						var metadatas = Translate<Metadata>(reader).ToList();

						foreach( Object o in objects )
						{
							o.pMetadatas = (from m in metadatas where m.ObjectGUID == o.GUID select m ).ToList();
						}
					}

					if( includeFiles )
					{
						reader.NextResult();
						var files = Translate<FileInfo>(reader).ToList();

						foreach( Object o in objects )
						{
							o.pFiles = (from f in files where f.ObjectGUID == o.GUID select f).ToList();
						}
					}

					if( includeObjectRelations )
					{
						reader.NextResult();
						var objectRelations = Translate<Object_Object_Join>(reader).ToList();

						foreach( Object o in objects )
						{
							o.ObjectRealtions = (from or in objectRelations where or.Object1GUID == o.GUID || or.Object2GUID == o.GUID select or).ToList();
						}
					}

					if( includeFolders )
					{
                        reader.NextResult();
                        var folders = Translate<Object_Folder_Join>(reader).ToList();

						foreach( Object o in objects )
						{
							o.Folders = (from f in folders where f.ObjectGUID == o.GUID select f).ToList();
						}
					}

                    if( includeAccessPoints )
				    {
                        reader.NextResult();
                        var accessPoints = Translate<AccessPoint_Object_Join>(reader).ToList();

					    foreach( var o in objects )
					    {
                            o.AccessPoints = (from ap in accessPoints where ap.ObjectGUID == o.GUID select ap ).ToList();
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

	    public IEnumerable<Object> Object_Get( IEnumerable<UUID> guids, bool includeMetadata, bool includeFiles, bool includeObjectRelations, bool includeFolders, bool includeAccessPoints, IEnumerable<DTO.MetadataSchema> metadataSchemas )
	    {
	        return Object_Get( ConvertToDBList( guids ), includeMetadata, includeFiles, includeObjectRelations, includeFolders, includeAccessPoints, metadataSchemas );
	    }
	}
}
