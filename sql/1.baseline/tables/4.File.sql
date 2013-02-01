CREATE TABLE File 
(
  ID                int(10) unsigned NOT NULL AUTO_INCREMENT,
  ObjectGUID        binary(16) NOT NULL,
  ParentID          int(10) unsigned DEFAULT NULL,
  FormatID          int(10) unsigned NOT NULL,
  DestinationID     int(10) unsigned NOT NULL,
  FileName          varchar(2048) NOT NULL,
  OriginalFileName  varchar(2048) NOT NULL,
  FolderPath        varchar(2048) NOT NULL,
  DateCreated       datetime NOT NULL,
  
  PRIMARY KEY (ID),
  UNIQUE KEY ID_UNIQUE (ID),
  KEY FK_File_ID_File_ParentID (ParentID),
  KEY FK_Destination_ID_File_DestinationID (DestinationID),
  KEY FK_Format_ID_File_FormatID (FormatID),
  KEY FK_Object_GUID_File_ObjectGUID (ObjectGUID),
  CONSTRAINT FK_Destination_ID_File_DestinationID FOREIGN KEY (DestinationID) REFERENCES Destination (ID),
  CONSTRAINT FK_File_ID_File_ParentID FOREIGN KEY (ParentID) REFERENCES File (ID),
  CONSTRAINT FK_Format_ID_File_FormatID FOREIGN KEY (FormatID) REFERENCES Format (ID),
  CONSTRAINT FK_Object_GUID_File_ObjectGUID FOREIGN KEY (ObjectGUID) REFERENCES Object (GUID)
) 
ENGINE=InnoDB