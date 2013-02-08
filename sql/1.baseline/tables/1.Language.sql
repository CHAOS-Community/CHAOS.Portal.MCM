CREATE TABLE Language 
(
  LanguageCode  varchar(10) NOT NULL,
  Name          varchar(255),

  PRIMARY KEY (LanguageCode),
  UNIQUE KEY LanguageCode_UNIQUE (LanguageCode)
) 
ENGINE=InnoDB