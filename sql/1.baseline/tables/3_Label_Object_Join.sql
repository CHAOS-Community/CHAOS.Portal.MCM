CREATE TABLE `Label_Object_Join` (
  `LabelId` INT UNSIGNED NOT NULL COMMENT '',
  `ObjectId` BINARY(16) NOT NULL COMMENT '',
  `DateCreated` DATETIME NOT NULL COMMENT '',
  PRIMARY KEY (`LabelId`, `ObjectId`)  COMMENT '',
  INDEX `fk_LOJ_ObjectId_O_Guid_idx` (`ObjectId` ASC)  COMMENT '',
  CONSTRAINT `fk_LOJ_LabelId_L_Id`
    FOREIGN KEY (`LabelId`)
    REFERENCES `Label` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_LOJ_ObjectId_O_Guid`
    FOREIGN KEY (`ObjectId`)
    REFERENCES `Object` (`GUID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE);
