using System.Collections.Generic;
using System.Linq;

namespace CHAOS.MCM.Data.EF
{
	public static class ExtensionMethods
	{
		#region DestinationInfo

		public static IEnumerable<DTO.DestinationInfo> ToDTO( this IEnumerable<DestinationInfo> destinationInfos )
		{
			return destinationInfos.Select( item => ToDTO( item ) );
		}

		public static DTO.DestinationInfo ToDTO( this DestinationInfo destinationInfo )
		{
			return new DTO.DestinationInfo( (uint) destinationInfo.ID, 
												   destinationInfo.SubscriptionGUID, 
												   destinationInfo.Name, 
												   destinationInfo.BasePath, 
												   destinationInfo.StringFormat,
												   destinationInfo.Token, 
												   destinationInfo.DateCreated );
		}

		#endregion
		#region File

		public static IEnumerable<DTO.File> ToDTO( this IEnumerable<File> files )
		{
		    return files.Select( ToDTO );
		}

		public static DTO.File ToDTO( this File file )
		{
		    return new DTO.File( (uint) file.ID, 
		                             (uint?) file.ParentID, 
		                             file.ObjectGUID, 
		                             file.FileName,
		                             file.OriginalFileName, 
		                             (uint) file.FormatID,
                                     file.FolderPath );
		}

		#endregion
		#region FileInfo

		public static IEnumerable<DTO.FileInfo> ToDTO( this IEnumerable<FileInfo> fileInfos )
		{
			return fileInfos.Select( item => ToDTO( item ) );
		}

		public static DTO.FileInfo ToDTO( this FileInfo fileInfo )
		{
			return new DTO.FileInfo( (uint) fileInfo.FileID,  
									 fileInfo.ObjectGUID,
                                     (uint?) fileInfo.ParentID,
									 (uint)fileInfo.DestinationID, 
									 fileInfo.FileName, 
									 fileInfo.OriginalFileName, 
									 fileInfo.FolderPath,
									 fileInfo.FileDateCreated,
									 fileInfo.BasePath, 
									 fileInfo.StringFormat, 
									 fileInfo.AccessProviderDateCreated,
                                     fileInfo.Token,
                                     (uint) fileInfo.FormatID,
                                     fileInfo.FormatName,
                                     fileInfo.FormatXML,
                                     fileInfo.MimeType,
                                     (uint) fileInfo.FormatCategoryID,
                                     fileInfo.FormatCategoryName,
                                     (uint) fileInfo.FormatTypeID,
                                     fileInfo.FormatTypeName );
		}

		#endregion
		#region FolderType

		public static IEnumerable<DTO.FolderType> ToDTO( this IEnumerable<FolderType> folderTypes )
		{
			return folderTypes.Select( item => ToDTO( item ) );
		}

		public static DTO.FolderType ToDTO( this FolderType folderType )
		{
			return new DTO.FolderType( (uint) folderType.ID, folderType.Name, folderType.DateCreated );
		}

		#endregion
		#region Folder

        public static IEnumerable<DTO.Link> ToDTO(this IEnumerable<Object_Folder_Join> folders)
		{
			return folders.Select( item => ToDTO( item ) );
		}

        public static DTO.Link ToDTO(this Object_Folder_Join folder)
		{
			return new DTO.Link( (uint) folder.FolderID, folder.ObjectGUID, (uint) folder.ObjectFolderTypeID, folder.DateCreated );
		}

		#endregion
		#region Folder

		public static IEnumerable<DTO.Folder> ToDTO( this IEnumerable<Folder> folders )
		{
			return folders.Select( item => ToDTO( item ) );
		}

		public static DTO.Folder ToDTO( this Folder folder )
		{
			return new DTO.Folder( (uint) folder.ID, (uint) folder.FolderTypeID, (uint?) folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated );
		}

		#endregion
		#region FolderInfo

		public static IEnumerable<DTO.FolderInfo> ToDTO( this IEnumerable<FolderInfo> folders )
		{
			return folders.Select( item => ToDTO( item ) );
		}

		public static DTO.FolderInfo ToDTO( this FolderInfo folder )
		{
			return new DTO.FolderInfo( (uint) folder.ID, (uint) folder.FolderTypeID, (uint?) folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated, folder.NumberOfSubFolders, folder.NumberOfObjects );
		}

		#endregion
		#region FormatType

		public static IEnumerable<DTO.FormatType> ToDTO( this IEnumerable<FormatType> formatTypes )
		{
			return formatTypes.Select( item => ToDTO( item ) );
		}

		public static DTO.FormatType ToDTO( this FormatType formatType )
		{
			return new DTO.FormatType( (uint) formatType.ID, formatType.Name );
		}

		#endregion
		#region FormatCategory

		public static IEnumerable<DTO.FormatCategory> ToDTO( this IEnumerable<FormatCategory> formatCategories )
		{
			return formatCategories.Select( item => ToDTO( item ) );
		}

		public static DTO.FormatCategory ToDTO( this FormatCategory formatCategory )
		{
			return new DTO.FormatCategory( (uint) formatCategory.ID, formatCategory.Name );
		}

		#endregion
		#region Format

		public static IEnumerable<DTO.Format> ToDTO( this IEnumerable<Format> formats )
		{
			return formats.Select( item => ToDTO( item ) );
		}

		public static DTO.Format ToDTO( this Format format )
		{
			return new DTO.Format( (uint) format.ID, (uint) format.FormatCategoryID, format.Name, format.FormatXML, format.MimeType );
		}

		#endregion
		#region Language

		public static IEnumerable<DTO.Language> ToDTO( this IEnumerable<Language> languages )
		{
			return languages.Select( item => ToDTO( item ) );
		}

		public static DTO.Language ToDTO( this Language language )
		{
			return new DTO.Language( language.Name, language.LanguageCode );
		}

		#endregion
		#region Metadata

		public static IEnumerable<DTO.Metadata> ToDTO( this IEnumerable<Metadata> metadatas )
		{
			return metadatas.Select( item => ToDTO( item ) );
		}

		public static DTO.Metadata ToDTO( this Metadata metadata )
		{
			return new DTO.Metadata( metadata.GUID, metadata.ObjectGUID, metadata.LanguageCode, metadata.MetadataSchemaGUID, (uint) metadata.RevisionID, metadata.MetadataXML, metadata.DateCreated );
		}

		#endregion
		#region MetadataSchema

		public static IEnumerable<DTO.MetadataSchema> ToDTO( this IEnumerable<MetadataSchema> metadataSchemata )
		{
			return metadataSchemata.Select( item => ToDTO( item ) );
		}

		public static DTO.MetadataSchema ToDTO( this MetadataSchema metadataSchema )
		{
			return new DTO.MetadataSchema( metadataSchema.GUID, metadataSchema.Name, metadataSchema.SchemaXML, metadataSchema.DateCreated );
		}

		#endregion
		#region Object

		public static IEnumerable<DTO.Object> ToDTO( this IEnumerable<Object> objects )
		{
			return objects.Select( item => ToDTO( item ) );
		}

		public static DTO.Object ToDTO( this Object obj )
		{
			return new DTO.Object( obj.GUID, (uint) obj.ObjectTypeID, obj.DateCreated, obj.pMetadatas.ToDTO(), obj.pFiles.ToDTO(), obj.ObjectRealtions.ToDTO(), obj.Folders.ToDTO(), obj.AccessPoints.ToDTO() );
		}

		#endregion
		#region ObjectType

		public static IEnumerable<DTO.ObjectType> ToDTO( this IEnumerable<ObjectType> objectTypes )
		{
			return objectTypes.Select( item => ToDTO( item ) );
		}

		public static DTO.ObjectType ToDTO( this ObjectType objectType )
		{
			return new DTO.ObjectType( (uint) objectType.ID, objectType.Name );
		}

		#endregion
		#region Object_Object_Join

		public static IEnumerable<DTO.Object_Object_Join> ToDTO( this IEnumerable<Object_Object_Join> objectRelations )
		{
			return objectRelations.Select( item => ToDTO( item ) );
		}

		public static DTO.Object_Object_Join ToDTO( this Object_Object_Join objectRelation )
		{
			return new DTO.Object_Object_Join( objectRelation.Object1GUID, objectRelation.Object2GUID, (uint) objectRelation.ObjectRelationTypeID, objectRelation.Sequence, objectRelation.DateCreated );
		}

		#endregion
        #region AccessPoint_Object_Join

		public static IEnumerable<DTO.AccessPoint_Object_Join> ToDTO( this IEnumerable<AccessPoint_Object_Join> objectRelations )
		{
			return objectRelations.Select( ToDTO );
		}

		public static DTO.AccessPoint_Object_Join ToDTO( this AccessPoint_Object_Join access )
		{
			return new DTO.AccessPoint_Object_Join( access.AccessPointGUID, access.ObjectGUID, access.StartDate, access.EndDate, access.DateCreated, access.DateModified );
		}

		#endregion
		#region ObjectRelationType

		public static IEnumerable<DTO.ObjectRelationType> ToDTO( this IEnumerable<ObjectRelationType> objectRelationTypes )
		{
			return objectRelationTypes.Select( item => ToDTO( item ) );
		}

		public static DTO.ObjectRelationType ToDTO( this ObjectRelationType objectRelationType )
		{
			return new DTO.ObjectRelationType( (uint) objectRelationType.ID, objectRelationType.Name );
		}

		#endregion
	}
}
