CREATE PROCEDURE ObjectRelation_SetMetadata
(
    Object1Guid				BINARY(16),
    Object2Guid				BINARY(16),
	MetadataGuid			BINARY(16),
    ObjectRelationTypeID	INT UNSIGNED,
    MetadataSchemaGUID		BINARY(16),
    LanguageCode		    VARCHAR(10),
    MetadataXml				MEDIUMTEXT,
    EditingUserGuid			BINARY(16)
)
BEGIN

    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;
		
        IF NOT EXISTS(	SELECT
							*
						FROM
							Object_Object_Join AS ooj
						WHERE
								ooj.Object1Guid          = Object1Guid
							AND ooj.Object2Guid          = Object2Guid
							AND	ooj.ObjectRelationTypeID = ObjectRelationTypeID )
		THEN
            
			CALL ObjectRelation_Set(Object1Guid, Object2Guid, ObjectRelationTypeID, null);

		ELSE
			                       
			UPDATE
				Object_Object_Join AS ooj
			SET
				ooj.MetadataGuid = MetadataGuid
			WHERE
					ooj.Object1Guid          = Object1Guid
				AND ooj.Object2Guid          = Object2Guid
				AND	ooj.ObjectRelationTypeID = ObjectRelationTypeID;
        
        END IF;

		IF NOT EXISTS (SELECT * FROM Metadata WHERE GUID = MetadataGuid )
		THEN

			INSERT INTO 
				Metadata
				( GUID,         LanguageCode, MetadataSchemaGUID, MetadataXml, DateCreated, EditingUserGUID )
            VALUES
				( MetadataGuid, LanguageCode, MetadataSchemaGUID, MetadataXml, NOW()      , EditingUserGuid );

			UPDATE
				Object_Object_Join AS ooj
			SET
				ooj.MetadataGuid = MetadataGuid
			WHERE
					ooj.Object1Guid          = Object1Guid
				AND ooj.Object2Guid          = Object2Guid
				AND	ooj.ObjectRelationTypeID = ObjectRelationTypeID;

		ELSE
			UPDATE
				Metadata AS m
				LEFT JOIN Object_Object_Join AS ooj ON ooj.MetadataGuid = m.GUID
			SET 
				m.MetadataXml	 = MetadataXML,
				m.DateCreated	 = NOW(),
				m.EditingUserGUID = EditingUserGUID
			WHERE 
					ooj.Object1Guid          = Object1Guid
				AND ooj.Object2Guid          = Object2Guid
				AND	ooj.ObjectRelationTypeID = ObjectRelationTypeID;

		END IF;

    COMMIT;
       
	SELECT 1;

END