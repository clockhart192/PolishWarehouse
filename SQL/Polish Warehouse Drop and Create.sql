--Drop Everything.
DROP TABLE Polishes_Extra_Colors
DROP TABLE Polishes_PolishTypes
DROP TABLE Polishes_AdditionalInfo
DROP TABLE Polishes
DROP TABLE Brands
DROP TABLE Colors
DROP TABLE PolishTypes

--Recreate Everything
CREATE TABLE PolishTypes
(
	ID				int			  IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Name			nvarchar(100) NOT NULL,
	Description		nvarchar(MAX) NULL,
)

CREATE TABLE Colors
(
	ID				int			  IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Name			nvarchar(100) NOT NULL,
	Description		nvarchar(MAX) NULL,
	IsPrimary		bit			  NOT NULL DEFAULT 0,
	IsSecondary		bit			  NOT NULL DEFAULT 0,
	IsGlitter		bit			  NOT NULL DEFAULT 0,
)

CREATE TABLE Brands
(
	ID				int			  IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Name			nvarchar(100) NOT NULL,
	Description		nvarchar(MAX) NULL,
)

CREATE TABLE Polishes
(
	ID				bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
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

  CONSTRAINT FK_Polishes_Color_ID FOREIGN KEY (ColorID)  REFERENCES Colors(ID),
  CONSTRAINT FK_Polishes_Brand_ID FOREIGN KEY (BrandID)  REFERENCES Brands(ID),
)

CREATE TABLE Polishes_AdditionalInfo
(
	PolishID		bigint	      NOT NULL PRIMARY KEY,
	Description		nvarchar(MAX) NULL,
	Notes			nvarchar(MAX) NULL,
	MakerImage		nvarchar(MAX) NULL,
	MakerImageURL	nvarchar(MAX) NULL,
	SelfImage		nvarchar(MAX) NULL,
	SelfImageURL	nvarchar(MAX) NULL,
	GiftFromName	nvarchar(100) NULL,

	CONSTRAINT FK_Polishes_AdditionalInfo_Polish_ID FOREIGN KEY (PolishID)  REFERENCES Polishes(ID),
)



CREATE TABLE Polishes_PolishTypes
(
	ID				bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
	PolishID		bigint		  NOT NULL,
	PolishTypeID	int			  NOT NULL,
	
	CONSTRAINT FK_Polishes_PolishTypes_Polish_ID FOREIGN KEY (PolishID)  REFERENCES Polishes(ID),
	CONSTRAINT FK_Polishes_PolishTypes_Polish_Type_ID FOREIGN KEY (PolishTypeID)  REFERENCES PolishTypes(ID),
)

CREATE TABLE Polishes_Extra_Colors
(
	ID				bigint	      IDENTITY (1,1) NOT NULL PRIMARY KEY,
	PolishID		bigint		  NOT NULL,
	ColorID			int			  NOT NULL,
	
	CONSTRAINT FK_Polishes_Extra_Colors_Polish_ID FOREIGN KEY (PolishID)  REFERENCES Polishes(ID),
	CONSTRAINT FK_Polishes_Extra_Colors_Color_ID FOREIGN KEY (ColorID)  REFERENCES Colors(ID),
)


--Insert Base data.
INSERT INTO Brands (Name) Values('A England');
INSERT INTO Brands (Name) Values('Anchor & Heart');
INSERT INTO Brands (Name) Values('Anonymous Lacquer');
INSERT INTO Brands (Name) Values('Barielle');
INSERT INTO Brands (Name) Values('Baroness X');
INSERT INTO Brands (Name) Values('Bear Pawlish');
INSERT INTO Brands (Name) Values('Black Cat Lacquer');
INSERT INTO Brands (Name) Values('BLN');
INSERT INTO Brands (Name) Values('Blush Lacquers');
INSERT INTO Brands (Name) Values('Bohemian Polish');
INSERT INTO Brands (Name) Values('Catrice');
INSERT INTO Brands (Name) Values('CDB Lacquer');
INSERT INTO Brands (Name) Values('Chanel');
INSERT INTO Brands (Name) Values('Chaos and Crocodiles');
INSERT INTO Brands (Name) Values('China Glaze');
INSERT INTO Brands (Name) Values('Christian Louboutin');
INSERT INTO Brands (Name) Values('Cirque');
INSERT INTO Brands (Name) Values('Color4Nails + Celestial');
INSERT INTO Brands (Name) Values('Colores de Carol');
INSERT INTO Brands (Name) Values('Colors By Llarowe');
INSERT INTO Brands (Name) Values('Contrary Polish');
INSERT INTO Brands (Name) Values('Crows Toes');
INSERT INTO Brands (Name) Values('Cult Nails');
INSERT INTO Brands (Name) Values('Cupcake Polish');
INSERT INTO Brands (Name) Values('Darling Diva Polish');
INSERT INTO Brands (Name) Values('Different Dimension');
INSERT INTO Brands (Name) Values('Digital Nails');
INSERT INTO Brands (Name) Values('Dollish Polish');
INSERT INTO Brands (Name) Values('Dreamland Lacquer/Smitten');
INSERT INTO Brands (Name) Values('Elevation Polish');
INSERT INTO Brands (Name) Values('Ellagee');
INSERT INTO Brands (Name) Values('Emerald and Ash');
INSERT INTO Brands (Name) Values('Enchanted Polish');
INSERT INTO Brands (Name) Values('Essie');
INSERT INTO Brands (Name) Values('Ever After');
INSERT INTO Brands (Name) Values('Fair Maiden');
INSERT INTO Brands (Name) Values('Fancy Gloss');
INSERT INTO Brands (Name) Values('Femme Fatale');
INSERT INTO Brands (Name) Values('Finger Paints');
INSERT INTO Brands (Name) Values('Firecracker Lacquer');
INSERT INTO Brands (Name) Values('Fleur de Lis');
INSERT INTO Brands (Name) Values('Fun Lacquer');
INSERT INTO Brands (Name) Values('Girly Bits');
INSERT INTO Brands (Name) Values('Glam Polish');
INSERT INTO Brands (Name) Values('Glitter Sheep');
INSERT INTO Brands (Name) Values('Great Lakes Lacquer');
INSERT INTO Brands (Name) Values('H & M');
INSERT INTO Brands (Name) Values('Hard Candy');
INSERT INTO Brands (Name) Values('Illamasqua');
INSERT INTO Brands (Name) Values('Illyrian');
INSERT INTO Brands (Name) Values('ILNP');
INSERT INTO Brands (Name) Values('Indigo Bananas');
INSERT INTO Brands (Name) Values('Julie G');
INSERT INTO Brands (Name) Values('KBShimmer');
INSERT INTO Brands (Name) Values('Kiko');
INSERT INTO Brands (Name) Values('Lacquer Lust');
INSERT INTO Brands (Name) Values('Lacquester');
INSERT INTO Brands (Name) Values('Lavish Polish');
INSERT INTO Brands (Name) Values('Leesha''s Lacquer');
INSERT INTO Brands (Name) Values('Lemming Lacquer');
INSERT INTO Brands (Name) Values('Lilypad Lacquer');
INSERT INTO Brands (Name) Values('Liquid Sky Lacquer');
INSERT INTO Brands (Name) Values('Lollipop Posse Lacquer');
INSERT INTO Brands (Name) Values('Lou it Yourself');
INSERT INTO Brands (Name) Values('Misslyn');
INSERT INTO Brands (Name) Values('Mod Lacquer');
INSERT INTO Brands (Name) Values('Nail Nation 3000');
INSERT INTO Brands (Name) Values('ncLA');
INSERT INTO Brands (Name) Values('NeverMind');
INSERT INTO Brands (Name) Values('Nine Zero');
INSERT INTO Brands (Name) Values('Ninja Polish');
INSERT INTO Brands (Name) Values('Noodles Nail Polish');
INSERT INTO Brands (Name) Values('Nvr Enuff');
INSERT INTO Brands (Name) Values('Octopus Party Nail Lacquer');
INSERT INTO Brands (Name) Values('OPI');
INSERT INTO Brands (Name) Values('ORLY');
INSERT INTO Brands (Name) Values('Pahlish');
INSERT INTO Brands (Name) Values('Painted Polish');
INSERT INTO Brands (Name) Values('Pantone Universe');
INSERT INTO Brands (Name) Values('Pipe Dream Polish');
INSERT INTO Brands (Name) Values('Polish ''M');
INSERT INTO Brands (Name) Values('Polish My Life');
INSERT INTO Brands (Name) Values('Polished for Days');
INSERT INTO Brands (Name) Values('Rainbow Honey');
INSERT INTO Brands (Name) Values('Sally Hansen');
INSERT INTO Brands (Name) Values('Sayuri');
INSERT INTO Brands (Name) Values('Sephora Formula X');
INSERT INTO Brands (Name) Values('Sparitual');
INSERT INTO Brands (Name) Values('Starrily');
INSERT INTO Brands (Name) Values('Super Moon Lacquer');
INSERT INTO Brands (Name) Values('Superchic Lacquer');
INSERT INTO Brands (Name) Values('Supernatural Lacquer');
INSERT INTO Brands (Name) Values('Sweet Heart Polish');
INSERT INTO Brands (Name) Values('Takko Lacquer');
INSERT INTO Brands (Name) Values('Tonic');
INSERT INTO Brands (Name) Values('Too Fancy Lacquer');
INSERT INTO Brands (Name) Values('Top Shelf Lacquer');
INSERT INTO Brands (Name) Values('Turtle Tootsie Polishes');
INSERT INTO Brands (Name) Values('ULTA');
INSERT INTO Brands (Name) Values('Vapid');
INSERT INTO Brands (Name) Values('Wing Dust');
INSERT INTO Brands (Name) Values('Zoya');

INSERT INTO Colors (Name) Values('Purple');
INSERT INTO Colors (Name) Values('Grey');
INSERT INTO Colors (Name) Values('Blue');
INSERT INTO Colors (Name) Values('Teal');
INSERT INTO Colors (Name) Values('Glitter');
INSERT INTO Colors (Name) Values('Pink');
INSERT INTO Colors (Name) Values('White');
INSERT INTO Colors (Name) Values('Green');
INSERT INTO Colors (Name) Values('Yellow');
INSERT INTO Colors (Name) Values('Red');
INSERT INTO Colors (Name) Values('Black');
INSERT INTO Colors (Name) Values('Orange');
INSERT INTO Colors (Name) Values('Brown');


INSERT INTO PolishTypes (Name) Values('Holo');
INSERT INTO PolishTypes (Name) Values('Shimmer');
INSERT INTO PolishTypes (Name) Values('Crelly');
INSERT INTO PolishTypes (Name) Values('Flakie');
INSERT INTO PolishTypes (Name) Values('Jelly');
INSERT INTO PolishTypes (Name) Values('Glitter');
INSERT INTO PolishTypes (Name) Values('Suede');
INSERT INTO PolishTypes (Name) Values('Multichrome');
INSERT INTO PolishTypes (Name) Values('Creme');
INSERT INTO PolishTypes (Name) Values('Metallic');
INSERT INTO PolishTypes (Name) Values('Duochrome');
INSERT INTO PolishTypes (Name) Values('Matte');
INSERT INTO PolishTypes (Name) Values('Texture');
INSERT INTO PolishTypes (Name) Values('Topper');
INSERT INTO PolishTypes (Name) Values('Thermal');
INSERT INTO PolishTypes (Name) Values('Neon');
INSERT INTO PolishTypes (Name) Values('Glow in the dark');


Select * From Brands
Select * from Colors
Select * from PolishTypes