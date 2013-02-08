CREATE TABLE Object_Folder_Join 
(
  ObjectGUID          binary(16) NOT NULL,
  FolderID            int(10) unsigned NOT NULL,
  ObjectFolderTypeID  int(10) unsigned NOT NULL,
  DateCreated         datetime NOT NULL,

  PRIMARY KEY (ObjectGUID,FolderID),
  KEY FK_Object_GUID_Object_Folder_Join_ObjectGUID (ObjectGUID),
  KEY FK_Folder_ID_Object_Folder_Join_FolderID (FolderID),
  KEY FK_ObjectFolderType_ID_Object_Folder_Join_ObjectFolderTypeID (ObjectFolderTypeID),
  CONSTRAINT FK_Folder_ID_Object_Folder_Join_FolderID FOREIGN KEY (FolderID) REFERENCES Folder (ID),
  CONSTRAINT FK_ObjectFolderType_ID_Object_Folder_Join_ObjectFolderTypeID FOREIGN KEY (ObjectFolderTypeID) REFERENCES ObjectFolderType (ID),
  CONSTRAINT FK_Object_GUID_Object_Folder_Join_ObjectGUID FOREIGN KEY (ObjectGUID) REFERENCES Object (GUID)
) 
ENGINE=InnoDB