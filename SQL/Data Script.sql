/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [PolishWarehouse]
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'659c9ec4-5aeb-4886-bb2b-0c7251cd10a5', N'clockhart192@gmail.com', 0, N'AEvQYhNVLkElTSBFAf97Y5UPnoCNFE087vWn8CqrcoMeHNjscT03/fGeJlmeDiZPxg==', N'ec5762b0-c01a-4790-b8df-854f47e7680d', NULL, 0, 0, NULL, 1, 0, N'clockhart192@gmail.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'8e67b8cc-4cf1-4182-8893-076bf567596f', N'aeresine@gmail.com', 0, N'AI16EX2ATNZttRojaa0g2uhdH+vWYIUjgDiqKYh3HURQOqs5xFdIu3ehdEDQ84ngnw==', N'eb5f5d8f-d43e-4605-97b5-366ca22425cb', NULL, 0, 0, NULL, 1, 0, N'aeresine@gmail.com')
SET IDENTITY_INSERT [dbo].[BrandCategory] ON 

INSERT [dbo].[BrandCategory] ([ID], [Name], [Description]) VALUES (1, N'Indie', NULL)
INSERT [dbo].[BrandCategory] ([ID], [Name], [Description]) VALUES (2, N'Mainstream', NULL)
INSERT [dbo].[BrandCategory] ([ID], [Name], [Description]) VALUES (3, N'Boutique', NULL)
SET IDENTITY_INSERT [dbo].[BrandCategory] OFF
SET IDENTITY_INSERT [dbo].[Colors] ON 

INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (1, N'Purple', NULL, 1, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (2, N'Grey', NULL, 1, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (3, N'Blue', NULL, 1, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (4, N'Teal', NULL, 1, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (5, N'Glitter', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (6, N'Pink', NULL, 1, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (7, N'White', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (8, N'Green', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (9, N'Yellow', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (10, N'Red', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (11, N'Black', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (12, N'Orange', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (13, N'Brown', NULL, 1, 1, 0)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (14, N'test', NULL, 0, 1, 1)
INSERT [dbo].[Colors] ([ID], [Name], [Description], [IsPrimary], [IsSecondary], [IsGlitter]) VALUES (15, N'Destash', N'Used only for polish that goes straight to destash', 0, 0, 0)
SET IDENTITY_INSERT [dbo].[Colors] OFF
SET IDENTITY_INSERT [dbo].[PolishTypes] ON 

INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (1, N'Holo', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (2, N'Shimmer', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (3, N'Crelly', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (4, N'Flakie', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (5, N'Jelly', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (6, N'Glitter', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (7, N'Suede', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (8, N'Multichrome', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (9, N'Creme', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (10, N'Metallic', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (11, N'Duochrome', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (12, N'Matte', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (13, N'Texture', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (14, N'Topper', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (15, N'Thermal', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (16, N'Neon', NULL)
INSERT [dbo].[PolishTypes] ([ID], [Name], [Description]) VALUES (17, N'GOTD', NULL)
SET IDENTITY_INSERT [dbo].[PolishTypes] OFF
SET IDENTITY_INSERT [dbo].[Settings] ON 

INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (1, N'Show Public List', N'true', N'Boolean', 0, 1)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (2, N'Show Private Setting', N'true', N'Boolean', 0, 1)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (3, N'Image max width', N'760', N'Integer', 0, 0)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (4, N'Image max height', N'960', N'Integer', 0, 0)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (5, N'Use original quality image', N'true', N'Boolean', 0, 1)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (6, N'Use database image', N'true', N'Boolean', 0, 0)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (7, N'Konami Code Active', N'true', N'Boolean', 0, 0)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (8, N'Show Public Destash', N'true', N'Boolean', 0, 1)
INSERT [dbo].[Settings] ([ID], [KeyName], [KeyValue], [KeyDataType], [PublicSetting], [PrivateSetting]) VALUES (9, N'Private List Display Configuration', N'{
	"BrandName":"true",
	"PolishName":"true",
	"Description":"true",
	"Notes":"false",
	"Label":"true",
	"Coats":"true",
	"HasBeenTried":"true",
	"WasGift":"true"
}', N'String', 0, 0)
SET IDENTITY_INSERT [dbo].[Settings] OFF
SET IDENTITY_INSERT [dbo].[ShippingProviders] ON 

INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (1, CAST(N'2017-10-08T16:59:32.367' AS DateTime), N'USPS', N'https://tools.usps.com/go/TrackConfirmAction_input?qtc_tLabels1=')
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (2, CAST(N'2017-10-08T16:59:35.790' AS DateTime), N'Fedex', N'https://www.fedex.com/apps/fedextrack/?action=track&action=track&tracknumbers=')
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (3, CAST(N'2017-10-08T16:59:37.373' AS DateTime), N'UPS', N'https://wwwapps.ups.com/tracking/tracking.cgi?tracknum=')
INSERT [dbo].[ShippingProviders] ([ID], [CreatedOn], [Name], [TrackingBaseURL]) VALUES (4, CAST(N'2017-10-08T16:59:39.637' AS DateTime), N'DHL', N'http://webtrack.dhlglobalmail.com/?trackingnumber=')
SET IDENTITY_INSERT [dbo].[ShippingProviders] OFF
