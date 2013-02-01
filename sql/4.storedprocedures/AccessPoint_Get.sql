CREATE PROCEDURE AccessPoint_Get(
    AccessPointGUID BINARY(16),
    UserGUID        BINARY(16),
    GroupGUIDs      VARCHAR(21845),
    Permission      INT UNSIGNED
)
BEGIN

    DECLARE END_OF_LOOP     BOOLEAN Default false;
    DECLARE CurrentGUID     BINARY(16);
    DECLARE CurrentPosition INT Default 1;
    DECLARE Length          INT Default CHAR_LENGTH( GroupGUIDs );

    CREATE TEMPORARY TABLE IF NOT EXISTS AP_GUID_Table 
    (
        GUID    BINARY(16) NOT NULL
    );
    
    DELETE FROM AP_GUID_Table;

    simple_loop: LOOP

        SET CurrentGUID     = UNHEX( SUBSTRING( GroupGUIDs, CurrentPosition, 32 ) );
        SET CurrentPosition = CurrentPosition + 33;

        IF(CurrentGUID IS NULL) THEN
            LEAVE simple_loop;
        END IF;

        INSERT INTO AP_GUID_Table
            ( GUID )
        VALUES
            ( CurrentGUID );

          IF CurrentPosition > Length THEN
            LEAVE simple_loop;
         END IF;

    END LOOP simple_loop;

    SELECT
        AccessPoint.*
    FROM
        AccessPoint
        LEFT OUTER JOIN  AccessPoint_Group_Join  ON AccessPoint_Group_Join.AccessPointGUID = AccessPoint.GUID
        LEFT OUTER JOIN  AccessPoint_User_Join   ON AccessPoint_User_Join.AccessPointGUID  = AccessPoint.GUID
    WHERE  
        (AccessPointGUID IS NULL OR AccessPoint.GUID = AccessPointGUID ) 
        AND
        (
            (
                AccessPoint_User_Join.UserGUID = UserGUID AND
                AccessPoint_User_Join.Permission  & Permission = Permission 
            )
            OR
            (
                AccessPoint_Group_Join.GroupGUID IN ( SELECT GUID FROM AP_GUID_Table ) AND
                AccessPoint_Group_Join.Permission & Permission = Permission
            )
        );

END