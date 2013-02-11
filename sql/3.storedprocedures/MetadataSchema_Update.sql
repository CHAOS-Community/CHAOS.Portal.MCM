CREATE PROCEDURE MetadataSchema_Update
(
    Guid        BINARY(16),
    Name        VARCHAR(255),
    SchemaXml   TEXT
)
BEGIN

    UPDATE  
    	MetadataSchema
	SET  
		MetadataSchema.Name      = Name,
		MetadataSchema.SchemaXML = SchemaXml
	WHERE
		MetadataSchema.GUID = Guid;

    SELECT ROW_COUNT();

END