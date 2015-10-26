CREATE PROCEDURE `Project_User_Join_Set` 
(
	ProjectId	int unsigned,
    UserId		binary(16)
)
BEGIN

	INSERT IGNORE INTO 
		`Project_User_Join`
		(`ProjectId`, `UserId`, `DateCreated`)
	VALUES
		(ProjectId, UserId, utc_timestamp());

	select 1;

END
