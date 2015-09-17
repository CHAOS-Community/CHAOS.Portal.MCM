CREATE TABLE AccessPoint_Object_Join 
(
    AccessPointGUID   binary(16) NOT NULL,
    ObjectGUID        binary(16) NOT NULL,
    StartDate         datetime DEFAULT NULL,
    EndDate           datetime DEFAULT NULL,
    DateCreated       datetime NOT NULL,
    DateModified      datetime DEFAULT NULL,
  
    PRIMARY KEY (AccessPointGUID,ObjectGUID),
    KEY FK_AccessPoint_AccessPoint_Object_Join (AccessPointGUID),
    KEY FK_Object_GUID_AccessPoint_Object_Join_ObjectGUID (ObjectGUID),
    CONSTRAINT FK_AccessPoint_AccessPoint_Object_Join FOREIGN KEY (AccessPointGUID) REFERENCES AccessPoint (GUID),
    CONSTRAINT FK_Object_GUID_AccessPoint_Object_Join_ObjectGUID FOREIGN KEY (ObjectGUID) REFERENCES Object (GUID)
) 
ENGINE=InnoDB