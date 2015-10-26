create procedure `Project_Set` 
(
	Id		int unsigned,
    Name	varchar(255)
)
begin

	insert into
		Project
		(`Id`, `Name`,`DateCreated`)
	values
		(Id, Name, UTC_TIMESTAMP())
	on duplicate key update 
		Project.Name = Name;
	
    -- Return currect Id
	if Id is null then
		select LAST_INSERT_ID();
	else
		select Id;
	end if;
    
end
