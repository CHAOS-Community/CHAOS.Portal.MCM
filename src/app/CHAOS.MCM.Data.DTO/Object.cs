using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Geckon;
using Geckon.Index;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class Object : Result, IIndexable
	{
		#region Properties

		[Serialize("GUID" )]
		public UUID GUID { get; set; }

		[Serialize("ObjectTypeID" )]
		public uint ObjectTypeID { get; set; }

		[Serialize("DateCreated" )]
		public DateTime DateCreated { get; set; }

		/// <summary>
		/// This property is used to Serialize Metadata relations
		/// </summary>
		[Serialize("Metadatas" )]
		public IEnumerable<Metadata> Metadatas { get; set; }

		/// <summary>
		/// This property is used to Serialize File relations
		/// </summary>
		[Serialize("Files" )]
		public IEnumerable<FileInfo> Files { get; set; }

		[Serialize("ObjectRelations" )]
		public List<Object_Object_Join> ObjectRealtions { get; set; }

		public IEnumerable<Folder> Folders { get; set; }
		public IEnumerable<Folder> FolderTree { get; set; }
		public List<Object> RelatedObjects { get; set; }

		#endregion
		#region Constructor

		public Object( Guid guid, uint objectTypeID, DateTime dateCreated, IEnumerable<Metadata> metadatas, IEnumerable<FileInfo> fileInfos, IEnumerable<Object_Object_Join> objectObjectJoins )
		{
			GUID         = new UUID( guid.ToByteArray() );
			ObjectTypeID = objectTypeID;
			DateCreated  = dateCreated;

			Metadatas       = metadatas.ToList();
			Files           = fileInfos.ToList();
			ObjectRealtions = objectObjectJoins.ToList();
		}

		public Object()
		{
			
		}

		#endregion
		#region Business Logic

		public IEnumerable<KeyValuePair<string, string>> GetIndexableFields( )
		{
			yield return new KeyValuePair<string, string>("GUID", GUID.ToString( ) );
			yield return new KeyValuePair<string, string>("ObjectTypeID", ObjectTypeID.ToString( ) );
			yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );

			if( Folders != null )
				foreach( Folder folder in Folders )
				{
					yield return new KeyValuePair<string, string>("FolderID", folder.ID.ToString( ) );
				}

			if( FolderTree != null )
				foreach( Folder folder in FolderTree )
				{
					yield return new KeyValuePair<string, string>("FolderTree", folder.ID.ToString( ) );
				}

			// TODO: Implement Metadata XML converter

			// Convert to all field
			if( Metadatas != null )
				foreach( Metadata metadata in Metadatas )
				{
					yield return new KeyValuePair<string, string>(string.Format("m{0}_{1}_all", metadata.MetadataSchemaGUID, metadata.LanguageCode ), GetXmlContent( metadata.MetadataXML.Root ) );
				}

			if( RelatedObjects != null )
				foreach( Object obj in RelatedObjects )
				{
					foreach( Metadata relatedMetadata in obj.Metadatas )
					{
						yield return new KeyValuePair<string, string>(string.Format("rm{0}_{1}_all", relatedMetadata.MetadataSchemaGUID, relatedMetadata.LanguageCode ), GetXmlContent( relatedMetadata.MetadataXML.Root ) );
					}
				}
		}

		private string GetXmlContent(XElement xml )
		{
			StringBuilder sb = new StringBuilder( );

			foreach( XElement node in xml.Descendants( ) )
			{
				if( !node.HasElements )
					sb.AppendLine(node.Value );
			}

			return sb.ToString( );
		}

		#endregion
	}
}
