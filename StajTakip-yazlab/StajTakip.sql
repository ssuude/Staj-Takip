USE [master]
GO
/****** Object:  Database [Staj1]    Script Date: 11/20/2022 3:50:49 PM ******/
CREATE DATABASE [Staj1]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Staj1', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Staj1.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Staj1_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Staj1_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Staj1] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Staj1].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Staj1] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Staj1] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Staj1] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Staj1] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Staj1] SET ARITHABORT OFF 
GO
ALTER DATABASE [Staj1] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Staj1] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Staj1] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Staj1] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Staj1] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Staj1] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Staj1] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Staj1] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Staj1] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Staj1] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Staj1] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Staj1] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Staj1] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Staj1] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Staj1] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Staj1] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Staj1] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Staj1] SET RECOVERY FULL 
GO
ALTER DATABASE [Staj1] SET  MULTI_USER 
GO
ALTER DATABASE [Staj1] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Staj1] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Staj1] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Staj1] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Staj1] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Staj1] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Staj1', N'ON'
GO
ALTER DATABASE [Staj1] SET QUERY_STORE = OFF
GO
USE [Staj1]
GO
/****** Object:  Table [dbo].[GeriGonderilenBelgeler]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeriGonderilenBelgeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BelgeAdi] [nvarchar](250) NULL,
	[Tarih] [datetime] NULL,
	[KullaniciID] [int] NULL,
	[Aciklama] [nvarchar](500) NULL,
 CONSTRAINT [PK_GeriGonderilenBelgeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kullanici]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kullanici](
	[KullaniciID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Soyadi] [nvarchar](100) NULL,
	[Numara] [nvarchar](50) NULL,
	[Mail] [nvarchar](50) NULL,
	[Parola] [nvarchar](50) NULL,
	[KayıtTarihi] [datetime] NULL,
	[OnaylandiMi] [bit] NULL,
	[AktifMi] [bit] NULL,
	[StajDurumID] [int] NULL,
	[BeniHatirla] [bit] NULL,
	[TelefonNo] [nvarchar](50) NULL,
	[StajBaslatilsinMi] [bit] NULL,
	[Adres] [nvarchar](100) NULL,
	[BasvuruyuDegerlendiren] [nvarchar](70) NULL,
	[Sinif] [nvarchar](5) NULL,
	[BasvuruDegerlendirmeTarihi] [datetime] NULL,
	[BasvuruYapildiMi] [bit] NULL,
	[Status] [bit] NULL,
	[Fakulte] [nvarchar](50) NULL,
	[Bolum] [nvarchar](50) NULL,
	[OgretmenId] [int] NULL,
 CONSTRAINT [PK_Kullanici] PRIMARY KEY CLUSTERED 
(
	[KullaniciID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KullaniciRol]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KullaniciRol](
	[KullaniciRolID] [int] IDENTITY(1,1) NOT NULL,
	[RolID] [int] NOT NULL,
	[KullaniciID] [int] NOT NULL,
 CONSTRAINT [PK_KullaniciRol] PRIMARY KEY CLUSTERED 
(
	[RolID] ASC,
	[KullaniciID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OgretmenAtama]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OgretmenAtama](
	[OgretmenId] [int] NOT NULL,
	[OgrenciId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResmiTatiller]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResmiTatiller](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResmiTatil] [datetime] NULL,
	[Aciklama] [nvarchar](100) NULL,
 CONSTRAINT [PK_ResmiTatiller_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[RolID] [int] IDENTITY(1,1) NOT NULL,
	[RolAdi] [nvarchar](50) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[RolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staj]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staj](
	[StajID] [int] IDENTITY(1,1) NOT NULL,
	[Baslik] [nvarchar](200) NULL,
	[Icerik] [nvarchar](max) NULL,
	[Tarih] [datetime] NULL,
	[KullaniciID] [int] NULL,
	[StajDonemNO] [int] NULL,
 CONSTRAINT [PK_Staj] PRIMARY KEY CLUSTERED 
(
	[StajID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajAdi]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajAdi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](15) NULL,
 CONSTRAINT [PK_StajAdi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajBaslatilsinMi]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajBaslatilsinMi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](50) NULL,
	[Aciklama] [nvarchar](250) NULL,
 CONSTRAINT [PK_StajBaslatilsinMi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajBasvuruBelgeleri]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajBasvuruBelgeleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BelgeAdi] [nvarchar](250) NULL,
	[Tarih] [datetime] NULL,
	[KullaniciID] [int] NULL,
	[StajDonemNO] [int] NULL,
 CONSTRAINT [PK_Resim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajDefteriTeslim]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajDefteriTeslim](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DosyaAdi] [nvarchar](50) NULL,
	[KullaniciID] [int] NULL,
	[Tarih] [datetime] NULL,
 CONSTRAINT [PK_StajDefteriTeslim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajDurum]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajDurum](
	[StajDurumID] [int] IDENTITY(1,1) NOT NULL,
	[StajDurumAdi] [nvarchar](50) NULL,
	[Gizle] [bit] NULL,
 CONSTRAINT [PK_StajDurum] PRIMARY KEY CLUSTERED 
(
	[StajDurumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StajyerOgrenciBaslatma]    Script Date: 11/20/2022 3:50:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StajyerOgrenciBaslatma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IsyeriAdi] [nvarchar](70) NULL,
	[StajBaslangicTarihi] [datetime] NULL,
	[StajBitisTarihi] [datetime] NULL,
	[HaftaIciGunSayisi] [int] NULL,
	[ResmiTatilSayisi] [int] NULL,
	[ToplamCalismaSüresi] [int] NULL,
	[Tarih] [datetime] NULL,
	[CumartesiCalisiyorMu] [nvarchar](10) NULL,
	[StajAdiID] [int] NULL,
	[KullaniciID] [int] NULL,
	[IsyeriAdresi] [nvarchar](250) NULL,
	[Ev_Numara] [nvarchar](50) NULL,
	[TcKimlik_Numara] [nvarchar](50) NULL,
	[Cep_Numara] [nvarchar](50) NULL,
	[Uyruk] [nvarchar](20) NULL,
	[Adres] [nvarchar](200) NULL,
	[Isyeri_Telefon] [nvarchar](50) NULL,
	[Isyeri_Faks] [nvarchar](50) NULL,
 CONSTRAINT [PK_StajyerOgrenciBaslatma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KullaniciRol] ADD  CONSTRAINT [df_KullaniciRol_RolID]  DEFAULT ((2)) FOR [RolID]
GO
ALTER TABLE [dbo].[GeriGonderilenBelgeler]  WITH CHECK ADD  CONSTRAINT [FK_GeriGonderilenBelgeler_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[GeriGonderilenBelgeler] CHECK CONSTRAINT [FK_GeriGonderilenBelgeler_Kullanici]
GO
ALTER TABLE [dbo].[Kullanici]  WITH CHECK ADD  CONSTRAINT [FK_Kullanici_StajDurum] FOREIGN KEY([StajDurumID])
REFERENCES [dbo].[StajDurum] ([StajDurumID])
GO
ALTER TABLE [dbo].[Kullanici] CHECK CONSTRAINT [FK_Kullanici_StajDurum]
GO
ALTER TABLE [dbo].[KullaniciRol]  WITH CHECK ADD  CONSTRAINT [FK_KullaniciRol_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[KullaniciRol] CHECK CONSTRAINT [FK_KullaniciRol_Kullanici]
GO
ALTER TABLE [dbo].[KullaniciRol]  WITH CHECK ADD  CONSTRAINT [FK_KullaniciRol_Rol] FOREIGN KEY([RolID])
REFERENCES [dbo].[Rol] ([RolID])
GO
ALTER TABLE [dbo].[KullaniciRol] CHECK CONSTRAINT [FK_KullaniciRol_Rol]
GO
ALTER TABLE [dbo].[Staj]  WITH CHECK ADD  CONSTRAINT [FK_Staj_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[Staj] CHECK CONSTRAINT [FK_Staj_Kullanici]
GO
ALTER TABLE [dbo].[StajBasvuruBelgeleri]  WITH CHECK ADD  CONSTRAINT [FK_StajBasvuru_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[StajBasvuruBelgeleri] CHECK CONSTRAINT [FK_StajBasvuru_Kullanici]
GO
ALTER TABLE [dbo].[StajDefteriTeslim]  WITH CHECK ADD  CONSTRAINT [FK_StajDefteriTeslim_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[StajDefteriTeslim] CHECK CONSTRAINT [FK_StajDefteriTeslim_Kullanici]
GO
ALTER TABLE [dbo].[StajyerOgrenciBaslatma]  WITH CHECK ADD  CONSTRAINT [FK_StajyerOgrenciBaslatma_Kullanici] FOREIGN KEY([KullaniciID])
REFERENCES [dbo].[Kullanici] ([KullaniciID])
GO
ALTER TABLE [dbo].[StajyerOgrenciBaslatma] CHECK CONSTRAINT [FK_StajyerOgrenciBaslatma_Kullanici]
GO
ALTER TABLE [dbo].[StajyerOgrenciBaslatma]  WITH CHECK ADD  CONSTRAINT [FK_StajyerOgrenciBaslatma_StajAdi] FOREIGN KEY([StajAdiID])
REFERENCES [dbo].[StajAdi] ([ID])
GO
ALTER TABLE [dbo].[StajyerOgrenciBaslatma] CHECK CONSTRAINT [FK_StajyerOgrenciBaslatma_StajAdi]
GO
USE [master]
GO
ALTER DATABASE [Staj1] SET  READ_WRITE 
GO
