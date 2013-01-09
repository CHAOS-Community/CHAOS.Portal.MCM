CREATE VIEW FileInfo 
AS 
	SELECT
		File.ID AS FileID, 
		File.ObjectGUID AS ObjectGUID, 
		File.ParentID AS ParentID, 
		File.DestinationID AS DestinationID, 
		File.FileName AS FileName,
		File.OriginalFileName AS OriginalFileName, 
		File.FolderPath AS FolderPath, 
		File.DateCreated AS FileDateCreated, 
		AccessProvider.BasePath AS BasePath, 
		AccessProvider.StringFormat AS StringFormat, 
		AccessProvider.DateCreated AS AccessProviderDateCreated, 
		AccessProvider.Token AS Token, 
		File.FormatID AS FormatID, 
		Format.Name AS FormatName, 
		Format.FormatXML AS FormatXML, 
		Format.MimeType AS MimeType, 
		Format.FormatCategoryID AS FormatCategoryID, 
		FormatCategory.Name AS FormatCategoryName, 
		FormatCategory.FormatTypeID AS FormatTypeID, 
		FormatType.Name AS FormatTypeName 
	FROM
		File 
		INNER JOIN AccessProvider ON AccessProvider.DestinationID = File.DestinationID 
		INNER JOIN Format 		  ON File.FormatID =  Format.ID
		INNER JOIN FormatCategory ON Format.FormatCategoryID =  FormatCategory.ID 
		INNER JOIN FormatType 	  ON FormatCategory.FormatTypeID =  FormatType.ID;

