CREATE PROCEDURE File_Set
(
	Id					INT,
	ObjectId          BINARY(16),
	ParentFileId        INT,
	FormatId            INT,
	DestinationId       INT,
	Filename            VARCHAR(1024),
	OriginalFilename    VARCHAR(1024),
	FolderPath          VARCHAR(1024)
)
BEGIN

    INSERT INTO `File`
        ( ID, ObjectGUID, ParentID,   FormatID, DestinationID, Filename, OriginalFilename, FolderPath, DateCreated )
    VALUES
        ( Id, ObjectId, ParentFileId, FormatId, DestinationId, Filename, OriginalFilename, FolderPath, NOW() )
	ON DUPLICATE KEY
    UPDATE 
		`File`.ObjectGUID=COALESCE(`File`.ObjectGUID, ObjectId), 
		`File`.ParentID=COALESCE(`File`.ParentID, ParentFileId), 
		`File`.FormatID=COALESCE(`File`.FormatID, FormatId), 
		`File`.DestinationID=COALESCE(`File`.DestinationID, DestinationId), 
		`File`.Filename=COALESCE(`File`.Filename, Filename),
		`File`.OriginalFilename=COALESCE(`File`.OriginalFilename, OriginalFilename),
		`File`.FolderPath=COALESCE(`File`.FolderPath, FolderPath);

    SELECT last_insert_id();
    
END