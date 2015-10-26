CREATE PROCEDURE `Label_Get`(
	ProjectId	int unsigned,
	ObjectId	binary(16)
)
BEGIN
	
	select distinct
		Label.*
	from
		Label
	left join 
		Label_Object_Join as LOJ on Label.Id = LOJ.LabelId
	where
			(ProjectId is null or Label.ProjectId = ProjectId)
		and (ObjectId is null or LOJ.ObjectId = ObjectId);
    
END