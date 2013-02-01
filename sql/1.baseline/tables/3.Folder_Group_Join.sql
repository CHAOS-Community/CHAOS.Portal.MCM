CREATE TABLE Folder_Group_Join 
(
  FolderID 		int(10) unsigned NOT NULL,
  GroupGUID 	binary(16) NOT NULL,
  Permission 	int(10) unsigned zerofill NOT NULL,
  DateCreated 	datetime NOT NULL,

  PRIMARY KEY (FolderID,GroupGUID),
  KEY FK_Folder_ID_Folder_Group_Join_FolderID (FolderID),
  CONSTRAINT FK_Folder_ID_Folder_Group_Join_FolderID FOREIGN KEY (FolderID) REFERENCES Folder (ID)
) 
ENGINE=InnoDB