CREATE PROCEDURE `Label_Object_Join_Set` 
(
	LabelId		int unsigned,
    ObjectId	binary(16)
)
BEGIN
	
    insert into
		Label_Object_Join
        (`LabelId`, `ObjectId`, `DateCreated`)
	values
		(LabelId, ObjectId, utc_timestamp());
        
	select row_count();
    
END
