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
-------------------------------------------------------------
--Chuyen co dau sang khoong dau
--vd: select * from tbl_City WHERE Lower(LTRIM(RTRIM(dbo.fuChuyenCoDauThanhKhongDau(City_Name)))) = 'Thanh Pho Ho Chi Minh'
-- vd: SELECT dbo.fuChuyenCoDauThanhKhongDau (N'Tạo một hàm mã hoá với T-SQL')

alter FUNCTION [dbo].[fuChuyenCoDauThanhKhongDau]
(
      @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN    
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    DECLARE @RT NVARCHAR(4000)
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
                  ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
                  ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
                  ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
                  +NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
                  iiiiiooooooooooooooouuuuuuuuuuyyyyy
                  AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
                  OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1
    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN  
      SET @COUNTER1 = 1
      --Tìm trong chuỗi mẫu
       WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
       BEGIN
     IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1))
            = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
     BEGIN          
          IF @COUNTER=1
              SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1)  
          ELSE
              SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1)
              +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
              BREAK
               END
             SET @COUNTER1 = @COUNTER1 +1
       END
      --Tìm tiếp
       SET @COUNTER = @COUNTER +1
    END
    -- neu muon replace khoang trang to - thi mo ra
    --SET @strInput = replace(@strInput,' ','-')
    RETURN @strInput
END
