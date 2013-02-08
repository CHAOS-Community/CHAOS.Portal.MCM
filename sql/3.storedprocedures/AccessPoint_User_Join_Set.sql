CREATE PROCEDURE AccessPoint_User_Join_Set
(
    AccessPointGUID BINARY(16), 
    UserGUID        BINARY(16), 
    Permission      INT UNSIGNED
)
BEGIN

    IF( SELECT 
          COUNT(*)
        FROM 
          AccessPoint_User_Join AS AUJ
        WHERE 
          AUJ.AccessPointGUID = AccessPointGUID AND
          AUJ.UserGUID        = UserGUID ) = 0 
    THEN

        INSERT INTO AccessPoint_User_Join
          ( AccessPointGUID, UserGUID, Permission, DateCreated ) 
        VALUES
          ( AccessPointGUID, UserGUID, Permission, NOW() );

    ELSE
        UPDATE 
          AccessPoint_User_Join AS AUJ
        SET 
          AUJ.Permission = Permission
        WHERE 
          AUJ.AccessPointGUID = AccessPointGUID AND
          AUJ.UserGUID        = UserGUID;

    END IF;

    SELECT ROW_COUNT();


END