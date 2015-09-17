CREATE VIEW ObjectMetadata 
AS 
	SELECT 
		m.GUID,
		omj.ObjectGuid,
		m.LanguageCode,
		m.MetadataSchemaGUID,
		m.EditingUserGUID,
		m.RevisionID,
		m.MetadataXML,
		m.DateCreated
	FROM
		Metadata AS m
		INNER JOIN Object_Metadata_Join AS omj ON m.GUID = omj.MetadataGuid;

