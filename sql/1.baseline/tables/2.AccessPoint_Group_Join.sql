CREATE TABLE AccessPoint_Group_Join 
(
  AccessPointGUID	binary(16) NOT NULL,
  GroupGUID 		binary(16) NOT NULL,
  Permission 		int(10) unsigned NOT NULL,
  DateCreated 		datetime NOT NULL,
  
  PRIMARY KEY (AccessPointGUID, GroupGUID), 
  KEY FK_AccessPoint_AccessPoint_Group_Join (AccessPointGUID),
  CONSTRAINT FK_AccessPoint_AccessPoint_Group_Join FOREIGN KEY (AccessPointGUID) REFERENCES AccessPoint (GUID)
) 
ENGINE=InnoDB