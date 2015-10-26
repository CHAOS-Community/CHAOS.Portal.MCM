CREATE PROCEDURE `Label_Set` 
(
	Id		int unsigned,
	ProjectId	int unsigned,
	Name		varchar(255)
)
BEGIN

	insert into 
		`Label`
		(`Id`,`ProjectId`,`Name`,`DateCreated`)
	values
		(Id, ProjectId, Name, utc_timestamp())
	on duplicate key update
		Label.Name = Name;

	-- Return currect Id
	if Id is null then
		select LAST_INSERT_ID();
	else
		select Id;
	end if;

END
