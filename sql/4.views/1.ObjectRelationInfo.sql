CREATE VIEW ObjectRelationInfo AS
SELECT
	ooj.Object1Guid,
	ooj.Object2Guid,
	o1.ObjectTypeID AS Object1TypeID,
	o2.ObjectTypeID AS Object2TypeID,
	ooj.MetadataGuid,
	ooj.Sequence,
	ooj.ObjectRelationTypeID,
	ort.Name AS ObjectRelationType,
	m.LanguageCode,
	m.MetadataSchemaGUID,
	m.MetadataXML
FROM
	Object_Object_Join AS ooj
	LEFT JOIN Metadata AS m ON ooj.MetadataGuid = m.GUID
	INNER JOIN ObjectRelationType AS ort ON ooj.ObjectRelationTypeID = ort.id
	INNER JOIN Object AS o1 ON ooj.Object1Guid = o1.GUID
	INNER JOIN Object AS o2 ON ooj.Object2Guid = o2.GUID