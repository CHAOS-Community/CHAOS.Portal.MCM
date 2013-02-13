CREATE PROCEDURE FolderInfo_Get
(
	FolderIDs	VARCHAR(21845)
)
BEGIN

	SET @sql_text := concat( 'SELECT * FROM FolderInfo WHERE ( FolderInfo.ID = ', REPLACE(FolderIDs,',',' OR FolderInfo.ID = '), '); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

END