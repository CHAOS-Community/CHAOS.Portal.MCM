CREATE PROCEDURE File_Create
(
	ObjectGuid          BINARY(16),
	ParentFileID        INT,
	FormatID            INT,
	DestinationID       INT,
	Filename            VARCHAR(1024),
	OriginalFilename    VARCHAR(1024),
	FolderPath          VARCHAR(1024)
)
BEGIN

    INSERT INTO File
        ( ObjectGUID, ParentID,     FormatID, DestinationID, Filename, OriginalFilename, FolderPath, DateCreated )
    VALUES
        ( ObjectGuid, ParentFileID, FormatID, DestinationID, Filename, OriginalFilename, FolderPath, NOW() );

    SELECT last_insert_id();
    
END