/*  ==================================================================
This file will help get the NCIC Codes ready for consumption
on a Microsoft SQL Server database.  Any other DB will require
applicable updates to the scripts as needed.

NOTE: Execute each numbered step manually, in order before proceeding.
      ** VALIDATE RESULT BEFORE PROCEEDING TO NEXT STEP.

This run is updated for the latest NCIC codes 06/30/2026 (20260630)
IF a newer version is in play, you will need to change to the
new date pattern

Assumes will be run within the desired database scope and that
tables will be managed in the dbo schema.
====================================================================== */

-- 1: Import using Task / Flat file import 
--      Adjust all string types to nvarchar(max), and all numbers to int for now.
--      Optionally update ints to other types when actual ranges are known.
-- NOTE: it will choose the same db file (NCICCodes) as the file name automatically - that's ok

-- 2: Post import Run to clean up these erroneous Category codes...
UPDATE dbo.NCICCodes SET [CategoryCode] = null, [CategoryDescription] = null WHERE [CategoryCode] like 'Sorted%'
UPDATE dbo.NCICCodes SET [CategoryCode] = null, [CategoryDescription] = null WHERE [CategoryCode] like 'Most Fr%'

-- These are reference only and not required to maintain
DELETE FROM dbo.NCICCodes WHERE [SubSectionName] = '1 - Introduction to State and Country Field Usage' 
DELETE FROM dbo.NCICCodes WHERE [SubSectionName] = '2.3 - For Reference Only Country Codes'

-- 3: Use Pivot alone to insert into new (wide) table NCICCodesTemp
SELECT
    [CodeSet],
    [SectionName],
    [SubSectionName],
    [CategoryCode],
    [CategoryDescription],
    [1] As [Code],
    [2] As [Description],
    [3] As [Comment]
    INTO dbo.NCICCodesTemp
FROM
(
    SELECT
        [CodeSet],
        [SectionName],
        [SubSectionName],
        [CategoryCode],
        [CategoryDescription],
        [FileLineNumber],
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
    [CategoryCode], 
    [Code]

-- 4: Determine actual column sizes for new data.
SELECT MAX(LEN([CodeSet])) AS [CodeSet]
      ,MAX(LEN([SectionName])) AS [SectionName]
      ,MAX(LEN([SubSectionName])) AS [SubSectionName]
      ,MAX(LEN([CategoryCode])) AS [CategoryCode]
      ,MAX(LEN([CategoryDescription])) AS [CategoryDescription]
      ,MAX(LEN([Code])) AS [Code]
      ,MAX(LEN([Description])) AS [Description]
      ,MAX(LEN([Comment])) AS [Comment]
  FROM dbo.NCICCodesTemp

-- 5: Manually update actual column sizes (in case of change) based on step 4 output
--    Then create this table (no previous version should exist)
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.NCICCodesNew_20260630 (
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CodeSet] [nvarchar](15) NULL,
	[SectionName] [nvarchar](28) NOT NULL,
	[SubSectionName] [nvarchar](109) NOT NULL,
	[CategoryCode] [nvarchar](12) NULL,
	[CategoryDescription] [nvarchar](224) NULL,
	[Code] [nvarchar](24) NULL,
	[Description] [nvarchar](360) NULL,
	[Comment] [nvarchar](325) NULL
) ON [PRIMARY]
GO

-- 6: Run this pivot to insert new values into new wide table
INSERT INTO NCICCodesNew_20260630 (
    [CodeSet],
    [SectionName],
    [SubSectionName],
    [CategoryCode],
    [CategoryDescription],
    [Code],
    [Description],
    [Comment]
)
SELECT
    [CodeSet],
    [SectionName],
    [SubSectionName],
    [CategoryCode],
    [CategoryDescription],
    [1] As [Code],
    [2] As [Description],
    [3] As [Comment]
FROM
(
    SELECT
        [CodeSet],
        [SectionName],
        [SubSectionName],
        [CategoryCode],
        [CategoryDescription],
        [FileLineNumber],
        [Column],
        [Value]
    FROM dbo.NCICCodes
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
    [CategoryCode], 
    [Code]


-- 7 Once verified, we can begin clean up:
IF EXISTS(SELECT [TABLE_NAME] FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'NCICCodesTemp')
DROP TABLE dbo.NCICCodesTemp
GO

-- 8: Drop any existing raw legacy table for this version
IF EXISTS(SELECT [TABLE_NAME] FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'NCICCodesRaw_20260630')
DROP TABLE dbo.NCICCodesRaw_20260630
GO

-- 9: Rename the deep import table to the versioned (raw) name
EXEC sp_rename
    'NCICCodes',
    'NCICCodesRaw_20260630';
GO

-- 10: Drop any existing legacy table for this version (If applicable)
IF EXISTS(SELECT [TABLE_NAME] FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'NCICCodes_20260630')
DROP TABLE dbo.NCICCodes_20260630
GO

-- 11: Rename the newly created table to the proper version name
EXEC sp_rename
    'NCICCodesNew_20260630',
    'NCICCodes_20260630';
GO
