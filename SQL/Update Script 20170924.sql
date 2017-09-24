CREATE TABLE Polishes_Images
(
	ID				bigint		  NOT NULL IDENTITY(1,1) PRIMARY KEY,
	PolishID		bigint	      NOT NULL,
	Image			VARCHAR(MAX)  NOT NULL,
	MIMEType		VARCHAR(50)	  NOT NULL,
	Description		nvarchar(MAX) NULL,
	Notes			nvarchar(MAX) NULL,
	MakerImage		bit			  NULL,
	PublicImage		bit			  NOT NULL DEFAULT 1,
	DisplayImage	bit			  NULL

	CONSTRAINT FK_Polishes_Images_Polish_ID FOREIGN KEY (PolishID)  REFERENCES Polishes(ID),
)

ALTER TABLE Polishes_AdditionalInfo
DROP COLUMN MakerImage, MakerImageURL, SelfImage,SelfImageURL;