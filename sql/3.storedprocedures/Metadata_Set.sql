CREATE PROCEDURE Metadata_Set
(
    GUID                BINARY(16),
	ObjectGUID          BINARY(16),
    MetadataSchemaGUID  BINARY(16),
    LanguageCode        VARCHAR(10),
    RevisionID          INT UNSIGNED,
    MetadataXML         MEDIUMTEXT,
    EditingUserGUID     BINARY(16)
)
BEGIN

    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

     IF NOT EXISTS(SELECT
						*
					FROM
						Object_Metadata_Join AS omj
						INNER JOIN Metadata AS m ON omj.MetadataGuid = m.GUID
					WHERE
							omj.ObjectGuid       = ObjectGUID
						AND	m.MetadataSchemaGUID = MetadataSchemaGUID
						AND	m.LanguageCode       = LanguageCode ) THEN
                                  
        INSERT INTO Metadata
			( GUID, LanguageCode, MetadataSchemaGUID, MetadataXml, DateCreated, EditingUserGUID )
        VALUES
			( GUID, LanguageCode, MetadataSchemaGUID, MetadataXML, NOW()      , EditingUserGUID );
                
		INSERT INTO Object_Metadata_Join
			(ObjectGuid,MetadataGuid)
		VALUES
			(ObjectGUID,GUID);

		SELECT 1;

    ELSE
  
        UPDATE 
            Metadata AS m
			LEFT JOIN Object_Metadata_Join AS omj ON m.GUID = omj.MetadataGuid
        SET 
            m.MetadataXml     = MetadataXML,
            m.DateCreated     = NOW(),
            m.EditingUserGUID = EditingUserGUID
        WHERE 
				omj.ObjectGUID       = ObjectGUID
            AND	m.MetadataSchemaGUID = MetadataSchemaGUID
            AND	m.LanguageCode       = LanguageCode;
           
		   SELECT 1;
		                        
    END IF;
       
    COMMIT;

END