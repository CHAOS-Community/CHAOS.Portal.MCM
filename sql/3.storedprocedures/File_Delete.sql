DROP PROCEDURE IF EXISTS File_Delete;

DELIMITER $$

CREATE PROCEDURE File_Delete
(
    ID  int unsigned
)
BEGIN

    DELETE
      FROM  File
     WHERE  File.ID = ID;

    SELECT  ROW_COUNT();

END