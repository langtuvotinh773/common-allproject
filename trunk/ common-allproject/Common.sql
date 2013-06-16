-- RESTORE Data
USE MASTER 
RESTORE DATABASE ConsmeticsManagement 
FROM DISK='F:\Project\phan mem Hoa\ management-sale\8.0\Database\Auto BackupData\ConsmeticsManagement 2013-06-16.bak'
with replace
-- Backup Data
DECLARE @strSQL AS VARCHAR(1000) 
SET @strSQL = 'F:\Project\phan mem Hoa\ management-sale\8.0\Database\Auto BackupData\ConsmeticsManagement '
SET @strSQL += CONVERT(char(10), GetDate(),126)
SET @strSQL +='.bak'
BACKUP DATABASE ConsmeticsManagement 
TO DISK = @strSQL