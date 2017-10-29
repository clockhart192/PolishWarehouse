--DROP TABLE IncomingOrderLines
--DROP TABLE IncomingOrderLines_Polishes
--DROP TABLE IncomingOrders


--DROP TABLE IncomingLineTypes
--DROP TABLE ShippingProviders

--DROP TABLE Polishes_Secondary_Colors_ARCHIVE
--DROP TABLE Polishes_Glitter_Colors_ARCHIVE
--DROP TABLE Polishes_PolishTypes_ARCHIVE
--DROP TABLE Polishes_AdditionalInfo_ARCHIVE
--DROP TABLE Polishes_Images_ARCHIVE
--DROP TABLE Polishes_DestashInfo_ARCHIVE
--DROP TABLE Polishes_ARCHIVE

CREATE TABLE IncomingOrders
(
	ID					bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Timestamp			timestamp	  NOT NULL,
	CreatedOn			datetime	  NOT NULL DEFAULT GETUTCDATE(),
	Name				nvarchar(100) NOT NULL,
	PurchaseDate		datetime	  NOT NULL,
	Notes				nvarchar(MAX) NULL,
	Tracking			nvarchar(MAX) NULL,
	TrackingProviderID  int			  NULL,
	Price			    Money		  NOT NULL
)

CREATE TABLE IncomingLineTypes
(
	ID			bigint			  IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Timestamp	timestamp		  NOT NULL,
	CreatedOn	datetime		  NOT NULL DEFAULT GETUTCDATE(),
	Name		nvarchar(100)     NOT NULL,
)

CREATE TABLE ShippingProviders
(
	ID				int			  IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Timestamp		timestamp	  NOT NULL,
	CreatedOn		datetime	  NOT NULL DEFAULT GETUTCDATE(),
	Name			nvarchar(100) NOT NULL,
	TrackingBaseURL	nvarchar(MAX) NULL,
)

CREATE TABLE IncomingOrderLines
(
	ID					bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Timestamp			timestamp	  NOT NULL,
	CreatedOn			datetime	  NOT NULL DEFAULT GETUTCDATE(),
	IncomingOrderID		bigint		  NOT NULL,
	IncomingLineTypeID  bigint		  NOT NULL,
	Name				varchar(100)  NULL,
	Price				Money		  NOT NULL,
	Qty					int			  NOT NULL DEFAULT 1,
	Notes				nvarchar(MAX) NULL,
	Tracking			nvarchar(MAX) NULL,
	ShippingProviderID  int			  NULL,

	 CONSTRAINT FK_Incoming_Order_ID FOREIGN KEY (IncomingOrderID)  REFERENCES IncomingOrders(ID),
	 CONSTRAINT FK_Incoming_Line_Type_ID FOREIGN KEY (IncomingLineTypeID)  REFERENCES IncomingLineTypes(ID),
	 CONSTRAINT FK_Shipping_Provider_ID FOREIGN KEY (ShippingProviderID)  REFERENCES ShippingProviders(ID),
)

CREATE TABLE IncomingOrderLines_Polishes
(
	ID					  bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Timestamp			  timestamp		  NOT NULL,
	CreatedOn			  datetime		  NOT NULL DEFAULT GETUTCDATE(),
	IncomingOrderLinesID  bigint		  NOT NULL,
	ColorID				  int			  NULL,
	BrandID				  int			  NULL,
	PolishName			  nvarchar(100)   NOT NULL,
	Coats				  int			  NOT NULL DEFAULT 1,
	HasBeenTried		  bit			  NOT NULL DEFAULT 0,
	WasGift				  bit		      NOT NULL DEFAULT 0,
	GiftFromName		  nvarchar(100)   NULL,
	Description			  varchar(max)    NULL,
	PolishID			  bigint		  NULL,
	Converted			  bit			  NOT NULL DEFAULT 0

	CONSTRAINT FK_Incoming_Order_Lines_ID FOREIGN KEY (IncomingOrderLinesID)  REFERENCES IncomingOrderLines(ID),
	CONSTRAINT FK_Incoming_Order_Lines_Color_ID FOREIGN KEY (ColorID)  REFERENCES Colors(ID),
    CONSTRAINT FK_Incoming_Order_Lines_Brand_ID FOREIGN KEY (BrandID)  REFERENCES Brands(ID),
)

ALTER TABLE IncomingOrders
ADD  CONSTRAINT FK_IncomingOrders_Shipping_Provider_ID FOREIGN KEY (TrackingProviderID)  REFERENCES ShippingProviders(ID);


SET IDENTITY_INSERT [dbo].[IncomingLineTypes] ON 
GO
INSERT [dbo].[IncomingLineTypes] ([ID], [CreatedOn], [Name]) VALUES (1, CAST(N'2017-10-08T17:00:17.870' AS DateTime), N'Nail Polish')
GO
INSERT [dbo].[IncomingLineTypes] ([ID], [CreatedOn], [Name]) VALUES (2, CAST(N'2017-10-08T17:00:20.783' AS DateTime), N'Other')
GO
SET IDENTITY_INSERT [dbo].[IncomingLineTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ShippingProviders] ON 
GO
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (1, CAST(N'2017-10-08T16:59:32.367' AS DateTime), N'USPS', N'https://tools.usps.com/go/TrackConfirmAction_input?qtc_tLabels1=')
GO
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (2, CAST(N'2017-10-08T16:59:35.790' AS DateTime), N'Fedex', N'https://www.fedex.com/apps/fedextrack/?action=track&action=track&tracknumbers=')
GO
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (3, CAST(N'2017-10-08T16:59:37.373' AS DateTime), N'UPS', N'https://wwwapps.ups.com/tracking/tracking.cgi?tracknum=')
GO
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (4, CAST(N'2017-10-08T16:59:39.637' AS DateTime), N'DHL', N'http://webtrack.dhlglobalmail.com/?trackingnumber=')
GO
SET IDENTITY_INSERT [dbo].[ShippingProviders] OFF
GO

ALTER TABLE IncomingOrders
ADD OrderComplete BIT NOT NULL DEFAULT 0


CREATE TABLE Polishes_ARCHIVE
(
	ID				bigint	      NOT NULL PRIMARY KEY,
	ColorID			int			  NOT NULL,
	BrandID			int			  NOT NULL,
	Timestamp		timestamp	  NOT NULL,
	CreatedOn		datetime	  NOT NULL DEFAULT GETUTCDATE(),
	LastEditedOn	datetime	  NULL,
	Name			nvarchar(100) NOT NULL,
	ColorNumber	    int			  NOT NULL,
	Label			nvarchar(100) NOT NULL,
	Coats		    int			  NOT NULL DEFAULT 1,
	Quantity	    int			  NOT NULL DEFAULT 1,
	HasBeenTried	bit			  NOT NULL DEFAULT 0,
	WasGift			bit		      NOT NULL DEFAULT 0,
	--Destash			bit		      NOT NULL DEFAULT 0,
)

CREATE TABLE Polishes_AdditionalInfo_ARCHIVE
(
	PolishID		bigint	      NOT NULL PRIMARY KEY,
	Description		nvarchar(MAX) NULL,
	Notes			nvarchar(MAX) NULL,
	GiftFromName	nvarchar(100) NULL,
)

CREATE TABLE Polishes_Images_ARCHIVE
(
	ID				bigint		  NOT NULL PRIMARY KEY,
	PolishID		bigint	      NOT NULL,
	Image			VARCHAR(MAX)  NOT NULL,
	MIMEType		VARCHAR(50)	  NOT NULL,
	Description		nvarchar(MAX) NULL,
	Notes			nvarchar(MAX) NULL,
	MakerImage		bit			  NULL,
	PublicImage		bit			  NOT NULL DEFAULT 1,
	DisplayImage	bit			  NULL,
	[CompressedImage] [varchar](max) NULL,
	[CompressedMIMEType] [varchar](50) NULL,
	[ImagePath] [varchar](max) NULL,
	[CompressedImagePath] [varchar](max) NULL,
)

CREATE TABLE [dbo].[Polishes_DestashInfo_ARCHIVE](
	[PolishID] [bigint] NOT NULL PRIMARY KEY,
	[Qty] [int] NOT NULL,
	[BuyerName] [nvarchar](50) NULL,
	[AskingPrice] [money] NULL,
	[SoldPrice] [money] NULL,
	[TrackingNumber] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[InternalNotes] [nvarchar](max) NULL,
	[SaleStatus] [nvarchar](5) NULL,
)



CREATE TABLE Polishes_PolishTypes_ARCHIVE
(
	ID				bigint	      NOT NULL PRIMARY KEY,
	PolishID		bigint		  NOT NULL,
	PolishTypeID	int			  NOT NULL,
)

CREATE TABLE Polishes_Secondary_Colors_ARCHIVE
(
	ID				bigint	      NOT NULL PRIMARY KEY,
	PolishID		bigint		  NOT NULL,
	ColorID			int			  NOT NULL,
)

CREATE TABLE Polishes_Glitter_Colors_ARCHIVE
(
	ID				bigint	      NOT NULL PRIMARY KEY,
	PolishID		bigint		  NOT NULL,
	ColorID			int			  NOT NULL,
)