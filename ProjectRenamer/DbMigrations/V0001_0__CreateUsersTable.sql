CREATE TABLE Users
(
  Id                   BIGINT AUTO_INCREMENT PRIMARY KEY,
  UserName             NVARCHAR(25) NOT NULL,
  Email                NVARCHAR(25) NOT NULL,
  EmailConfirmed       BIT         NOT NULL,
  PasswordHash         LONGTEXT    NOT NULL,
  OldEmail             NVARCHAR(25) NULL,
  IsDeleted            BIT         NOT NULL DEFAULT 0,
  UpdateDate           DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CreateDate           DATETIME DEFAULT CURRENT_TIMESTAMP
);