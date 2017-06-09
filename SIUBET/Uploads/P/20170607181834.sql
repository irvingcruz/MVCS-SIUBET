--Scripts Irving 2017.06.01

CREATE TABLE [dbo].[SOL_RES_Items](
	[IdItems] [int] IDENTITY(1,1) NOT NULL,
	[vNro] [char](2) NULL,
	[vDescripcion] [varchar](50) NULL,
	[bPadre] [bit] NULL,
	[vTipo] [char](1) NULL,
	[nPonderado] [int] NULL,
	[vAlias] [varchar](20) NULL,
 CONSTRAINT [PK_SOL_RES_Items] PRIMARY KEY CLUSTERED 
(
	[IdItems] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SOL_RES_Options](
	[IdOptions] [int] IDENTITY(1,1) NOT NULL,
	[vDescripcion] [varchar](50) NULL,
	[IdItems] [int] NULL,
	[nOrden] [int] NULL,
	[nPeso] [numeric](2, 1) NULL,
 CONSTRAINT [PK_SOL_RES_Options] PRIMARY KEY CLUSTERED 
(
	[IdOptions] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SOL_RES_Cliches](
	[IdCliches] [int] IDENTITY(1,1) NOT NULL,
	[vTipo] [char](1) NULL,
	[IdItems] [int] NULL,
	[vDescripcion] [varchar](200) NULL,
	[id_cliche] [int] NULL,
 CONSTRAINT [PK_SOL_RES_Cliches] PRIMARY KEY CLUSTERED 
(
	[IdCliches] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO