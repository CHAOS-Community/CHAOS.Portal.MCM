CREATE PROCEDURE File_Create
(
    IN  ObjectGUID          BINARY(16),
    IN  ParentFileID        INT,
    IN  FormatID            INT,
    IN  DestinationID       INT,
    IN  Filename            VARCHAR(1024),
    IN  OriginalFilename    VARCHAR(1024),
    IN  FolderPath          VARCHAR(1024)
)
BEGIN

    INSERT INTO File
        ( ObjectGUID, ParentID,     FormatID, DestinationID, Filename, OriginalFilename, FolderPath, DateCreated )
    VALUES
        ( ObjectGUID, ParentFileID, FormatID, DestinationID, Filename, OriginalFilename, FolderPath, NOW() );

    SELECT last_insert_id();
    
END