INSERT INTO UserRoles (RoleName) VALUES ("admin");

SET @AdminRoleId := LAST_INSERT_ID();

--password: 123
INSERT INTO Users
(UserName, Email, EmailConfirmed, PasswordHash, OldEmail, IsDeleted) VALUES
('admin', 'admin@admin.com', 0, '40-BD-00-15-63-08-5F-C3-51-65-32-9E-A1-FF-5C-5E-CB-DB-BE-EF', NULL , 0);

SET @AdminUserId := LAST_INSERT_ID();


INSERT INTO  UserClaims (UserId, UserRoleId, IsDeleted) VALUES (@AdminUserId, @AdminRoleId, 0);

