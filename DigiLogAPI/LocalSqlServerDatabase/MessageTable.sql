﻿CREATE TABLE [dbo].[MessageTable]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Message] NVARCHAR(MAX) NULL, 
    [TimeStamp] DATETIME NULL DEFAULT GETDATE() 
)
