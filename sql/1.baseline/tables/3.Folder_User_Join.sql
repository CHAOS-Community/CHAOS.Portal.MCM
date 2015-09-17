CREATE TABLE Folder_User_Join 
(
  FolderID 		int(10) unsigned NOT NULL,
  UserGUID 		binary(16) NOT NULL,
  Permission 	int(10) unsigned NOT NULL,
  DateCreated 	datetime NOT NULL,

  PRIMARY KEY (FolderID,UserGUID),
  KEY FK_Folder_ID_Folder_User_Join_FolderID (FolderID),
  CONSTRAINT FK_Folder_ID_Folder_User_Join_FolderID FOREIGN KEY (FolderID) REFERENCES Folder (ID)
) 
ENGINE=InnoDB