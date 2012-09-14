DROP PROCEDURE IF EXISTS Format_Get;

DELIMITER $$

CREATE DEFINER=`root`@`%` PROCEDURE `Format_Get`(
    IN  ID      INT,
    IN  Name    VARCHAR(255)
)
BEGIN

    SELECT  *
      FROM  Format
     WHERE  ( ID   IS NULL OR Format.ID = ID ) AND
            ( Name IS NULL OR Format.Name = Name );
END