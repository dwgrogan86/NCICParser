/*  ==========================================================
This file will help get the NCIC Codes ready for consumption
NOTE: Each of these steps are run independently - not at once..
      Validate Each as you go...

This run is assuming the NCIC codes for 03/31/2023 (20230331)
IF a newer version is in play, you will need to change to the
new date pattern
================================================================ */

-- 1: Import using Task / Flat file import (Adjust all string types to nvarchar(max), and all number to int for now...
-- NOTE: it will chose the same db file (NCICCodes) as the file name automatically - that's ok

-- 2: Post import Run to clean up these erroneous parent codes...
UPDATE [NCICCodes].[dbo].[NCICCodes] SET ParentCode = null, ParentDescription = null WHERE ParentCode like 'Sorted%'
UPDATE [NCICCodes].[dbo].[NCICCodes] SET ParentCode = null, ParentDescription = null WHERE ParentCode like 'Most Fr%'

-- 3: Use Pivot alone to insert into new (wide) table NCICCodesTemp
SELECT
    CodeSet,
    SectionName,
    SubSectionName,
    ParentCode,
    ParentDescription,
    [1] As [Code],
    [2] As [Description],
    [3] As [Comment]
    INTO NCICCodesTemp
FROM
(
    SELECT
        CodeSet,
        SectionName,
        SubSectionName,
        ParentCode,
        ParentDescription,
        FileLineNumber,
        [Column],
        [Value]
    FROM NCICCodes
) src
PIVOT
(
    MAX([Value])
    FOR [Column] IN
    (
        [1],
        [2],
        [3]
    )
) pvt
ORDER BY
    [SectionName], 
    [SubSectionName], 
    [ParentCode], 
    [Code]


-- 4: Determine actual column sizes (in case of change)
SELECT TOP (1000) 
      MAX(LEN([CodeSet])) AS [CodeSet]
      ,MAX(LEN([SectionName])) AS [SectionName]
      ,MAX(LEN([SubSectionName])) AS [SubSectionName]
      ,MAX(LEN([ParentCode])) AS [ParentCode]
      ,MAX(LEN([ParentDescription])) AS [ParentDescription]
      ,MAX(LEN([Code])) AS [Code]
      ,MAX(LEN([Description])) AS [Description]
      ,MAX(LEN([Comment])) AS [Comment]
  FROM [NCICCodes].[dbo].[NCICCodesTemp]

-- 5: Adjust column widths and create this table
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NCICCodes_20230331](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CodeSet] [nvarchar](15) NULL,
	[SectionName] [nvarchar](28) NOT NULL,
	[SubSectionName] [nvarchar](109) NOT NULL,
	[ParentCode] [nvarchar](12) NULL,
	[ParentDescription] [nvarchar](224) NULL,
	[Code] [nvarchar](24) NULL,
	[Description] [nvarchar](360) NULL,
	[Comment] [nvarchar](325) NULL
) ON [PRIMARY]
GO

-- 6: Drop Temp [wide] Table - not needed any more
DROP TABLE NCICCodesTemp

-- 7: Rename the deep import table to the versioned name
EXEC sp_rename
    'NCICCodes',
    'NCICCodesRaw_20230331';

-- 8: Run this pivot to insert new values into new wide table
INSERT INTO NCICCodes_20230331(
    CodeSet,
    SectionName,
    SubSectionName,
    ParentCode,
    ParentDescription,
    [Code],
    [Description],
    [Comment]
)
SELECT
    CodeSet,
    SectionName,
    SubSectionName,
    ParentCode,
    ParentDescription,
    [1] As [Code],
    [2] As [Description],
    [3] As [Comment]
FROM
(
    SELECT
        CodeSet,
        SectionName,
        SubSectionName,
        ParentCode,
        ParentDescription,
        FileLineNumber,
        [Column],
        [Value]
    FROM NCICCodesRaw_20230331
) src
PIVOT
(
    MAX([Value])
    FOR [Column] IN
    (
        [1],
        [2],
        [3]
    )
) pvt
ORDER BY
    [SectionName], 
    [SubSectionName], 
    [ParentCode], 
    [Code]
