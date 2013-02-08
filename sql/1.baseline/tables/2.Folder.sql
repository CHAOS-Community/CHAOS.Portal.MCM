CREATE TABLE Folder 
(
  ID                int(10) unsigned NOT NULL AUTO_INCREMENT,
  ParentID          int(10) unsigned DEFAULT NULL,
  FolderTypeID      int(10) unsigned NOT NULL,
  SubscriptionGUID  binary(16) DEFAULT NULL,
  Name              varchar(255) NOT NULL,
  DateCreated       datetime NOT NULL,

  PRIMARY KEY (ID),
  UNIQUE KEY ID_UNIQUE (ID),
  KEY FK_Folder_ID_Folder_ParentID (ParentID),
  KEY FK_FolderType_ID_Folder_FolderTypeID (FolderTypeID),
  CONSTRAINT FK_FolderType_ID_Folder_FolderTypeID FOREIGN KEY (FolderTypeID) REFERENCES FolderType (ID),
  CONSTRAINT FK_Folder_ID_Folder_ParentID FOREIGN KEY (ParentID) REFERENCES Folder (ID)
) 
ENGINE=InnoDB