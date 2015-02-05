


IF(OBJECT_ID('dbo.Log') IS NOT NULL)
	DROP TABLE dbo.Log
		
CREATE TABLE dbo.Log ( 
	LogId 					INT					NOT NULL		IDENTITY(1, 1)
,	[Date] 					DATETIME 			NOT NULL  
,	[Thread] 				NVARCHAR(255) 		NOT NULL 
,	[Level] 				NVARCHAR(20) 		NOT NULL
,	[Logger] 				NVARCHAR(255) 		NOT NULL
,	[Message] 				NVARCHAR(4000) 		NOT NULL 
,	[StackTrace] 			NVARCHAR(4000) 			NULL 
)

ALTER TABLE dbo.Log
ADD CONSTRAINT PK_Log
PRIMARY KEY(LogId)