using System.Collections.Generic;
using System.Linq;
using CHAOS.MCM.Data.Dto;
using CHAOS.MCM.Data.Dto.Standard;

namespace CHAOS.MCM.Data.EF
{
	public static class ExtensionMethods
	{
		#region DestinationInfo

		public static IEnumerable<Dto.Standard.DestinationInfo> ToDTO( this IEnumerable<DestinationInfo> destinationInfos )
		{
			return destinationInfos.Select( item => ToDTO( item ) );
		}

        public static Dto.Standard.DestinationInfo ToDTO(this DestinationInfo destinationInfo)
        {
            return new Dto.Standard.DestinationInfo((uint) destinationInfo.ID,
                                                    destinationInfo.SubscriptionGUID,
                                                    destinationInfo.Name,
                                                    destinationInfo.BasePath,
                                                    destinationInfo.StringFormat,
                                                    destinationInfo.Token,
                                                    destinationInfo.DateCreated);
        }

		#endregion
		#region File

        public static IEnumerable<Dto.Standard.File> ToDTO(this IEnumerable<File> files)
		{
		    return files.Select( ToDTO );
		}

        public static Dto.Standard.File ToDTO(this File file)
		{
            return new Dto.Standard.File((uint)file.ID, 
		                             (uint?) file.ParentID, 
		                             file.ObjectGUID, 
		                             file.FileName,
		                             file.OriginalFileName, 
		                             (uint) file.FormatID,
                                     file.FolderPath );
		}

		#endregion
		#region FileInfo

        public static IEnumerable<Dto.Standard.FileInfo> ToDTO(this IEnumerable<FileInfo> fileInfos, UUID sessionGUID = null)
		{
			return fileInfos.Select( item => ToDTO( item, sessionGUID ) );
		}

        public static Dto.Standard.FileInfo ToDTO(this FileInfo fileInfo, UUID sessionGUID = null)
		{
            return new Dto.Standard.FileInfo((uint)fileInfo.FileID,  
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
                                     fileInfo.FormatTypeName,
									 sessionGUID );
		}

		#endregion
		#region FolderType

        public static IEnumerable<Dto.Standard.FolderType> ToDTO(this IEnumerable<FolderType> folderTypes)
		{
			return folderTypes.Select( item => ToDTO( item ) );
		}

        public static Dto.Standard.FolderType ToDTO(this FolderType folderType)
		{
            return new Dto.Standard.FolderType((uint)folderType.ID, folderType.Name, folderType.DateCreated);
		}

		#endregion
		#region Object_Folder

        public static IEnumerable<Link> ToDTO(this IEnumerable<Object_Folder_Join> folders)
		{
			return folders.Select( item => ToDTO( item ) );
		}

        public static Link ToDTO(this Object_Folder_Join folder)
		{
			return new Link( (uint) folder.FolderID, folder.ObjectGUID, (uint) folder.ObjectFolderTypeID, folder.DateCreated );
		}

		#endregion
		#region Folder

		public static IEnumerable<IFolder> ToDTO( this IEnumerable<Folder> folders )
		{
			return folders.Select( item => ToDTO( item ) );
		}

		public static IFolder ToDTO( this Folder folder )
		{
			return new Dto.Standard.Folder( (uint) folder.ID, (uint) folder.FolderTypeID, (uint?) folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated );
		}

		#endregion
		#region FolderInfo

        public static IEnumerable<Dto.Standard.FolderInfo> ToDTO(this IEnumerable<FolderInfo> folders)
		{
			return folders.Select( item => ToDTO( item ) );
		}

        public static Dto.Standard.FolderInfo ToDTO(this FolderInfo folder)
		{
            return new Dto.Standard.FolderInfo((uint)folder.ID, (uint)folder.FolderTypeID, (uint?)folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated, folder.NumberOfSubFolders, folder.NumberOfObjects);
		}

		#endregion
		#region FormatType

		public static IEnumerable<Dto.Standard.FormatType> ToDTO( this IEnumerable<FormatType> formatTypes )
		{
			return formatTypes.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.FormatType ToDTO( this FormatType formatType )
		{
			return new Dto.Standard.FormatType( (uint) formatType.ID, formatType.Name );
		}

		#endregion
		#region FormatCategory

        public static IEnumerable<Dto.Standard.FormatCategory> ToDTO(this IEnumerable<FormatCategory> formatCategories)
		{
			return formatCategories.Select( item => ToDTO( item ) );
		}

        public static Dto.Standard.FormatCategory ToDTO(this FormatCategory formatCategory)
		{
            return new Dto.Standard.FormatCategory((uint)formatCategory.ID, formatCategory.Name);
		}

		#endregion
		#region Format

        public static IEnumerable<Dto.Standard.Format> ToDTO(this IEnumerable<Format> formats)
		{
			return formats.Select( item => ToDTO( item ) );
		}

        public static Dto.Standard.Format ToDTO(this Format format)
		{
            return new Dto.Standard.Format((uint)format.ID, (uint)format.FormatCategoryID, format.Name, format.FormatXML, format.MimeType, format.Extension);
		}

		#endregion
		#region Language

		public static IEnumerable<Dto.Standard.Language> ToDTO( this IEnumerable<Language> languages )
		{
			return languages.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.Language ToDTO( this Language language )
		{
			return new Dto.Standard.Language( language.Name, language.LanguageCode );
		}

		#endregion
		#region Metadata

		public static IEnumerable<Dto.Standard.Metadata> ToDTO( this IEnumerable<Metadata> metadatas )
		{
			return metadatas.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.Metadata ToDTO( this Metadata metadata )
		{
			return new Dto.Standard.Metadata( metadata.GUID, metadata.ObjectGUID, metadata.LanguageCode, metadata.MetadataSchemaGUID, (uint) metadata.RevisionID, metadata.MetadataXML, metadata.DateCreated, metadata.EditingUserGUID );
		}

		#endregion
		#region MetadataSchema

		public static IEnumerable<Dto.Standard.MetadataSchema> ToDTO( this IEnumerable<MetadataSchema> metadataSchemata )
		{
			return metadataSchemata.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.MetadataSchema ToDTO( this MetadataSchema metadataSchema )
		{
			return new Dto.Standard.MetadataSchema( metadataSchema.GUID, metadataSchema.Name, metadataSchema.SchemaXML, metadataSchema.DateCreated );
		}

		#endregion
		#region Object

		public static IEnumerable<Dto.Standard.Object> ToDTO( this IEnumerable<Object> objects, UUID sessionGUID = null )
		{
			return objects.Select( item => ToDTO( item, sessionGUID ) );
		}

		public static Dto.Standard.Object ToDTO( this Object obj, UUID sessionGUID = null )
		{
			return new Dto.Standard.Object( obj.GUID, (uint) obj.ObjectTypeID, obj.DateCreated, obj.pMetadatas.ToDTO(), obj.pFiles.ToDTO( sessionGUID ), obj.ObjectRealtions.ToDTO(), obj.Folders.ToDTO(), obj.AccessPoints.ToDTO() );
		}

		#endregion
		#region ObjectType

		public static IEnumerable<Dto.Standard.ObjectType> ToDTO( this IEnumerable<ObjectType> objectTypes )
		{
			return objectTypes.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.ObjectType ToDTO( this ObjectType objectType )
		{
			return new Dto.Standard.ObjectType( (uint) objectType.ID, objectType.Name );
		}

		#endregion
		#region Object_Object_Join

		public static IEnumerable<Dto.Standard.Object_Object_Join> ToDTO( this IEnumerable<Object_Object_Join> objectRelations )
		{
			return objectRelations.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.Object_Object_Join ToDTO( this Object_Object_Join objectRelation )
		{
			return new Dto.Standard.Object_Object_Join( objectRelation.Object1GUID, objectRelation.Object2GUID, (uint) objectRelation.ObjectRelationTypeID, objectRelation.Sequence, objectRelation.DateCreated );
		}

		#endregion
        #region AccessPoint_Object_Join

        public static IEnumerable<Dto.Standard.AccessPoint_Object_Join> ToDTO(this IEnumerable<AccessPoint_Object_Join> objectRelations)
		{
			return objectRelations.Select( ToDTO );
		}

        public static Dto.Standard.AccessPoint_Object_Join ToDTO(this AccessPoint_Object_Join access)
		{
            return new Dto.Standard.AccessPoint_Object_Join(access.AccessPointGUID, access.ObjectGUID, access.StartDate, access.EndDate, access.DateCreated, access.DateModified);
		}

		#endregion
		#region ObjectRelationType

		public static IEnumerable<Dto.Standard.ObjectRelationType> ToDTO( this IEnumerable<ObjectRelationType> objectRelationTypes )
		{
			return objectRelationTypes.Select( item => ToDTO( item ) );
		}

		public static Dto.Standard.ObjectRelationType ToDTO( this ObjectRelationType objectRelationType )
		{
			return new Dto.Standard.ObjectRelationType( (uint) objectRelationType.ID, objectRelationType.Name );
		}

		#endregion
	}
}
