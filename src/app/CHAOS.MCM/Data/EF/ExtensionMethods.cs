using System.Collections.Generic;
using System.Linq;
using CHAOS;
using Chaos.Mcm.Data.Dto.Standard;

namespace Chaos.Mcm.Data.EF
{
	public static class ExtensionMethods
    {
        #region AccessPoint

        public static IEnumerable<Dto.Standard.AccessPoint> ToDto(this IEnumerable<AccessPoint> accessPoints )
        {
            return accessPoints.Select(ToDto);
        }

        public static Dto.Standard.AccessPoint ToDto( this AccessPoint accessPoint )
        {
            return new Dto.Standard.AccessPoint
                       {
                           Guid             = accessPoint.GUID,
                           SubscriptionGuid = accessPoint.SubscriptionGUID,
                           Name             = accessPoint.Name,
                           DateCreated      = accessPoint.DateCreated
                       };
        }

        #endregion
        #region DestinationInfo

        public static IEnumerable<Dto.Standard.DestinationInfo> ToDto( this IEnumerable<DestinationInfo> destinationInfos )
		{
			return destinationInfos.Select( item => ToDto( item ) );
		}

        public static Dto.Standard.DestinationInfo ToDto(this DestinationInfo destinationInfo)
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

        public static IEnumerable<Dto.Standard.File> ToDto(this IEnumerable<File> files)
		{
		    return files.Select( ToDto );
		}

        public static Dto.Standard.File ToDto(this File file)
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

        public static IEnumerable<Dto.Standard.FileInfo> ToDto(this IEnumerable<FileInfo> fileInfos, UUID sessionGUID = null)
		{
			return fileInfos.Select( item => ToDto( item, sessionGUID ) );
		}

        // TODO: Refactor sessionGuid, fileinfo shouldn't be generated like this.
        public static Dto.Standard.FileInfo ToDto(this FileInfo fileInfo, UUID sessionGUID = null)
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

        public static IEnumerable<Dto.Standard.FolderType> ToDto(this IEnumerable<FolderType> folderTypes)
		{
			return folderTypes.Select( item => ToDto( item ) );
		}

        public static Dto.Standard.FolderType ToDto(this FolderType folderType)
		{
            return new Dto.Standard.FolderType((uint)folderType.ID, folderType.Name, folderType.DateCreated);
		}

		#endregion
		#region Object_Folder

        public static IEnumerable<Link> ToDto(this IEnumerable<Object_Folder_Join> folders)
		{
			return folders.Select( item => ToDto( item ) );
		}

        public static Link ToDto(this Object_Folder_Join folder)
		{
			return new Link( (uint) folder.FolderID, folder.ObjectGUID, (uint) folder.ObjectFolderTypeID, folder.DateCreated );
		}

		#endregion
		#region Folder

		public static IEnumerable<Dto.Standard.Folder> ToDto( this IEnumerable<Folder> folders )
		{
			return folders.Select(ToDto);
		}

		public static Dto.Standard.Folder ToDto( this Folder folder )
		{
			return new Dto.Standard.Folder( (uint) folder.ID, (uint) folder.FolderTypeID, (uint?) folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated );
		}

		#endregion
		#region FolderInfo

        public static IEnumerable<Dto.Standard.FolderInfo> ToDto(this IEnumerable<FolderInfo> folders)
		{
			return folders.Select( item => ToDto( item ) );
		}

        public static Dto.Standard.FolderInfo ToDto(this FolderInfo folder)
		{
            return new Dto.Standard.FolderInfo((uint)folder.ID, (uint)folder.FolderTypeID, (uint?)folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated, folder.NumberOfSubFolders, folder.NumberOfObjects);
		}

		#endregion
		#region FormatType

		public static IEnumerable<Dto.Standard.FormatType> ToDto( this IEnumerable<FormatType> formatTypes )
		{
			return formatTypes.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.FormatType ToDto( this FormatType formatType )
		{
			return new Dto.Standard.FormatType( (uint) formatType.ID, formatType.Name );
		}

		#endregion
		#region FormatCategory

        public static IEnumerable<Dto.Standard.FormatCategory> ToDto(this IEnumerable<FormatCategory> formatCategories)
		{
			return formatCategories.Select( item => ToDto( item ) );
		}

        public static Dto.Standard.FormatCategory ToDto(this FormatCategory formatCategory)
		{
            return new Dto.Standard.FormatCategory((uint)formatCategory.ID, formatCategory.Name);
		}

		#endregion
		#region Format

        public static IEnumerable<Dto.Standard.Format> ToDto(this IEnumerable<Format> formats)
		{
			return formats.Select( item => ToDto( item ) );
		}

        public static Dto.Standard.Format ToDto(this Format format)
		{
            return new Dto.Standard.Format((uint)format.ID, (uint)format.FormatCategoryID, format.Name, format.FormatXML, format.MimeType, format.Extension);
		}

		#endregion
		#region Language

		public static IEnumerable<Dto.Standard.Language> ToDto( this IEnumerable<Language> languages )
		{
			return languages.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.Language ToDto( this Language language )
		{
			return new Dto.Standard.Language( language.Name, language.LanguageCode );
		}

		#endregion
		#region Metadata

		public static IEnumerable<Dto.Standard.Metadata> ToDto( this IEnumerable<Metadata> metadatas )
		{
			return metadatas.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.Metadata ToDto( this Metadata metadata )
		{
			return new Dto.Standard.Metadata( metadata.GUID, metadata.ObjectGUID, metadata.LanguageCode, metadata.MetadataSchemaGUID, (uint) metadata.RevisionID, metadata.MetadataXML, metadata.DateCreated, metadata.EditingUserGUID );
		}

		#endregion
		#region MetadataSchema

		public static IEnumerable<Dto.Standard.MetadataSchema> ToDto( this IEnumerable<MetadataSchema> metadataSchemata )
		{
			return metadataSchemata.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.MetadataSchema ToDto( this MetadataSchema metadataSchema )
		{
			return new Dto.Standard.MetadataSchema( metadataSchema.GUID, metadataSchema.Name, metadataSchema.SchemaXML, metadataSchema.DateCreated );
		}

		#endregion
		#region Object

		public static IEnumerable<Dto.Standard.Object> ToDto( this IEnumerable<Object> objects, UUID sessionGUID = null )
		{
			return objects.Select( item => ToDto( item, sessionGUID ) );
		}

		public static Dto.Standard.Object ToDto( this Object obj, UUID sessionGUID = null )
		{
			return new Dto.Standard.Object( obj.GUID, (uint) obj.ObjectTypeID, obj.DateCreated, obj.pMetadatas.ToDto(), obj.pFiles.ToDto( sessionGUID ), obj.ObjectRealtions.ToDto(), obj.Folders.ToDto(), obj.AccessPoints.ToDto() );
		}

		#endregion
		#region ObjectType

		public static IEnumerable<Dto.Standard.ObjectType> ToDto( this IEnumerable<ObjectType> objectTypes )
		{
			return objectTypes.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.ObjectType ToDto( this ObjectType objectType )
		{
			return new Dto.Standard.ObjectType( (uint) objectType.ID, objectType.Name );
		}

		#endregion
		#region Object_Object_Join

		public static IEnumerable<Dto.Standard.Object_Object_Join> ToDto( this IEnumerable<Object_Object_Join> objectRelations )
		{
			return objectRelations.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.Object_Object_Join ToDto( this Object_Object_Join objectRelation )
		{
			return new Dto.Standard.Object_Object_Join( objectRelation.Object1GUID, objectRelation.Object2GUID, (uint) objectRelation.ObjectRelationTypeID, objectRelation.Sequence, objectRelation.DateCreated );
		}

		#endregion
        #region AccessPoint_Object_Join

        public static IEnumerable<Dto.Standard.AccessPoint_Object_Join> ToDto(this IEnumerable<AccessPoint_Object_Join> objectRelations)
		{
			return objectRelations.Select( ToDto );
		}

        public static Dto.Standard.AccessPoint_Object_Join ToDto(this AccessPoint_Object_Join access)
		{
            return new Dto.Standard.AccessPoint_Object_Join(access.AccessPointGUID, access.ObjectGUID, access.StartDate, access.EndDate, access.DateCreated, access.DateModified);
		}

		#endregion
		#region ObjectRelationType

		public static IEnumerable<Dto.Standard.ObjectRelationType> ToDto( this IEnumerable<ObjectRelationType> objectRelationTypes )
		{
			return objectRelationTypes.Select( item => ToDto( item ) );
		}

		public static Dto.Standard.ObjectRelationType ToDto( this ObjectRelationType objectRelationType )
		{
			return new Dto.Standard.ObjectRelationType( (uint) objectRelationType.ID, objectRelationType.Name );
		}

		#endregion
	}
}
