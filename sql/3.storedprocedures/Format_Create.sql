CREATE PROCEDURE Format_Create
(
	FormatCategoryID    INT,
	Name                VARCHAR(255),
	FormatXml           TEXT,
	MimeType            VARCHAR(255), 
	Extension           VARCHAR(255)
)
BEGIN

    INSERT INTO Format
    	( FormatCategoryID, Name, FormatXML, MimeType, Extension )
    VALUES
    	( FormatCategoryID, Name, FormatXml, MimeType, Extension );
    
    SELECT last_insert_id();
    
END