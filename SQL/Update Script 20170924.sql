CREATE TABLE Logs
(
	ID				bigint		  NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LogType		    VARCHAR(5)	  NOT NULL,
	Details			nvarchar(MAX)  NOT NULL,
	FriendlyMessage	nvarchar(MAX) NULL,
	Error			nvarchar(MAX) NULL,
	StackTrace		nvarchar(MAX) NULL,
	InputData		nvarchar(MAX) NULL,
	OutputData		nvarchar(MAX) NULL,
	ParentLogID		bigint		  NULL
)

CREATE TABLE Settings
(
	ID				bigint		  NOT NULL IDENTITY(1,1) PRIMARY KEY,
	KeyName			nvarchar(100) NOT NULL,
	KeyValue		nvarchar(MAX) NULL,
	KeyDataType		nvarchar(100) NULL,
	PublicSetting	bit			  NOT NULL DEFAULT 0,
)