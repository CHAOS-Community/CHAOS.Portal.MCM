CREATE TABLE AccessPoint_User_Join 
(
  	AccessPointGUID	binary(16) NOT NULL,
 	UserGUID 		binary(16) NOT NULL,
    Permission 		int(10) unsigned NOT NULL,
	DateCreated 	datetime NOT NULL,
	
	PRIMARY KEY (AccessPointGUID,UserGUID),
	KEY FK_AccessPoint_AccessPoint_User_Join (AccessPointGUID),
	CONSTRAINT FK_AccessPoint_AccessPoint_User_Join FOREIGN KEY (AccessPointGUID) REFERENCES AccessPoint (GUID)
) 
ENGINE=InnoDB