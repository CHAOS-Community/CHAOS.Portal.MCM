CREATE TABLE Version
(
	ID			INT UNSIGNED NOT NULL AUTO_INCREMENT,
	Major		VARCHAR(8) NOT NULL,
	Minor		VARCHAR(8) NOT NULL,
	`Release`	VARCHAR(32) NOT NULL,
	ScriptName	VARCHAR(255) NOT NULL,
	DateCreated	DATETIME NOT NULL,
	
	PRIMARY KEY (ID),
	UNIQUE KEY uq_ID (ID)
);