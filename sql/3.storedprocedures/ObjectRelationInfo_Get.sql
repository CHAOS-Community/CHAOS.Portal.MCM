CREATE PROCEDURE ObjectRelationInfo_Get
(
	Object1Guid	BINARY(16)
)
BEGIN

	SELECT
		*
	FROM
		ObjectRelationInfo AS ori
	WHERE
		ori.Object1Guid = Object1Guid;

END