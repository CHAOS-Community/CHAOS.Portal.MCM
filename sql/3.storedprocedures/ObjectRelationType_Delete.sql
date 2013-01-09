CREATE PROCEDURE ObjectRelationType_Delete
(
    ID  INT
)
BEGIN

    DELETE FROM ObjectRelationType
    WHERE
    	ObjectRelationType.ID =  ID;
     
    SELECT ROW_COUNT();

END