-- RESTORE Data
alter database ConsmeticsManagement set single_user with rollback immediate waitfor delay '00:00:05'  -- wait for 5 secs
alter database ConsmeticsManagement set multi_user
USE MASTER 
RESTORE DATABASE ConsmeticsManagement 
FROM DISK='D:\Project\phan mem Hoa\management-sale20130722\9.0\Management\bin\Debug\BackupDataBase\20131016.bak'
with replace
-- Backup Data
alter database ConsmeticsManagement set single_user with rollback immediate waitfor delay '00:00:05'  -- wait for 5 secs
alter database ConsmeticsManagement set multi_user
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
------------------------------- loc
alter FUNCTION [dbo].[fLocDauTiengViet]
(
      @strInput NVARCHAR(4000)
) 
RETURNS NVARCHAR(4000)
AS
Begin
	Set @strInput=rtrim(ltrim(lower(@strInput)))
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    Declare @text nvarchar(50), @i int
    Set @text='-''`~!@#$%^&*()?><:|}{,./\"''='';–'
    Select @i= PATINDEX('%['+@text+']%',@strInput ) 
    while @i > 0
        begin
	        set @strInput = replace(@strInput, substring(@strInput, @i, 1), '')
	        set @i = patindex('%['+@text+']%', @strInput)
        End
        Set @strInput =replace(@strInput,'  ',' ')
        
    DECLARE @RT NVARCHAR(4000)
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
                  ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý'
                  +NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
                  iiiiiooooooooooooooouuuuuuuuuuyyyyy'
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1
    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN   
      SET @COUNTER1 = 1
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
       SET @COUNTER = @COUNTER +1
    End
	--SET @strInput = replace(@strInput,' ','-')
    RETURN lower(@strInput)
End
-------------------------------------ADD Foreign Key-------------------------------------------------------------------------
ALTER TABLE tbl_Comments WITH CHECK ADD
 CONSTRAINT FK_Comments_Person FOREIGN KEY (P_ID) REFERENCES tbl_Person (P_ID)
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
--------------------------------------------------------------------------2013/07/04------------------------------------------------------------------------------
-- Lay cac cot co identity 
SELECT   OBJECT_NAME(OBJECT_ID) AS TABLENAME, 
         NAME AS COLUMNNAME, 
         SEED_VALUE,
         is_identity , 
         INCREMENT_VALUE, 
         LAST_VALUE, 
         IS_NOT_FOR_REPLICATION 
FROM     SYS.IDENTITY_COLUMNS 
where SEED_VALUE > 0
and OBJECT_NAME(OBJECT_ID) = 'tbl_City'
ORDER BY 1
--lay cot nao la khoa chinh cua bang
SELECT column_name
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') =1
AND table_name = 'tbl_City'
-- lay thong tin trong table
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tbl_City'
ORDER BY ORDINAL_POSITION
------------------------------------------09-07-2013------------------------
SELECT distinct 
   --fk.name 'FK Name',
    tp.name 'Parent table',
    cp.name, cp.column_id,
    tr.name 'Refrenced table',
    cr.name, cr.column_id
FROM 
    sys.foreign_keys fk
INNER JOIN 
    sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN 
    sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN 
    sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN 
    sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
--Group by cp.column_id
ORDER BY tp.name, cp.column_id
---------------------------------------------------------------------------------------------------------------------------
-- Get ID Vua moi INSERT Trong IDENTITY
select SalaryPercent_ID
from dbo.tbl_SalaryPercents  order by SalaryPercent_ID desc
where @@ROWCOUNT > 0 and [SalaryPercent_ID] = scope_identity()
----------------------------------
-- tim kiem co dau 
SELECT * FROM M_ITEM WHERE Item_Name_EN like 'kha%' COLLATE Latin1_General_CI_AI

