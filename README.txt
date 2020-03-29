ΓΙΑ ΝΑ ΔΟΥΛΕΨΕΙ ΣΤΟ MERCURY DATABASE
ΠΡΕΠΕΙ ΝΑ ΠΡΟΣΤΕΘΟΥΝ ΝΑ ΤΡΕΞΟΥΜΕ ΤΟ SCRIPT "CREATE_TABLES_ALTER_TABLES: (ΒΛΕΠΕ ΠΑΡΑΚΑΤΩ)

ΤΡΕΧΕΙ ΑΥΤΟΝΟΜΟ ή ΑΠΟ ΤΟ MERCURY (UTPROEID.FRM πρεπει να υπάρχει το "c:\mercvb\debug\paidonagogh.exe") κανοντας διπλο κλίκ στην αρχή που δείχνει τις λήξεις
( για να δείχνει τις λήξεις των περίόδων αντί για λήξεις επιταγών δηλώνω στην πορτοκαλί οθόνη προειδοποιήσεις=2)

μολις ανοίγω το paidonagogh
ενημερώνεται από το ΤΙΜ , τραβαει τον αριθμό και το ποσό και διαλέγει την περίοδο που ταιριάζει
με την ημερ.ληξεως ως εξής :  από DATEADD(D,-1,EOS) εως (DATEADD(D,27,EOS)





















******************************************************   "CREATE_TABLES_ALTER_TABLES:"
USE [MERCURY]
GO

/****** Object:  Table [dbo].[GNOMATEYSI]    Script Date: 11/3/2020 10:10:27 πμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GNOMATEYSI](
	[KOD] [nchar](10) NULL,
	[IDPEL] [int] NULL,
	[KATHGORIA] [nvarchar](50) NULL,
	[KODNOSIMATOS] [nvarchar](10) NULL,
	[TITLOSNOSIMATOS] [nvarchar](150) NULL,
	[EIDIK1] [nvarchar](50) NULL,
	[EIDIK2] [nvarchar](50) NULL,
	[EIDIK3] [nvarchar](50) NULL,
	[EIDIK4] [nvarchar](50) NULL,
	[LOGH] [int] NULL,
	[ERGH] [int] NULL,
	[PSIH] [int] NULL,
	[FYSH] [int] NULL,
	[EIDH] [int] NULL,
	[OIKH] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ANANEOSI] [nchar](20) NULL,
	[ENARXI] [date] NULL,
	[LHXH] [date] NULL,
	[ANANEOSIAAMHNOS] [int] NULL,
	[IMAGE] [image] NULL,
	[ENERGH] [int] NOT NULL,
	[EIK] [nvarchar](50) NULL,
	[DATEKATAX] [datetime] NULL,
	[KOSTOSSYNEDRIAS] [real] NULL,
	[SYNOLKOSTOS] [real] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [MERCURY]
GO

/****** Object:  Table [dbo].[PERIODOI]    Script Date: 11/3/2020 10:10:53 πμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PERIODOI](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDGN] [int] NULL,
	[APO] [date] NULL,
	[EOS] [date] NULL,
	[N1] [real] NULL,
	[N2] [real] NULL,
	[C1] [nvarchar](50) NULL,
	[C2] [nvarchar](50) NULL,
	[SYNEDRIES] [int] NULL,
	[AJIAAPOD] [real] NULL,
	[ATIM] [nvarchar](10) NULL,
	[IDPEL] [int] NULL,
 CONSTRAINT [PK_PERIODOI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [MERCURY]
GO

/****** Object:  Table [dbo].[SYNEDRIES]    Script Date: 11/3/2020 10:11:23 πμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SYNEDRIES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDGN] [int] NULL,
	[IDTH] [int] NULL,
	[HME] [datetime] NULL,
	[ORES] [real] NULL,
	[N1] [real] NULL,
	[C1] [nvarchar](50) NULL,
	[N2] [real] NULL,
	[C2] [nvarchar](50) NULL,
	[DATEKATAX] [datetime] NULL,
	[IDPEL] [int] NULL,
	[ENERGH] [int] NULL,
 CONSTRAINT [PK_SYNEDRIES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SYNEDRIES] ADD  CONSTRAINT [DF_SYNEDRIES_ENERGJ]  DEFAULT ((1)) FOR [ENERGH]
GO

USE [MERCURY]
GO

/****** Object:  Table [dbo].[THERAP]    Script Date: 11/3/2020 10:11:42 πμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[THERAP](
	[EPO] [nvarchar](20) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LOGCH] [bit] NULL,
	[ERGCH] [bit] NULL,
	[EIDCH] [bit] NULL,
	[OIKCH] [bit] NULL,
	[SYMPCH] [bit] NULL,
 CONSTRAINT [PK_THERAP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE MERCURY

ALTER TABLE SYNEDRIES ADD IDPER INT NULL

ALTER TABLE PERIODOI ADD BERGOT NVARCHAR(20) NULL
ALTER TABLE PERIODOI ADD BOIKPS NVARCHAR(20) NULL
ALTER TABLE PERIODOI ADD BEIDDI NVARCHAR(20) NULL
ALTER TABLE PERIODOI ADD BLOGOT NVARCHAR(20) NULL
******************************************************   "CREATE_TABLES_ALTER_TABLES:"



