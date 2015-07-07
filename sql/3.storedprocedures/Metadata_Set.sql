CREATE PROCEDURE `Metadata_Set`(
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
						GUID
					FROM
						Metadata
					WHERE
						Metadata.GUID = GUID)
	THEN
                                  
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
            Metadata
        SET 
            m.MetadataXml     = MetadataXML,
            m.DateCreated     = NOW(),
            m.EditingUserGUID = EditingUserGUID,
            m.RevisionID      = m.RevisionID + 1
        WHERE 
				m.GUID = GUID
			AND m.RevisionID = RevisionID;
           
		IF(ROW_COUNT() = 0) then
			SELECT -201;
		ELSE
			SELECT 1;
		END IF;
		                        
    END IF;
       
    COMMIT;

END