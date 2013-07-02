-- RESTORE Data
DECLARE @strSQL AS VARCHAR(1000) 
SET @strSQL = 'F:\Project\phan mem Hoa\ management-sale\8.0\Database\Auto BackupData\ConsmeticsManagement '
SET @strSQL += CONVERT(char(10), GetDate(),126)
SET @strSQL +='.bak'
BACKUP DATABASE ConsmeticsManagement 
TO DISK = @strSQL
-------------------------------------------
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
-- Foreach Table
CREATE PROCEDURE [dbo].[testSet]
AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @NumberofIntType            int,
            @RowCount                   int

    -- get the number of items
    SET @NumberofIntType = (SELECT  count(*)
                            FROM dbo.IntType)

    SET @RowCount = 0           -- set the first row to 0

    -- loop through the records 
    -- loop until the rowcount = number of records in your table
    WHILE @RowCount <= @NumberIntType
        BEGIN
            -- do your process here

            SET @RowCount = @RowCount + 1
        END
END
---------
SELECT 
    RowNum = ROW_NUMBER() OVER(ORDER BY CustomerID)
    ,*
INTO #Customers
FROM SalesLT.Customer

DECLARE @MaxRownum INT
SET @MaxRownum = (SELECT MAX(RowNum) FROM #Customers)

DECLARE @Iter INT
SET @Iter = (SELECT MIN(RowNum) FROM #Customers)

WHILE @Iter <= @MaxRownum
BEGIN
    SELECT *
    FROM #Customers
    WHERE RowNum = @Iter
    
    -- run your operation here
    
    SET @Iter = @Iter + 1
END