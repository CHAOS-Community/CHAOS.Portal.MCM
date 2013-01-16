CREATE TRIGGER Metadata_CreateRevision BEFORE INSERT ON MCM.Metadata FOR EACH ROW
BEGIN
    DECLARE NewRevisionID INT UNSIGNED;

    SELECT  
    	COUNT(*) INTO NewRevisionID
	FROM  
		Metadata
	WHERE  
			Metadata.ObjectGUID         = NEW.ObjectGUID
        AND Metadata.LanguageCode       = NEW.LanguageCode
        AND Metadata.MetadataSchemaGUID = NEW.MetadataSchemaGUID;
            
    SET NEW.RevisionID = NewRevisionID + 1;
END