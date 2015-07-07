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

	IF EXISTS(SELECT GUID FROM Metadata WHERE Metadata.GUID = GUID AND Metadata.RevisionID <> RevisionID)	THEN
	   SELECT -201;
	ELSE
		INSERT INTO
			Metadata (GUID, LanguageCode, MetadataSchemaGUID, MetadataXml, DateCreated, EditingUserGUID) 
		VALUES 
			(GUID, LanguageCode, MetadataSchemaGUID, MetadataXML, NOW(), EditingUserGUID)
		ON DUPLICATE KEY UPDATE 
			MetadataXml     = MetadataXML, 
			DateCreated     = NOW(),
			EditingUserGUID = EditingUserGUID,
			RevisionID      = Metadata.RevisionID + 1;
		
		INSERT IGNORE INTO Object_Metadata_Join
			(ObjectGuid,MetadataGuid)
		VALUES
			(ObjectGUID,GUID);

		SELECT 1;
	END IF;

END