CREATE PROCEDURE `Project_Delete` 
(
	Id	int unsigned
)
BEGIN

	delete
    from
		Project
	where
		Project.Id = Id;
        
	select row_count();

END
