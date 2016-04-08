CREATE PROCEDURE `File_Set`(
	Id					INT,
	ObjectId          	BINARY(16),
	ParentFileId        INT,
	FormatId            INT,
	DestinationId       INT,
	Filename            VARCHAR(1024),
	OriginalFilename    VARCHAR(1024),
	FolderPath          VARCHAR(1024)
)
BEGIN

	IF (Id IS NULL) THEN
		SELECT 0;
	ELSE
		INSERT INTO `File`
			( ID, ObjectGUID, ParentID,   FormatID, DestinationID, Filename, OriginalFilename, FolderPath, DateCreated )
		VALUES
			( Id, ObjectId, ParentFileId, FormatId, DestinationId, Filename, OriginalFilename, FolderPath, NOW() )
		ON DUPLICATE KEY
		UPDATE 
			`File`.ObjectGUID=COALESCE(ObjectId, `File`.ObjectGUID), 
			`File`.ParentID=COALESCE(ParentFileId, `File`.ParentID), 
			`File`.FormatID=COALESCE(FormatId, `File`.FormatID), 
			`File`.DestinationID=COALESCE(DestinationId, `File`.DestinationID), 
			`File`.Filename=COALESCE(Filename, `File`.Filename),
			`File`.OriginalFilename=COALESCE(OriginalFilename, `File`.OriginalFilename),
			`File`.FolderPath=COALESCE(FolderPath, `File`.FolderPath);

		SELECT last_insert_id();
	END IF;
		
END