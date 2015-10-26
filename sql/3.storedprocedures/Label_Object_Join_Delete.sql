CREATE PROCEDURE `Label_Object_Join_Delete` 
(
	LabelId		int unsigned,
	ObjectId	binary(16)
)
BEGIN

	DELETE FROM 
		`Label_Object_Join`
	WHERE 
			`Label_Object_Join`.LabelId = LabelId
		AND `Label_Object_Join`.ObjectId = ObjectId;

	select row_count();

END
