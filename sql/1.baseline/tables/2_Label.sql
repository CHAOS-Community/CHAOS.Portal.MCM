CREATE TABLE `Label` 
(
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '',
  `ProjectId` INT UNSIGNED NOT NULL COMMENT '',
  `Name` VARCHAR(255) NOT NULL COMMENT '',
  `DateCreated` DATETIME NOT NULL COMMENT '',

  PRIMARY KEY (`Id`),
  KEY FK_Project_Id_Label_ProjectId (`ProjectId`),
  CONSTRAINT FK_Project_Id_Label_ProjectId FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`)
)ENGINE=InnoDB;
