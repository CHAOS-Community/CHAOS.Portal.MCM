CREATE TABLE FolderType 
(
  ID 			int(10) unsigned NOT NULL AUTO_INCREMENT,
  Name 			varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  DateCreated 	datetime NOT NULL,

  PRIMARY KEY (ID),
  UNIQUE KEY ID_UNIQUE (ID)
) 
ENGINE=InnoDB