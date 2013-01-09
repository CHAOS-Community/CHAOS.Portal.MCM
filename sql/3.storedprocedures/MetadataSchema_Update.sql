CREATE PROCEDURE MetadataSchema_Update
(
    GUID        BINARY(16),
    Name        VARCHAR(255),
    SchemaXML   TEXT
)
BEGIN

    UPDATE  
    	MetadataSchema
	SET  
		MetadataSchema.Name      = Name,
		MetadataSchema.SchemaXML = SchemaXML
	WHERE
		MetadataSchema.GUID = GUID;

    SELECT ROW_COUNT();

END