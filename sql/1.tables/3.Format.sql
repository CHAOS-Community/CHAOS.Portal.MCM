CREATE TABLE Format 
(
  ID                int(10) unsigned NOT NULL AUTO_INCREMENT,
  FormatCategoryID  int(10) unsigned NOT NULL,
  Name              varchar(255) NOT NULL,
  FormatXML         text,
  MimeType          varchar(255) NOT NULL,
  Extension         varchar(255),
  
  PRIMARY KEY (ID),
  UNIQUE KEY ID_UNIQUE (ID),
  KEY FK_FormatCategory_ID_Format_FormatCategoryID (FormatCategoryID),
  CONSTRAINT FK_FormatCategory_ID_Format_FormatCategoryID FOREIGN KEY (FormatCategoryID) REFERENCES FormatCategory (ID)
) 
ENGINE=InnoDB