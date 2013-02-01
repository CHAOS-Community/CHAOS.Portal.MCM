CREATE PROCEDURE Object_GetByRelatedObjectGUID
(
    RelatedObjectGUID       BINARY(16),
    ObjectRelationTypeID    INT UNSIGNED,
    IncludeMetadata         BOOLEAN,
    IncludeFiles            BOOLEAN,
    IncludeFolders          BOOLEAN,
    IncludeAccessPoints     BOOLEAN
)
BEGIN

SELECT
    Object.*
FROM
    Object
    JOIN Object_Object_Join ON Object.GUID = Object_Object_Join.Object2GUID 
WHERE
    Object_Object_Join.Object1GUID = RelatedObjectGUID
    AND (ObjectRelationTypeID IS NULL OR Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID);

IF( IncludeMetadata = 1 ) THEN
    SELECT
        M1.*
    FROM
        Object_Object_Join
        JOIN  Metadata AS M1 ON Object_Object_Join.Object2GUID = M1.ObjectGUID AND
                                            M1.GUID = ( SELECT GUID
                                                          FROM Metadata AS M2
                                                         WHERE M2.ObjectGUID         = M1.ObjectGUID   AND
                                                               M2.LanguageCode       = M1.LanguageCode AND
                                                               M2.MetadataSchemaGUID = M1.MetadataSchemaGUID
                                                         ORDER BY M2.RevisionID DESC
                                                         LIMIT 1)
    WHERE
        Object_Object_Join.Object1GUID = RelatedObjectGUID
        AND (ObjectRelationTypeID IS NULL OR Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID);
END IF;

IF( IncludeFiles = 1 ) THEN
    SELECT
        FileInfo.*
    FROM
        Object_Object_Join
        JOIN FileInfo ON Object_Object_Join.Object2GUID = FileInfo.ObjectGUID
    WHERE
        Object_Object_Join.Object1GUID = RelatedObjectGUID
        AND (ObjectRelationTypeID IS NULL OR Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID);
END IF;
            
IF( IncludeFolders = 1 ) THEN
    SELECT
        Object_Folder_Join.*
    FROM
        Object_Object_Join
        INNER JOIN Object_Folder_Join ON Object_Object_Join.Object2GUID = Object_Folder_Join.ObjectGUID
    WHERE
        Object_Object_Join.Object1GUID = RelatedObjectGUID
        AND (ObjectRelationTypeID IS NULL OR Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID);
END IF;

IF( IncludeAccessPoints = 1 ) THEN
    SELECT
        AccessPoint_Object_Join.*
    FROM
        Object_Object_Join
        JOIN AccessPoint_Object_Join ON Object_Object_Join.Object2GUID = AccessPoint_Object_Join.ObjectGUID
    WHERE
        Object_Object_Join.Object1GUID = RelatedObjectGUID
        AND (ObjectRelationTypeID IS NULL OR Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID);
END IF;

END