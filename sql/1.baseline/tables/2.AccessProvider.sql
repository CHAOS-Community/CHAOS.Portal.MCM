CREATE TABLE AccessProvider   
(
    ID              int(10) unsigned NOT NULL AUTO_INCREMENT,
    DestinationID   int(10) unsigned NOT NULL,
    BasePath        varchar(2048) NOT NULL,
    StringFormat    varchar(2048) NOT NULL,
    DateCreated     datetime NOT NULL,
    Token           varchar(255) NOT NULL,
  
    PRIMARY KEY (ID),
    UNIQUE KEY ID_UNIQUE (ID),
    KEY FK_Destination_ID_AccessProvider_DestinationID (DestinationID),
    CONSTRAINT FK_Destination_ID_AccessProvider_DestinationID FOREIGN KEY (DestinationID) REFERENCES Destination (ID)
) 
ENGINE=InnoDB