CREATE PROCEDURE AccessPoint_Object_Join_Set
(
    AccessPointGuid BINARY(16),
    ObjectGuid      BINARY(16),
    StartDate       DATETIME,
    EndDate         DATETIME
)
BEGIN

    IF( SELECT 
          COUNT(*)
        FROM 
          AccessPoint_Object_Join AS AOJ
        WHERE 
          AOJ.AccessPointGUID = AccessPointGUID AND
          AOJ.ObjectGUID      = ObjectGUID ) = 0 
    THEN

        INSERT INTO AccessPoint_Object_Join
          (AccessPointGUID, ObjectGUID, StartDate, EndDate, DateCreated, DateModified)
        VALUES
          (AccessPointGuid, ObjectGuid, StartDate, EndDate, NOW(), null );

    ELSE

        UPDATE 
          AccessPoint_Object_Join AS AOJ
        SET 
          AOJ.StartDate = StartDate,
          AOJ.EndDate   = EndDate
        WHERE 
          AOJ.AccessPointGUID = AccessPointGuid AND
          AOJ.ObjectGUID      = ObjectGuid;

    END IF;

    SELECT ROW_COUNT();

END