CREATE TABLE Destination
(
  	ID 					int(10) unsigned NOT NULL AUTO_INCREMENT,
  	SubscriptionGUID 	binary(16) NOT NULL,
  	Name 				varchar(255) NOT NULL,
  	DateCreated 		datetime NOT NULL,

  	PRIMARY KEY (ID),
  	UNIQUE KEY ID_UNIQUE (ID)
)
ENGINE=InnoDB