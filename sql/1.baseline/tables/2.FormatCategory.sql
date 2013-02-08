CREATE TABLE FormatCategory 
(
  	ID 				int(10) unsigned NOT NULL AUTO_INCREMENT,
  	FormatTypeID 	int(10) unsigned NOT NULL,
  	Name 			varchar(255) NOT NULL,
  	
  	PRIMARY KEY (ID),
  	UNIQUE KEY ID_UNIQUE (ID),
  	KEY FK_FormatType_ID_FormatCategory_FormatTypeID (FormatTypeID),
	CONSTRAINT FK_FormatType_ID_FormatCategory_FormatTypeID FOREIGN KEY (FormatTypeID) REFERENCES FormatType (ID)
) 
ENGINE=InnoDB