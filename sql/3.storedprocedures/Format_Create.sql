DROP PROCEDURE IF EXISTS Format_Create;

DELIMITER $$

CREATE PROCEDURE Format_Create(
    IN  FormatCategoryID    INT,
    IN  Name                VARCHAR(255),
    IN  FormatXML           TEXT,
    IN  MimeType            VARCHAR(255), 
    IN  Extension           VARCHAR(255)
)
BEGIN

    INSERT INTO MCM.Format( FormatCategoryID, Name, FormatXML, MimeType, Extension )
                    VALUES( FormatCategoryID, Name, FormatXML, MimeType, Extension );
    
    SELECT last_insert_id();
    
END