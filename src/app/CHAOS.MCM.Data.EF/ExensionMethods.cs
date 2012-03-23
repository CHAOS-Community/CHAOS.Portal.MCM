using System.Collections.Generic;
using System.Linq;

namespace CHAOS.MCM.Data.EF
{
	public static class ExensionMethods
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

		public static IEnumerable<DTO.Folder> ToDTO( this IEnumerable<Folder> folders )
		{
			return folders.Select( item => ToDTO( item ) );
		}

		public static DTO.Folder ToDTO( this Folder folder )
		{
			return new DTO.Folder( (uint) folder.ID, (uint) folder.FolderTypeID, (uint?) folder.ParentID, folder.SubscriptionGUID, folder.Name, folder.DateCreated );
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
		#region Metadata

		public static IEnumerable<DTO.MetadataSchema> ToDTO( this IEnumerable<MetadataSchema> metadataSchemata )
		{
			return metadataSchemata.Select( item => ToDTO( item ) );
		}

		public static DTO.MetadataSchema ToDTO( this MetadataSchema metadataSchema )
		{
			return new DTO.MetadataSchema( metadataSchema.GUID, metadataSchema.Name, metadataSchema.SchemaXML, metadataSchema.DateCreated );
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
