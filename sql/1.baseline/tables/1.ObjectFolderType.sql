CREATE TABLE ObjectFolderType 
(
  ID 	int(10) unsigned NOT NULL,
  Name	varchar(255) NOT NULL,

  PRIMARY KEY (ID),
  UNIQUE KEY ID_UNIQUE (ID)
) 
ENGINE=InnoDB