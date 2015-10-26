CREATE PROCEDURE `Label_Get` 
(
	ProjectId	int unsigned
)
BEGIN
	
    select
		*
	from
		Label
	where
		Label.ProjectId = ProjectId;
    
END
