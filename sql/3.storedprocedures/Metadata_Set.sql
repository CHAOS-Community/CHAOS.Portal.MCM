CREATE PROCEDURE Metadata_Set
(
    GUID                BINARY(16),
    MetadataSchemaGUID  BINARY(16),
    LanguageCode        VARCHAR(10),
    RevisionID          INT UNSIGNED,
    MetadataXML         TEXT,
    EditingUserGUID     BINARY(16)
)
BEGIN

    DECLARE Result INT DEFAULT 11;
    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

        IF( RevisionID IS NULL ) THEN
            IF ( SELECT COUNT(*)
                   FROM Metadata AS M1
                  WHERE M1.ObjectGUID         = ObjectGUID         AND
                        M1.MetadataSchemaGUID = MetadataSchemaGUID AND
                        M1.LanguageCode       = LanguageCode ) = 0 THEN
                                  
                INSERT INTO Metadata( GUID, ObjectGUID, LanguageCode, MetadataSchemaGUID, MetadataXml, DateCreated, EditingUserGUID )
                              VALUES( GUID, ObjectGUID, LanguageCode, MetadataSchemaGUID, MetadataXML, NOW()      , EditingUserGUID );
                
                SET Result = 1;
            ELSE
                -- Trying to insert metadata without revision ID, while other metadata already exist
                SET Result = -350;
            END IF;
        ELSE
    
            IF( SELECT COUNT(*)
                  FROM Object AS O
                       JOIN Metadata AS M1 ON O.GUID  = M1.ObjectGUID AND
                                              M1.GUID = ( SELECT M2.GUID
                                                            FROM Metadata AS M2
                                                           WHERE M2.ObjectGUID         = M1.ObjectGUID   AND
                                                                 M2.LanguageCode       = M1.LanguageCode AND
                                                                 M2.MetadataSchemaGUID = M1.MetadataSchemaGUID
                                                           ORDER BY M2.RevisionID DESC
                                                           LIMIT 1 )
                 WHERE M1.ObjectGUID         = ObjectGUID         AND
                       M1.MetadataSchemaGUID = MetadataSchemaGUID AND
                       M1.LanguageCode       = LanguageCode       AND
                       M1.RevisionID         = RevisionID ) > 0 THEN

               -- INSERT INTO Metadata( GUID, ObjectGUID, LanguageCode, MetadataSchemaGUID, MetadataXml, DateCreated, EditingUserGUID )
               --               VALUES( GUID, ObjectGUID, LanguageCode, MetadataSchemaGUID, MetadataXML, NOW()      , EditingUserGUID );
               
                UPDATE 
                    Metadata
                SET 
                    Metadata.MetadataXml = MetadataXML,
                    Metadata.DateCreated = NOW(),
                    Metadata.EditingUserGUID = EditingUserGUID
                WHERE 
                    Metadata.ObjectGUID         = ObjectGUID         AND
                    Metadata.MetadataSchemaGUID = MetadataSchemaGUID AND
                    Metadata.LanguageCode       = LanguageCode;
                                
                SET Result = 1;

            ELSE
                -- RevisionID has been changed, so metadata cannot be saved
                SET Result = -300;
            END IF;
        END IF;
       
    COMMIT;
       
    SELECT Result;

END