CREATE PROCEDURE `Project_User_Join_Delete`
(
	ProjectId	int unsigned,
    UserId		binary(16)
)
BEGIN

	delete
    from
		`Project_User_Join`
	where
			`Project_User_Join`.ProjectId = ProjectId
        and `Project_User_Join`.UserId = UserId; 

	select 1;

END
