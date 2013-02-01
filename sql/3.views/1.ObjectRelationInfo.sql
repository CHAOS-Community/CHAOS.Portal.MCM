CREATE VIEW ObjectRelationInfo AS
SELECT
	ooj.Object1Guid,
	ooj.Object2Guid,
	ooj.MetadataGuid,
	ooj.Sequence,
	ort.Name AS ObjectRelationType,
	m.LanguageCode,
	m.MetadataSchemaGUID,
	m.MetadataXML
FROM
	Object_Object_Join AS ooj
	INNER JOIN Metadata AS m ON ooj.MetadataGuid = m.GUID
	INNER JOIN ObjectRelationType AS ort ON ooj.ObjectRelationTypeID = ort.id