CREATE PROCEDURE `Label_Delete` 
(
	Id	int unsigned
)
BEGIN

	delete
    from
		Label
	where
		Label.Id = Id;
        
	select row_count();

END
