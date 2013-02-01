CREATE PROCEDURE Metadata_Get
(
    ObjectGUID			BINARY(16),
    MetadataSchemaGUID  BINARY(16),
    LanguageCode		VARCHAR(10)
)
BEGIN

    SELECT	
    	*
      FROM	
      	Metadata
     WHERE	
     		Metadata.ObjectGUID = ObjectGUID
		AND ( MetadataSchemaGUID IS NULL OR MetadataSchemaGUID = Metadata.MetadataSchemaGUID )
		AND ( LanguageCode       IS NULL OR LanguageCode       = Metadata.LanguageCode );

END