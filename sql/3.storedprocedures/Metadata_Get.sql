CREATE PROCEDURE Metadata_Get
(
    Guid	BINARY(16)
)
BEGIN

	SELECT	
		*
	FROM	
		Metadata
	WHERE	
		Metadata.Guid = Guid;

END