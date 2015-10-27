CREATE TABLE `Project_User_Join` (
  `ProjectId` INT UNSIGNED NOT NULL COMMENT '',
  `UserId` BINARY(16) NOT NULL COMMENT '',
  `DateCreated` DATETIME NOT NULL COMMENT '',

  PRIMARY KEY (`ProjectId`, `UserId`)  COMMENT '',
  UNIQUE INDEX `PK_UNIQUE` (`ProjectId` ASC, `UserId` ASC)  COMMENT '',
  INDEX `IX_UserId` (`UserId` ASC)  COMMENT '',
  CONSTRAINT `fk_PUJ_ProjectId_P_Id`
    FOREIGN KEY (`ProjectId`)
    REFERENCES `Project` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE);
