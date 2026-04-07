USE [LabExtim]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[HireDate] [datetime] NULL,
	[LeavingDate] [datetime] NULL,
	[ID_Manager] [int] NULL,
	[ID_Dept] [int] NULL,
	[ID_Machine] [int] NULL,
	[UniqueName]  AS ((((([Surname]+'  ')+[Name])+' (')+CONVERT([nvarchar],[ID],(0)))+')'),
	[UserGUID] [uniqueidentifier] NULL,
	[Role] [smallint] NULL,
	[ID_Company] [int] NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_EmployeesToLabe]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE VIEW [dbo].[VW_EmployeesToLabe]
AS


SELECT
       max(case when id_company = 1 then id else null end) LabeID
	  ,max(case when id_company = 2 then id else null end) CartoLabeID
	   ,Name
	   ,Surname
	  ,(((([Surname]+'  ')+[Name])+' (')+CONVERT([nvarchar],max(case when id_company = 1 then id else null end),(0)))+')' UniqueName
	  ,max(case when id_company = 1 then Role else null end) Role
	  ,max(case when id_company = 1 then UserGUID else null end) LabeUserGuid
	  ,max(case when id_company = 2 then UserGUID else null end) CartoLabeUserGuid

  FROM [LabExtim].[dbo].[Employees]

  group by 
	    Name
	   ,Surname
       
having  max(case when id_company = 1 then id else null end) is not null

GO
/****** Object:  Table [dbo].[ProductionOrders]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionOrders](
	[ID] [int] IDENTITY(6100,1) NOT NULL,
	[Number] [varchar](10) NULL,
	[Description] [nvarchar](200) NULL,
	[ID_Customer] [int] NULL,
	[ID_CustomerOrder] [int] NULL,
	[ID_Quotation] [int] NULL,
	[ID_Company] [int] NULL,
	[StartDate] [datetime] NULL,
	[Quantity] [real] NULL,
	[DeliveryDate] [datetime] NULL,
	[Cost] [money] NULL,
	[DirectSupply] [bit] NOT NULL,
	[Price] [money] NULL,
	[Status] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[AccountNote] [nvarchar](max) NULL,
	[YearStartDate]  AS (datepart(year,[startdate])),
	[ID_Contractor] [int] NULL,
	[Number_1] [varchar](10) NULL,
	[NonConformityCode] [int] NULL,
	[ID_Manager] [int] NULL,
	[Note1] [nvarchar](max) NULL,
	[ComplaintReceived] [int] NULL,
	[CorrectiveActionCode] [int] NULL,
	[UnusedProductsCheck] [bit] NULL,
 CONSTRAINT [PK_ProductionOrders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PickingItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PickingItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[ItemTypeCode] [int] NOT NULL,
	[ItemDescription] [nvarchar](255) NULL,
	[UM] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[SupplierCode] [int] NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[Percentage] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Link] [nvarchar](50) NULL,
	[MILink] [nvarchar](50) NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Template] [int] NULL,
	[ItemManufacturing] [int] NULL,
	[StandardPercentageCode] [int] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_PickingItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionOrderDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionOrderDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_ProductionOrder] [int] NOT NULL,
	[ID_QuotationDetail] [int] NULL,
	[ID_Owner] [int] NULL,
	[ID_Company] [int] NULL,
	[ID_PickingItem] [int] NULL,
	[SupplierCode] [int] NULL,
	[UMRawMaterial] [int] NULL,
	[UMUser] [int] NULL,
	[ID_PickingItemSup] [int] NULL,
	[SupplierCodeSup] [int] NULL,
	[RawMaterialX] [real] NULL,
	[RawMaterialY] [real] NULL,
	[RawMaterialZ] [real] NULL,
	[RawMaterialQuantity] [real] NULL,
	[ID_Phase] [int] NULL,
	[UMProduct] [int] NULL,
	[ProducedQuantity] [real] NULL,
	[ProductionTime] [numeric](18, 0) NULL,
	[ProductionDate] [datetime] NULL,
	[QuantityOver] [bit] NOT NULL,
	[Cost] [money] NULL,
	[DirectSupply] [bit] NOT NULL,
	[Note] [varchar](max) NULL,
	[FreeTypeCode] [int] NULL,
	[FreeItemTypeCode] [int] NULL,
	[FreeItemDescription] [varchar](255) NULL,
	[RFlag] [char](1) NULL,
	[SFlag] [char](1) NULL,
	[FFlag] [char](1) NULL,
	[HistoricalCostPhase] [money] NULL,
	[HistoricalCostRawM] [money] NULL,
	[HistoricalCostSupM] [money] NULL,
	[MacroRef] [int] NULL,
	[OkCopiesCount] [int] NULL,
	[KoCopiesCount] [int] NULL,
	[Special] [bit] NULL,
	[CreateTS] [datetime] NULL,
	[UpdateTS] [datetime] NULL,
	[ID_DeliveryTrip] [int] NULL,
 CONSTRAINT [PK_ProductionOrderDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Types]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Types](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](100) NULL,
	[Order] [nvarchar](50) NULL,
	[Category] [char](1) NULL,
 CONSTRAINT [PK_Phase] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemTypes](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NULL,
	[Order] [nvarchar](50) NULL,
	[Category] [char](1) NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Code] [int] NOT NULL,
	[Type] [nvarchar](1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Contact] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](18) NULL,
	[Fax] [nvarchar](18) NULL,
	[Street] [nvarchar](70) NULL,
	[CAP] [nvarchar](9) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](2) NULL,
	[P_IVA] [nvarchar](11) NULL,
	[Note] [nvarchar](255) NULL,
	[Name2] [varchar](50) NULL,
	[IDAgente1] [int] NULL,
	[DescrizioneAgente1] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_EmployeesWorkingDayHours_old]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[VW_EmployeesWorkingDayHours_old]
AS

--WITH CTE_DatesTable
-- AS
-- (
--   SELECT CAST('20100101' as datetime) AS DateValue
--   UNION ALL
--   SELECT DATEADD(dd, 1, DateValue)
--   FROM CTE_DatesTable
--   WHERE DATEADD(dd, 1, DateValue) < '20300101'
-- )


SELECT 

  b.ID
, b.UniqueName
, a.DateValue As ProductionDate
, IsNull(c.ProductionTime, 0) As ProductionTime
, IsNull(c.ID_Phase, 0) As ID_Phase
, IsNull(c.RawMaterialX, 0) As RawMaterialX
, IsNull(c.RawMaterialY, 0) As RawMaterialY
, IsNull(c.RawMaterialZ, 0) As RawMaterialZ
, IsNull(c.ItemDescription, '') As ItemDescription
, IsNull(c.TypeCode, 0) As TypeCode
, IsNull(c.TypeDescription, '') As TypeDescription
, IsNull(c.ItemTypeCode, 0) As ItemTypeCode
, IsNull(c.ItemTypeDescription, '') As ItemTypeDescription
, IsNull(c.PickingItemOrder, 0) As PickingItemOrder
--, c.YearProductionDate
--, c.MonthProductionDate
, YEAR(DateValue) AS YearProductionDate
, MONTH(DateValue) AS MonthProductionDate 
,c.ID_ProductionOrder
,c.Number
,c.ID_Company
,c.ID_Customer
,c.CustomerName

FROM 
(
 --SELECT DateValue FROM CTE_DatesTable 

-- SELECT DATEADD(d,number,(Select MIN(ProductionDate) From dbo.ProductionOrderDetails))AS DateValue
--FROM master..spt_values
--WHERE number BETWEEN 1 AND
--DATEDIFF(d,(Select MIN(ProductionDate) From dbo.ProductionOrderDetails),(Select MAX(ProductionDate) From dbo.ProductionOrderDetails))
--AND type = 'P'

select
    dateadd(d, v1.number+v2.number*2048, '20100101') DateValue
from master..spt_values v1
    cross join (select number from master..spt_values where number<5 and type='p') v2       
where type='p'
    and (v1.number+v2.number*2048)<= datediff(d,'20100101','20241231')
	
) a
Cross Join  dbo.Employees b
Left Join
(
Select
  dbo.ProductionOrderDetails.ID_ProductionOrder
, dbo.ProductionOrders.Number
, dbo.ProductionOrders.ID_Customer
, dbo.Customers.Name CustomerName
, dbo.ProductionOrderDetails.ID_Owner
, dbo.ProductionOrderDetails.ID_Company
, dbo.ProductionOrderDetails.ProductionDate
, dbo.ProductionOrderDetails.ProductionTime
, dbo.ProductionOrderDetails.ID_Phase
, dbo.ProductionOrderDetails.RawMaterialX
, dbo.ProductionOrderDetails.RawMaterialY
, dbo.ProductionOrderDetails.RawMaterialZ
, dbo.PickingItems.ItemDescription
, dbo.PickingItems.TypeCode
, dbo.Types.Description AS TypeDescription
, dbo.PickingItems.ItemTypeCode
, dbo.ItemTypes.Description AS ItemTypeDescription
, dbo.PickingItems.[Order] AS PickingItemOrder
, YEAR(dbo.ProductionOrderDetails.ProductionDate) AS YearProductionDate
, MONTH(dbo.ProductionOrderDetails.ProductionDate) AS MonthProductionDate 
From
     dbo.ProductionOrderDetails INNER JOIN
     dbo.PickingItems ON dbo.ProductionOrderDetails.ID_Phase = dbo.PickingItems.ID INNER JOIN
     dbo.Types ON dbo.PickingItems.TypeCode = dbo.Types.Code INNER JOIN
     dbo.ItemTypes ON dbo.PickingItems.ItemTypeCode = dbo.ItemTypes.Code INNER JOIN
	 dbo.ProductionOrders On dbo.ProductionOrders.ID = dbo.ProductionOrderDetails.ID_ProductionOrder LEFT JOIN
	 dbo.Customers on dbo.Customers.Code = dbo.ProductionOrders.ID_Customer

) c 
On
  (a.DateValue = c.ProductionDate And b.ID = c.ID_Owner)
GO
/****** Object:  View [dbo].[VW_ProductionOrdersCosts]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE VIEW [dbo].[VW_ProductionOrdersCosts]
AS
SELECT     dbo.ProductionOrders.ID, dbo.ProductionOrders.ID_Company, MIN(dbo.ProductionOrders.Number) AS Number, dbo.ProductionOrders.Description AS PORDescription, 
					  MIN(ProductionOrders.Quantity) as StartQuantity,
                      MIN(dbo.ProductionOrders.ID_Customer) AS ID_Customer, MIN(dbo.ProductionOrders.ID_CustomerOrder) AS ID_CustomerOrder, 
                      MIN(dbo.ProductionOrders.ID_Quotation) AS ID_Quotation,

					 CASE WHEN (MIN(CAST(dbo.ProductionOrders.DirectSupply AS integer)) = 1) THEN MIN(dbo.ProductionOrders.Cost) 
                      ELSE 0 END AS PORDirectCost,

 SUM(CASE WHEN (CAST(dbo.ProductionOrderDetails.DirectSupply AS integer) = 1) 

 THEN IsNull(ProductionOrderDetails.Cost,0) ELSE

 (CASE WHEN dbo.ProductionOrderDetails.RFlag IS NULL
	THEN IsNull(dbo.ProductionOrderDetails.RawMaterialQuantity, 0) * IsNull(dbo.PickingItems.Cost, 0)
	ELSE 0
	END)


+
--(CASE WHEN dbo.ProductionOrderDetails.SFlag IS NULL
--	THEN (IsNull(dbo.ProductionOrderDetails.RawMaterialX, 0)
--		* IsNull(dbo.ProductionOrderDetails.RawMaterialY, 1) 
--		* IsNull(dbo.ProductionOrderDetails.RawMaterialZ, 1))
--		/Cast(1 as Decimal) * IsNull(PickingItems_2.Cost, 0)
--	ELSE 0
--	END)

(CASE WHEN dbo.ProductionOrderDetails.SFlag IS NULL
	THEN (
	Case When dbo.ProductionOrderDetails.RawMaterialX is Null And dbo.ProductionOrderDetails.RawMaterialY is Null And dbo.ProductionOrderDetails.RawMaterialZ is Null
	Then 0 Else
	IsNull(dbo.ProductionOrderDetails.RawMaterialX, 1)
		* IsNull(dbo.ProductionOrderDetails.RawMaterialY, 1) 
		* IsNull(dbo.ProductionOrderDetails.RawMaterialZ, 1) End)
		/Cast(1 as Decimal) * IsNull(PickingItems_2.Cost, 0)
	ELSE 0
	END)


+ (IsNull(PickingItems_1.Cost, 0)
* IsNull(dbo.ProductionOrderDetails.ProductionTime, 0) / 36000000000
 + IsNull(ProductionOrderDetails.Cost,0)) END)  AS PORDetailsCost,

Sum(
Case When (Cast(dbo.ProductionOrderDetails.DirectSupply AS integer) = 1) Then IsNull(ProductionOrderDetails.Cost,0) Else
 Case When ProductionOrderDetails.FreeTypeCode Is Not Null Then ProductionOrderDetails.Cost Else
    IsNull(ProductionOrderDetails.HistoricalCostPhase, 0)
	+ Case When RFlag Is Null Then IsNull(ProductionOrderDetails.HistoricalCostRawM, 0) Else 0 End
	+ Case When SFlag Is Null Then IsNull(ProductionOrderDetails.HistoricalCostSupM, 0) Else 0 End
End
End)
   As PORDetailsHistoricalCost,

 IsNull(MIN(dbo.ProductionOrders.Price),0) AS PORPrice,

					

(Select Sum(ProducedQuantity) from ProductionOrderDetails where ProductionOrders.ID = ProductionOrderDetails.ID_ProductionOrder and  QuantityOver = 1 group by ProductionOrderDetails.ID_ProductionOrder) as ProducedQuantity,

MIN(dbo.ProductionOrders.Status) AS [Status],
dbo.ProductionOrders.StartDate,
MAX(dbo.ProductionOrderDetails.ProductionDate) as EndDate,
dbo.ProductionOrders.AccountNote,
dbo.ProductionOrders.Note,
dbo.ProductionOrders.NonConformityCode,
dbo.ProductionOrders.ComplaintReceived,
dbo.ProductionOrders.CorrectiveActionCode
--isnull(dbo.ProductionOrders.AccountNote,'') + '(' +
--isnull(
--(Select 
--       distinct Left(Main.Aggreg,Len(Main.Aggreg)-1) 
--From
--    (
--        Select ST2.id, 
--            (
--			Select
--                'Codice marca inchiostro: ' + ST1.CodiceMarcaInchiostro + ' ,',
--				'Ricetta: ' + case when ST1.Ricetta = 1 then 'SI' else 'NO' end + ' ,',
--				'Telaio numero fili: ' + cast(ST1.TelaioNumeroFili as varchar) + ' ,',
--				'Gelatina spessore: ' + cast(ST1.GelatinaSpessore as varchar) + ' ,',
--				'Racla inclinazione: ' + cast(ST1.RaclaInclinazione as varchar) + ' ,',
--				'Racla durezza spigolo: ' + cast(ST1.RaclaDurezzaSpigolo as varchar) + ' ,',
--				'Codice marca film: ' + ST1.CodiceMarcaFilm + ' ,',
--				'Cliche reso: ' + ST1.ClicheReso + ' ,',
--				'Cliche condizioni: ' + ST1.ClicheCondizioni + ' ,'

--				 AS [text()]
--                From dbo.ProductionOrderTechSpecs ST1 where ID_ProductionOrder=dbo.ProductionOrders.ID
--                 ORDER BY ST1.id
--                For XML PATH ('')
--            ) Aggreg
--        From dbo.ProductionOrderTechSpecs ST2
		
--    ) [Main] where aggreg is not null),'') + ')' As 'AccountNote'
	 


FROM         dbo.ProductionOrders LEFT JOIN
                      dbo.ProductionOrderDetails ON dbo.ProductionOrders.ID = dbo.ProductionOrderDetails.ID_ProductionOrder LEFT OUTER JOIN
                      dbo.PickingItems ON dbo.PickingItems.ID = dbo.ProductionOrderDetails.ID_PickingItem LEFT OUTER JOIN
                      dbo.PickingItems AS PickingItems_1 ON PickingItems_1.ID = dbo.ProductionOrderDetails.ID_Phase LEFT OUTER JOIN
					  dbo.PickingItems AS PickingItems_2 ON PickingItems_2.ID = dbo.ProductionOrderDetails.ID_PickingItemSup 
					  --LEFT OUTER JOIN dbo.Quotations ON dbo.ProductionOrders.ID_Quotation = dbo.Quotations.ID
--where ProductionOrders.Status = 3

GROUP BY dbo.ProductionOrders.ID, dbo.ProductionOrders.ID_Company, dbo.ProductionOrders.Description, dbo.ProductionOrders.StartDate, dbo.ProductionOrders.Note, dbo.ProductionOrders.AccountNote,
dbo.ProductionOrders.NonConformityCode,
dbo.ProductionOrders.ComplaintReceived,
dbo.ProductionOrders.CorrectiveActionCode
GO
/****** Object:  Table [dbo].[QuotationDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Quotation] [int] NOT NULL,
	[Position] [nvarchar](50) NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[CommonKey] [int] NULL,
	[MacroItemKey] [int] NULL,
	[ItemTypeCode] [int] NOT NULL,
	[ItemTypeDescription] [nvarchar](100) NULL,
	[UM] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [real] NOT NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[SelectPhase] [bit] NOT NULL,
	[MarkUp] [int] NOT NULL,
	[SupplierCode] [int] NULL,
	[Percentage] [int] NOT NULL,
	[Save] [bit] NOT NULL,
 CONSTRAINT [PK_QuotationDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quotations]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotations](
	[ID] [int] IDENTITY(2,1) NOT NULL,
	[Number] [varchar](10) NULL,
	[ID_Company] [int] NULL,
	[CustomerCode] [int] NULL,
	[Date] [datetime] NULL,
	[Subject] [nvarchar](200) NULL,
	[Q1] [int] NULL,
	[Q2] [int] NULL,
	[Q3] [int] NULL,
	[Q4] [int] NULL,
	[Q5] [int] NULL,
	[MarkUp] [int] NULL,
	[ID_Owner] [int] NULL,
	[ID_Approver] [int] NULL,
	[Draft] [bit] NULL,
	[Status] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[P1] [bit] NULL,
	[P2] [bit] NULL,
	[P3] [bit] NULL,
	[P4] [bit] NULL,
	[P5] [bit] NULL,
	[PriceCom] [nvarchar](max) NULL,
	[PrintingMainText] [nvarchar](max) NULL,
	[UpdateDate] [datetime] NULL,
	[ID_Manager] [int] NULL,
	[Note1] [nvarchar](max) NULL,
	[Note2] [nvarchar](max) NULL,
 CONSTRAINT [PK_Quotations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_QuotationsCostsPrices]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_QuotationsCostsPrices]
AS
SELECT

  dbo.Quotations.ID AS ID_Quotation
, dbo.Quotations.ID_Owner AS QUOOwner
, dbo.Quotations.Subject AS QUOSubject
, dbo.Quotations.Q1
, (Select SUM(dbo.QuotationDetails.Cost * dbo.QuotationDetails.Quantity )
		FROM dbo.QuotationDetails 
		Where dbo.QuotationDetails.ID_Quotation = dbo.Quotations.ID
		And dbo.QuotationDetails.Multiply = 0)
  As QUORefFixCost

,(Select SUM(dbo.QuotationDetails.Price * dbo.QuotationDetails.Quantity)
		FROM dbo.QuotationDetails 
		Where dbo.QuotationDetails.ID_Quotation = dbo.Quotations.ID
		And dbo.QuotationDetails.Multiply = 0)
  As QUORefFixPrice

, (Select SUM(dbo.QuotationDetails.Cost * dbo.QuotationDetails.Quantity )
		FROM dbo.QuotationDetails 
		Where dbo.QuotationDetails.ID_Quotation = dbo.Quotations.ID
		And dbo.QuotationDetails.Multiply = 1)
  As QUORefVarCost

,(Select SUM(dbo.QuotationDetails.Price * dbo.QuotationDetails.Quantity )
		FROM dbo.QuotationDetails 
		Where dbo.QuotationDetails.ID_Quotation = dbo.Quotations.ID
		And dbo.QuotationDetails.Multiply = 1)
  As QUORefVarPrice
,PriceCom


FROM         dbo.QuotationDetails RIGHT JOIN
             dbo.Quotations ON dbo.QuotationDetails.ID_Quotation = dbo.Quotations.ID
WHERE     (dbo.Quotations.Q1 > 0)
GROUP BY dbo.QuotationDetails.ID_Quotation, dbo.Quotations.ID, dbo.Quotations.Q1, dbo.Quotations.Subject, dbo.Quotations.ID_Owner, PriceCom

GO
/****** Object:  View [dbo].[VW_QUOPORCostsPrices_select_Company01]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE   VIEW [dbo].[VW_QUOPORCostsPrices_select_Company01]
AS
SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END )
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C','E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue,

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  )
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C','E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as ProvvTotValue,

(SELECT   max(ts.tm_datdoc)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C','E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as DataBolla,

(SELECT   max(ts.tm_tipork)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C','E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where VW_ProductionOrdersCosts.ID_Company= 1

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode
GO
/****** Object:  View [dbo].[VW_QUOPORCostsPrices_select_Company02]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





















CREATE   VIEW [dbo].[VW_QUOPORCostsPrices_select_Company02]
AS
SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END ),0)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
+
(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END ),0)
FROM CartoLabe.dbo.movmag dt
	 inner join CartoLabe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
as FATTotValue,

(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  ),0)
FROM LABE.dbo.movmag dt
	 inner join LABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
+
(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  ),0)
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) 
as ProvvTotValue,

Coalesce(
(SELECT   max(ts.tm_datdoc)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
,
(SELECT   max(ts.tm_datdoc)
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) 
)
as DataBolla,

Coalesce(
(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
,
(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
)
as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where VW_ProductionOrdersCosts.ID_Company= 2

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode
GO
/****** Object:  Table [dbo].[DDTQUOPORCostsPrices]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DDTQUOPORCostsPrices](
	[ID_Customer] [int] NULL,
	[TipoRec] [varchar](1) NOT NULL,
	[SerieBolla] [varchar](3) NOT NULL,
	[NumBolla] [int] NOT NULL,
	[DataBolla] [datetime] NOT NULL,
	[QtaBolla] [decimal](38, 9) NULL,
	[PrezzoBolla] [decimal](38, 6) NULL,
	[FATTotValue] [money] NULL,
	[ProvvTotValue] [money] NULL,
	[ID] [int] NOT NULL,
	[ID_Company] [int] NULL,
	[Number] [varchar](10) NULL,
	[Status] [int] NULL,
	[StartQuantity] [float] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ID_Quotation] [int] NULL,
	[QUOSubject] [nvarchar](200) NULL,
	[QUOOwner] [int] NULL,
	[OwnerName] [nvarchar](135) NULL,
	[PORPropCost] [float] NULL,
	[PORPropHistoricalCost] [money] NULL,
	[QUOPropCost] [float] NULL,
	[PropProducedQuantity] [float] NULL,
	[PORTotCost] [float] NULL,
	[PORTotHistoricalCost] [money] NULL,
	[QUOTotCost] [float] NULL,
	[AccountNote] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[IDAgente1] [int] NULL,
	[DescrizioneAgente1] [nvarchar](50) NULL,
	[PriceCom] [nvarchar](max) NULL,
	[NonConformityCode] [int] NULL,
	[ComplaintReceived] [int] NULL,
	[CorrectiveActionCode] [int] NULL,
 CONSTRAINT [PK_VW_DDTQUOPORCostsPrices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[TipoRec] ASC,
	[SerieBolla] ASC,
	[NumBolla] ASC,
	[DataBolla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NonConformities]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NonConformities](
	[ID] [int] NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_NonConformities] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorrectiveActions]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorrectiveActions](
	[ID] [int] NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_CorrectiveActions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_DDTQUOPORCostsPrices]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE   VIEW [dbo].[VW_DDTQUOPORCostsPrices]
AS


SELECT

       cp.[ID_Customer]
      ,cu.Name CustomerName
      ,cp.[TipoRec]
      ,cp.[SerieBolla]
      ,cp.[NumBolla]
      ,cp.[DataBolla]
      ,cp.[QtaBolla]
      ,cp.[PrezzoBolla]
      ,cp.[FATTotValue]
      ,cp.[ProvvTotValue]
      ,cp.[ID]
      ,cp.[ID_Company]
      ,cp.[Number]
      ,cp.[Status]
      ,cp.[StartQuantity]
      ,cp.[StartDate]
      ,cp.[EndDate]
      ,cp.[ID_Quotation]
       ,isnull(cp.[QUOSubject],'Senza nome') [QUOSubject]
      ,cp.[QUOOwner]
      ,cp.[OwnerName]
      ,cp.[PORPropCost]
      ,cp.[PORPropHistoricalCost]
      ,cp.[QUOPropCost]
      ,cp.[PropProducedQuantity]
      ,cp.PORTotCost
      ,cp.PORTotHistoricalCost
      ,cp.QUOTotCost
      ,cp.[AccountNote]
      ,cp.[Note]
      ,cp.[IDAgente1]
      ,cp.[DescrizioneAgente1]
      ,cp.[PriceCom]
      ,cp.[NonConformityCode]
      ,nc.Description NonConformityDescription
	  ,po.ID_Manager
	  ,po.ComplaintReceived
	  ,po.CorrectiveActionCode
	  ,ca.Description CorrectiveActionDescription

  FROM [dbo].[DDTQUOPORCostsPrices] cp

  left join Customers cu on Code = cp.ID_Customer
  left join NonConformities nc on nc.ID = cp.NonConformityCode
  left join ProductionOrders po on po.ID = cp.ID
  left join CorrectiveActions ca on ca.ID = po.CorrectiveActionCode



GO
/****** Object:  Table [dbo].[Units]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NOT NULL,
 CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[ID] [int] NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[Code] [int] NOT NULL,
	[Type] [nvarchar](1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Contact] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](18) NULL,
	[Fax] [nvarchar](18) NULL,
	[Street] [nvarchar](70) NULL,
	[CAP] [nvarchar](9) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](2) NULL,
	[P_IVA] [nvarchar](11) NULL,
	[Note] [nvarchar](255) NULL,
	[MarkUp] [int] NULL,
	[Name2] [varchar](50) NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_ProductionOrderDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE   VIEW [dbo].[VW_ProductionOrderDetails]
AS

select 
d.ID_ProductionOrder,
d.id,
--d.ID_Owner,
e.UniqueName Operatore,
--d.ID_Company,
cp.Description Company,

--d.SupplierCode,
su.Name Fornitore,
--ID_PickingItem,
p.ItemDescription PickingItem,
--UMRawMaterial,
u.Description UM,
d.RawMaterialX,
d.RawMaterialY,
d.RawMaterialZ,

--d.SupplierCodeSup,
su2.Name FornitoreSup,
--ID_PickingItem,
p2.ItemDescription PickingItemSup,
--UMProduct,
u2.Description UMproduct,
d.ProducedQuantity,



d.ID_Phase,
f.ItemDescription Fase,
cast(d.ProductionTime /10000000.0 /60.0 as int) ProdTimeMin,
d.ProductionDate,
d.FreeItemDescription,
d.RFlag,
d.SFlag,
d.FFlag,
d.Cost,
d.Note,
d.MacroRef
from ProductionOrderDetails d
left join Employees e on e.id=d.ID_Owner
left join Companies cp on cp.id=d.ID_Company
left join Suppliers su on su.Code=d.SupplierCode
left join Suppliers su2 on su2.Code=d.SupplierCodeSup
left join PickingItems p on p.ID = ID_PickingItem
left join PickingItems p2 on p2.ID = ID_PickingItemSup
left join Units u on u.ID = d.UMRawMaterial
left join Units u2 on u2.ID = d.UMProduct
left join PickingItems f on f.ID = ID_Phase


GO
/****** Object:  Table [dbo].[ProductionMPS]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionMPS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDProductionOrder] [int] NULL,
	[IDPickingItem] [int] NULL,
	[IDMacroItem] [int] NULL,
	[IDMacroItemDetail] [int] NULL,
	[IDQuotationDetail] [int] NULL,
	[IDProductionMachine] [int] NULL,
	[NumProductionMachine] [int] NULL,
	[Order] [nvarchar](50) NULL,
	[ProdStart] [datetime] NULL,
	[Priority] [int] NULL,
	[ProdTimeMin] [int] NULL,
	[ProdEnd] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ProductionMPS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Company] [int] NULL,
	[Description] [nvarchar](100) NULL,
	[Order] [varchar](50) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionMachines]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionMachines](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ID_Company] [int] NULL,
	[IDDepartment] [int] NULL,
	[Quantity] [int] NULL,
	[Priority] [int] NULL,
	[MinPerDay] [int] NULL,
	[MinOvertime] [int] NULL,
	[ManPowerCoeff] [decimal](18, 2) NULL,
	[DaysOfWeek] [varchar](13) NULL,
	[WorkTimeStart] [time](7) NULL,
	[BreakTimeStart] [time](7) NULL,
	[BreakTimeEnd] [time](7) NULL,
	[WorkTimeEnd]  AS (dateadd(minute,[MinPerDay]+datediff(minute,[BreakTimeStart],[BreakTimeEnd]),[WorkTImeStart])),
	[AlwaysExternal] [bit] NULL,
	[ID_ExternalCompany] [int] NULL,
	[BarColor] [varchar](50) NULL,
	[Inserted] [bit] NOT NULL,
 CONSTRAINT [PK_ProductionMachines] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_ProductionMPSGroupedByMachine]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[VW_ProductionMPSGroupedByMachine]
AS


	--SELECT 
 --      mp.[IDProductionOrder]
	--  ,po.Description PODescription
 --     ,mp.[IDProductionMachine]
 --     ,mp.[NumProductionMachine]
 --     ,min(mp.[ProdStart]) ProdStart
 --     ,sum(mp.[ProdTimeMin]) ProdTimeMin
 --     ,max(mp.[ProdEnd]) ProdEnd
 -- FROM [LabExtim].[dbo].[ProductionMPS] mp

 -- inner join ProductionOrders po on mp.[IDProductionOrder] = po.ID

 -- where mp.status = 11

 -- group by 

 --      mp.[IDProductionOrder]
	--  ,po.Description
 --     ,mp.[IDProductionMachine]
 --     ,mp.[NumProductionMachine]
	--  ,cast(mp.ProdStart as date)

SELECT 
       mp.[IDProductionOrder]
	  ,po.Description PODescription
	  ,po.ID_Company
	  ,cu.Code CustomerCode
	  ,dp.ID IDDepartment
      ,mp.[IDProductionMachine]
	  ,pm.Description PMDescription
      ,mp.[NumProductionMachine]
      ,min(mp.[ProdStart]) ProdStart
      ,sum(mp.[ProdTimeMin]) ProdTimeMin
      ,max(mp.[ProdEnd]) ProdEnd
	  ,mp.Status [Status]
	  ,max(pm.BarColor) BarColor
  FROM [LabExtim].[dbo].[ProductionMPS] mp

  inner join ProductionOrders po on mp.[IDProductionOrder] = po.ID
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  inner join Departments dp on pm.IDDepartment = dp.ID

  where mp.status in (11,15)

  group by 

       mp.[IDProductionOrder]
	  ,po.Description
	  ,po.ID_Company
	  ,cu.Code
	  ,dp.ID
      ,mp.[IDProductionMachine]
	  ,pm.Description
      ,mp.[NumProductionMachine]
	  ,cast(mp.ProdStart as date)
	  ,mp.Status
GO
/****** Object:  Table [dbo].[MacroItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MacroItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[ItemTypeCode] [int] NOT NULL,
	[MacroItemDescription] [nvarchar](255) NULL,
	[UM] [int] NOT NULL,
	[Cost] [decimal](18, 4) NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[Percentage] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Link] [nvarchar](50) NULL,
	[PILink] [nvarchar](50) NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Template] [int] NULL,
	[ItemManufacturing] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[ExpandInStats] [bit] NOT NULL,
 CONSTRAINT [PK_MacroItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Statuses]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statuses](
	[ID] [int] NOT NULL,
	[Description] [nchar](30) NOT NULL,
	[StatusType] [int] NOT NULL,
 CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MacroItemDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MacroItemDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_MacroItem] [int] NOT NULL,
	[Position] [nvarchar](50) NOT NULL,
	[CommonKey] [int] NOT NULL,
	[Quantity] [real] NOT NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[SelectPhase] [bit] NOT NULL,
	[Link] [nvarchar](50) NULL,
 CONSTRAINT [PK_MacroItemDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_ProductionMPS]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













CREATE VIEW [dbo].[VW_ProductionMPS]
AS


SELECT mp.[ID]
      ,mp.[IDProductionOrder]
	  ,po.Description poDescription
	  ,po.ID_Customer
	  ,cu.Name cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
      ,mp.[IDMacroItemDetail]
	  ,mi.MacroItemDescription
	  ,qd.ID_Quotation
      ,mp.[IDQuotationDetail]
	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      ,mp.[NumProductionMachine]
      ,mp.[Order]
      ,mp.[ProdStart]
      ,mp.[Priority]
      ,mp.[ProdTimeMin]
	  ,dbo.IntToMinutes([ProdTimeMin]) ProdTime
      ,mp.[ProdEnd]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription

  FROM [dbo].[ProductionMPS] mp

  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  inner join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3


GO
/****** Object:  View [dbo].[VW_ProductionMPSGroupedByDate]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE VIEW [dbo].[VW_ProductionMPSGroupedByDate]
AS


	SELECT 
	a.IDProductionMachine,
	a.NumProductionMachine
	,Cast(a.ProdStart as date) ProdDate
    ,sum(a.[ProdTimeMin]) as ProdTimeAssignedMin
	,max(b.MinPerDay) as ProdTimeMaxMin
	,max(b.MinPerDay) - sum(a.[ProdTimeMin]) as ProdTimeAvailMin
	,case when
		dateadd(MINUTE, -sum(a.[ProdTimeMin]), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeEnd , '17:00:00' ) ) < 
		cast(Cast(a.ProdStart as date) as datetime) + isnull(b.BreakTimeStart , '12:00:00' ) 
	then 
		dateadd(MINUTE, -sum(a.[ProdTimeMin]) - datediff(MINUTE, isnull(b.BreakTimeStart , '12:00:00' ),isnull(b.BreakTimeEnd , '13:00:00' )), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeEnd , '17:00:00' )  )
	else
		dateadd(MINUTE, -sum(a.[ProdTimeMin]), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeEnd , '17:00:00' ) )
	end
	  as ProdStart
	,case when
		dateadd(MINUTE, sum(a.[ProdTimeMin]), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeStart , '8:00:00' ) ) > 
		cast(Cast(a.ProdStart as date) as datetime) + isnull(b.BreakTimeStart , '12:00:00' ) 
	then 
		dateadd(MINUTE, sum(a.[ProdTimeMin]) + datediff(MINUTE, isnull(b.BreakTimeStart , '12:00:00' ),isnull(b.BreakTimeEnd , '13:00:00' )), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeStart , '8:00:00' )  )
	else
		dateadd(MINUTE, sum(a.[ProdTimeMin]), cast(Cast(a.ProdStart as date) as datetime) + isnull(b.WorkTimeStart , '8:00:00' ) )
	end
	  as ProdEnd
	,b.DaysOfWeek

  FROM [dbo].[ProductionMPS] a inner join ProductionMachines b on a.IDProductionMachine = b.id 

  Group By 

	 a.IDProductionMachine
	,a.NumProductionMachine
	,Cast(a.ProdStart as date)
	,b.DaysOfWeek
	,b.WorkTimeStart
	,b.BreakTimeStart
	,b.BreakTimeEnd
	,b.WorkTimeEnd


GO
/****** Object:  View [dbo].[VW_ProductionSequences]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_ProductionSequences]
AS

select poi.ID poiid, poi.Description,  poi.Quantity poiquantity, qi.ID idqi, qi.Subject, qi.Q1, qd.id qdid, qd.ItemTypeDescription, qd.Quantity qdquantity, mi.id miid, mi.MacroItemDescription, md.ID mdid, md.Quantity mdquantity, pi.ID piid, pi.ItemDescription,
 ti.Code ticode, ti.Description tidescription, ii.code iicode, ii.Description iidescription , ti1.Description ti1description, ii1.Description ii1description, pi.UM, qd.Quantity*md.Quantity*(case when qd.Multiply = 0 or qi.Q1 = 0 or poi.Quantity = 1 then 1 else poi.Quantity/cast(qi.Q1 as real) end) *60 ProdTimeMin
 from 
 productionOrders poi
 inner join Quotations qi on qi.ID = poi.ID_Quotation
 inner join QuotationDetails qd on qd.ID_Quotation=qi.ID
 inner join MacroItems mi on mi.ID = qd.MacroItemKey
 inner join MacroItemDetails md on md.ID_MacroItem = mi.ID
 inner join PickingItems pi on (pi.ID = qd.CommonKey or pi.ID = md.CommonKey)
 inner join Types ti on ti.Code = pi.TypeCode
 inner join ItemTypes ii on ii.Code = pi.ItemTypeCode
 inner join Types ti1 on ti1.Code = mi.TypeCode
 inner join ItemTypes ii1 on ii1.Code = mi.ItemTypeCode
 where 
 ((pi.TypeCode in (31) and pi.ItemTypeCode in (10,65))) 


 union all

 select poi.ID poiid, poi.Description,  poi.Quantity poiquantity, qi.ID idqi, qi.Subject, qi.Q1, qd.id qdid, qd.ItemTypeDescription, qd.Quantity qdquantity, null miid, null MacroItemDescription, null mdid, 0 mdquantity, pi.ID piid, pi.ItemDescription,
 ti.Code ticode, ti.Description tidescription, ii.code iicode, ii.Description iidescription , null ti1description, null ii1description, pi.UM, qd.Quantity*(case when qd.Multiply = 0 or qi.Q1 = 0 or poi.Quantity = 1 then 1 else poi.Quantity/cast(qi.Q1 as real) end) *60 ProdTimeMin
 from 
 productionOrders poi
 inner join Quotations qi on qi.ID = poi.ID_Quotation
 inner join QuotationDetails qd on qd.ID_Quotation=qi.ID
 inner join PickingItems pi on (pi.ID = qd.CommonKey )
 inner join Types ti on ti.Code = pi.TypeCode
 inner join ItemTypes ii on ii.Code = pi.ItemTypeCode
 
 where 
 ((pi.TypeCode in (31) and pi.ItemTypeCode in (10,65)))
GO
/****** Object:  Table [dbo].[CustomerNicknames]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerNicknames](
	[Code] [int] NOT NULL,
	[Nickname] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CustomerNicknames] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionMPSExceptions]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionMPSExceptions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDProductionOrder] [int] NULL,
	[IDPickingItem] [int] NULL,
	[IDMacroItem] [int] NULL,
	[IDMacroItemDetail] [int] NULL,
	[IDQuotationDetail] [int] NULL,
	[OldIDProductionMachine] [int] NULL,
	[OldNumProductionMachine] [int] NULL,
	[OldOrder] [nvarchar](50) NULL,
	[NewIDProductionMachine] [int] NULL,
	[NewNumProductionMachine] [int] NULL,
	[NewOrder] [nvarchar](50) NULL,
 CONSTRAINT [PK_ProductionMPSExceptions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionTimeStamps]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionTimeStamps](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDProductionOrder] [int] NULL,
	[MinIDQuotationDetail] [int] NULL,
	[ProdStart] [datetime] NULL,
	[ProdEnd] [datetime] NULL,
	[IdUser] [int] NULL,
	[TotMin]  AS (case when [prodend] IS NOT NULL then datediff(minute,[prodstart],[prodend]) else datediff(minute,[prodstart],getdate()) end),
 CONSTRAINT [PK_ProductionTimeStamps] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_ProductionExtMPS_GroupedByPhase]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE VIEW [dbo].[VW_ProductionExtMPS_GroupedByPhase]
AS


SELECT min(mp.[ID]) ID
      ,mp.[IDProductionOrder]
	  ,po.Number
	  ,po.Description poDescription
	  ,po.ID_Manager ID_Company
	  ,po.ID_Customer
	  ,coalesce(nn.nickname, cu.Name) cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
      --,mp.[IDMacroItemDetail]
	  --,mi.MacroItemDescription
	  --,qd.ID_Quotation
      ,min(mp.[IDQuotationDetail]) [IDQuotationDetail]
	  --,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description deDescription
      --,min(mp.[Order]) [Order]
	  ,case when min(mp.[IDQuotationDetail]) = -1 then '99' else min(coalesce(mpe.NewOrder, mp.[Order])) end [Order]
      ,min(mp.[ProdStart]) [ProdStart]
      ,min(mp.[Priority]) [Priority]
      ,sum(mp.[ProdTimeMin]) [ProdTimeMin]
	  ,dbo.IntToMinutes(sum([ProdTimeMin])) ProdTime
      ,max(mp.[ProdEnd]) [ProdEnd]
	  ,(select sum(TotMin) from ProductionTimeStamps where IDProductionOrder = mp.IDProductionOrder and MinIDQuotationDetail = min(mp.[IDQuotationDetail])) [ProdEffMin]
	  ,(select count(1) from ProductionTimeStamps where IDProductionOrder = mp.IDProductionOrder and MinIDQuotationDetail = min(mp.[IDQuotationDetail]) and ProdEnd is null) [isInLav]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription
	  ,qt.Note qtNote
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11, 15) order by ProdEnd) curMachineId
	  ,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11,15) order by ProdEnd) ) curMachineDescription
	  ,(select top 1 IDQuotationDetail from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseQuotationDetail
	  ,(select top 1 Status from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseStatus
	  ,(select top 1 ID from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseID
	  ,(select isnull(max(OkCopiesCount),0) from ProductionOrderDetails where ID_ProductionOrder = po.ID and ID_Phase = mp.IDPickingItem) OkCopiesCount
	  --,pm.ID_ExternalCompany
	  ,pm.ID_Company ID_ExternalCompany
	  ,cp.Description ExternalCompanyDescription

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  --inner join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join Quotations qt on po.ID_Quotation = qt.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  --inner join Departments de on pm.IDDepartment = de.ID
  left join Departments de on pm.IDDepartment = de.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3
  --left join Companies cp on cp.ID = pm.ID_ExternalCompany
  left join Companies cp on cp.ID = pm.ID_Company
  --left join Managers ma on ma.ID = po.ID_Manager
  left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail
  left join CustomerNicknames nn on nn.Code = po.ID_Customer


  GROUP BY 

  --mp.[ID]
      --,
	  mp.[IDProductionOrder]
	  ,po.ID
	  ,po.Number
	  ,po.Description
	  ,po.ID_Manager --po.ID_Company
	  ,po.ID_Customer
	  ,coalesce(nn.nickname, cu.Name)
	  ,po.Status
	  ,st.Description
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
--      ,mp.[IDMacroItemDetail]
--	  ,mi.MacroItemDescription
--	  ,qd.ID_Quotation
--      ,mp.[IDQuotationDetail]
--	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description
      --,mp.[Order]
      --,mp.[ProdStart]
      --,mp.[Priority]
      --,mp.[ProdTimeMin]
      --,mp.[ProdEnd]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description
	  ,qt.Note
	  --,pm.ID_ExternalCompany
	  ,pm.ID_Company
	  ,cp.Description

	  --having (min(mp.status) <> 12 or max(mp.status) <> 12)
GO
/****** Object:  View [dbo].[VW_DD_ProductionMPS]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE VIEW [dbo].[VW_DD_ProductionMPS]
AS


SELECT 
       [IDProductionOrder]
      ,[IDPickingItem]
      ,min([IDQuotationDetail]) [IDQuotationDetail]
      ,[IDProductionMachine]
      ,[NumProductionMachine]
      ,min([Order]) [Order]
      ,min([Priority]) [Priority]
      ,min([Status]) [Status]
  FROM [dbo].[ProductionMPS]

  group by 

  
       [IDProductionOrder]
      ,[IDPickingItem]
      ,[IDProductionMachine]
      ,[NumProductionMachine]
      ,[Status]
GO
/****** Object:  View [dbo].[VW_ProductionExtMPS_new]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[VW_ProductionExtMPS_new]
AS


SELECT mp.[ID]
      ,mp.[IDProductionOrder]
	  ,po.Description poDescription
	  ,po.ID_Customer
	  ,cu.Name cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
	  ,mp.IDMacroItem
      ,mp.[IDMacroItemDetail]
	  ,mi.MacroItemDescription
	  ,qd.ID_Quotation
      ,mp.[IDQuotationDetail]
	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description deDescription
      ,case when mp.[IDQuotationDetail] = -1 then '99' else coalesce(
	  (select min(NewOrder) from ProductionMPSExceptions where IDQuotationDetail = mp.IDQuotationDetail group by IDQuotationDetail)
	  , mp.[Order]) end [Order]
      ,mp.[ProdStart]
      ,mp.[Priority]
      ,mp.[ProdTimeMin]
	  ,dbo.IntToMinutes([ProdTimeMin]) ProdTime
      ,mp.[ProdEnd]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription
	  ,qt.Note qtNote
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11, 15) order by ProdEnd) curMachineId
	  ,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11,15) order by ProdEnd) ) curMachineDescription
	  ,(select top 1 IDQuotationDetail from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseQuotationDetail
	  ,(select top 1 Status from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseStatus
	  ,(select top 1 ID from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseID

	  ,(select isnull(max(OkCopiesCount),0) from ProductionOrderDetails where ID_ProductionOrder = po.ID and ID_Phase = mp.IDPickingItem) OkCopiesCount
	  ,pm.ID_ExternalCompany
	  ,cp.Description ExternalCompanyDescription

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  --inner join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join Quotations qt on po.ID_Quotation = qt.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  inner join Departments de on pm.IDDepartment = de.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3

  left join Companies cp on cp.ID = pm.ID_ExternalCompany
  --left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail
GO
/****** Object:  Table [dbo].[LeaveTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveTypes](
	[ID] [int] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_LeaveTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NOT NULL,
 CONSTRAINT [PK_StatusTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DayFractions]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DayFractions](
	[ID] [char](1) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_DayFractions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveRequests]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveRequests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_Company] [int] NULL,
	[ID_Applicant] [int] NULL,
	[LeaveType] [int] NULL,
	[RequestDate] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[DayFraction] [char](1) NULL,
	[VacationDays] [int] NULL,
	[MessageToManager] [varchar](1024) NULL,
	[ID_Manager] [int] NULL,
	[Status] [int] NULL,
	[StatusDate] [datetime] NULL,
	[ID_Approver] [int] NULL,
	[MessageToApplicant] [varchar](1024) NULL,
 CONSTRAINT [PK_LeaveRequests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_LeaveRequests]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[VW_LeaveRequests]
AS

SELECT lr.[ID]
      ,lr.ID_Company
      ,cp.Description CompanyDesc
      ,lr.[ID_Applicant]
      ,ep1.UniqueName ApplicantDesc
      ,lr.[LeaveType]
      ,lt.Description LeaveTypeDesc 
      ,lr.[RequestDate]
      ,lr.[StartDate]
      ,lr.[EndDate]
      ,lr.[DayFraction]
      ,df.Description DayFractionDesc
      ,lr.[VacationDays]
      ,lr.[MessageToManager]
      ,lr.[ID_Manager]
      ,ep2.UniqueName ManagerDesc
      ,lr.[Status]
      ,s.Description StatusDesc
      ,lr.[StatusDate]
      ,lr.[ID_Approver]
      ,ep3.UniqueName ApproverDesc
      ,lr.[MessageToApplicant]

  FROM [dbo].[LeaveRequests] lr

    inner join LeaveTypes lt on lr.LeaveType = lt.ID
    inner join Employees ep1 on ep1.ID= lr.ID_Applicant
    inner join Employees ep2 on ep2.ID= lr.ID_Manager
    left join Employees ep3 on ep3.ID= lr.ID_Approver
    inner join Companies cp on cp.ID= lr.ID_Company
    inner join Statuses s on s.ID = lr.Status
    inner join StatusTypes st on st.ID = s.StatusType and st.ID = 5
    inner join DayFractions df on df.ID = lr.DayFraction


GO
/****** Object:  View [dbo].[VW_ProductionOrderDetailsConsumption]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE   VIEW [dbo].[VW_ProductionOrderDetailsConsumption]
AS
SELECT a.[ID]
      ,a.[ID_ProductionOrder]
	  ,po.Number
      ,a.[ID_PickingItem]
	  ,pir.ItemDescription ID_PickingItemDesc
	  ,pir.[Order]
	  ,pir.ID_Company
	  ,cp.Description CompanyDescription
	  ,pir.TypeCode
	  ,tp.Description TypeDescription
	  ,pir.ItemTypeCode 
	  ,itp.Description ItemTypeDescription
      ,a.[SupplierCode]
	  ,su.Name
      ,pir.UM
	  ,un.Description UMDescription
      ,isnull(a.[RawMaterialQuantity],1.0) [RawMaterialQuantity]
	  ,isnull(a.RawMaterialQuantity,1.0) * pir.Cost CurrentCost
	  ,a.HistoricalCostRawM HistoricalCost
	  ,a.ProductionDate
	  ,YEAR(ProductionDate) AS YearProductionDate
	  ,MONTH(ProductionDate) AS MonthProductionDate 

  FROM ProductionOrderDetails a 

  inner join ProductionOrders po on po.ID = a.ID_ProductionOrder
  inner join PickingItems pir on pir.ID = a.ID_PickingItem
  inner join Types tp on pir.TypeCode = tp.Code
  inner join ItemTypes itp on pir.ItemTypeCode = itp.Code
  inner join Units un on pir.UM = un.ID
  left join Suppliers su on a.SupplierCode = su.Code
  inner join Companies cp on cp.ID= pir.ID_Company

  where a.ID_PickingItem is not null and pir.UM <> 3 and rflag is null
 

  union all


  SELECT a.[ID]
     ,a.[ID_ProductionOrder]
	 ,po.Number
     ,a.[ID_PickingItemSup] [ID_PickingItem]
	 ,pis.ItemDescription ID_PickingItemDesc
	 ,pis.[Order]
	 ,pis.ID_Company
	 ,cp.Description CompanyDescription
	 ,pis.TypeCode
	 ,tp.Description TypeDescription
	 ,pis.ItemTypeCode 
	 ,itp.Description ItemTypeDescription
     ,a.[SupplierCodeSup] [SupplierCode]
	 ,su.Name
     ,pis.UM [UMRawMaterial]
	 ,un.Description UMDescription
     ,isnull(a.[RawMaterialX],1.0) * isnull(a.[RawMaterialY],1.0) * isnull(a.[RawMaterialZ],1.0) [RawMaterialQuantity]
	 ,isnull(a.[RawMaterialX],1.0) * isnull(a.[RawMaterialY],1.0) * isnull(a.[RawMaterialZ],1.0) * pis.Cost CurrentCost
	 ,a.HistoricalCostSupM HistoricalCost
	 ,a.ProductionDate
	 ,YEAR(ProductionDate) AS YearProductionDate
	 ,MONTH(ProductionDate) AS MonthProductionDate 

	
  FROM ProductionOrderDetails a 

  inner join ProductionOrders po on po.ID = a.ID_ProductionOrder
  inner join PickingItems pis on pis.ID = a.ID_PickingItemSup
  inner join Types tp on pis.TypeCode = tp.Code
  inner join ItemTypes itp on pis.ItemTypeCode = itp.Code
  inner join Units un on pis.UM = un.ID
  left join Suppliers su on a.SupplierCodeSup = su.Code
  inner join Companies cp on cp.ID= pis.ID_Company

   where a.ID_PickingItemSup is not null and pis.UM <> 3 and sflag is null
GO
/****** Object:  Table [dbo].[PlasticCoatingMachineParameters]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlasticCoatingMachineParameters](
	[Id_ProductionOrder] [int] NOT NULL,
	[LATI] [int] NULL,
	[BASE_1] [int] NULL,
	[ALTEZZA_1] [int] NULL,
 CONSTRAINT [PK_PlasticCoatingMachineParameters] PRIMARY KEY CLUSTERED 
(
	[Id_ProductionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EuroProgetti_DB_Ordini]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EuroProgetti_DB_Ordini](
	[i_ID] [int] NOT NULL,
	[i_Cliente] [nvarchar](50) NULL,
	[i_Codice] [nvarchar](50) NULL,
	[i_o_Ricetta] [nvarchar](50) NULL,
	[o_Pezzi] [int] NULL,
	[o_FogliIncollati] [int] NULL,
	[o_Cartoni] [int] NULL,
	[o_Ribordati] [int] NULL,
	[o_Scarti] [int] NULL,
	[o_TimeEmergenza] [nvarchar](50) NULL,
	[o_Prodpezzi] [nvarchar](50) NULL,
	[o_TimeArresto] [nvarchar](50) NULL,
	[o_TimeAvviamento] [nvarchar](50) NULL,
	[o_TimeProduzione] [nvarchar](50) NULL,
	[o_TimeRegistrazione] [nvarchar](50) NULL,
	[o_BitEmergenza] [bit] NULL,
	[o_BitArresto] [bit] NULL,
	[i_Quantita] [int] NULL,
	[o_BitAvviamento] [bit] NULL,
	[o_BitProduzione] [bit] NULL,
	[Articolo] [nvarchar](50) NULL,
	[o_DataInizio] [nvarchar](50) NULL,
	[o_OraInizio] [nvarchar](50) NULL,
	[o_DataFine] [nvarchar](50) NULL,
	[o_OraFine] [nvarchar](50) NULL,
	[Giorno] [nvarchar](50) NULL,
	[Record] [bit] NULL,
	[i_o_Sospensione] [nvarchar](50) NULL,
	[i_Passaggi] [int] NULL,
	[i_Larghezza] [int] NULL,
	[i_Lunghezza] [int] NULL,
	[o_pezzi_2] [int] NULL,
	[o_FogliIncollati_2] [int] NULL,
	[o_Cartoni_2] [int] NULL,
	[o_Ribordati_2] [int] NULL,
	[o_Scarti_2] [int] NULL,
	[o_TimeEmergenza_2] [nvarchar](50) NULL,
	[o_ProdPezzi_2] [nvarchar](50) NULL,
	[o_TimeArresto_2] [nvarchar](50) NULL,
	[o_TimeAvviamento_2] [nvarchar](50) NULL,
	[o_TimeProduzione_2] [nvarchar](50) NULL,
	[o_TimeRegistrazione_2] [nvarchar](50) NULL,
	[o_BitEmergenza_2] [bit] NULL,
	[o_BitArresto_2] [bit] NULL,
	[o_BitAvviamento_2] [bit] NULL,
	[o_BitProduzione_2] [bit] NULL,
	[o_DataInizio_2] [nvarchar](50) NULL,
	[o_OraInizio_2] [nvarchar](50) NULL,
	[o_DataFine_2] [nvarchar](50) NULL,
	[o_OraFine_2] [nvarchar](50) NULL,
	[i_Larghezza_2] [int] NULL,
	[i_Lunghezza_2] [int] NULL,
	[LabextimStatus] [int] NULL,
	[LabextimID_OdP] [int] NULL,
 CONSTRAINT [PK_EuroProgetti_DB_Ordini] PRIMARY KEY CLUSTERED 
(
	[i_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[S7Data]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[S7Data](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OdpMacchina] [int] NOT NULL,
	[ID_Odp] [int] NOT NULL,
	[Titolo] [varchar](30) NULL,
	[CopieDaProdurre] [int] NULL,
	[CopieProdotte] [int] NULL,
	[MetriLineariLavorati] [int] NULL,
	[MinutiMacchinaAccesa] [int] NULL,
	[MinutiMacchinaInPassaggio] [int] NULL,
	[MinutiMacchinaInPressa] [int] NULL,
	[DataOraInizio] [datetime] NULL,
	[DataOraFine] [datetime] NULL,
	[Stato] [int] NULL,
 CONSTRAINT [PK_S7Data] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_PlasticCoatingMachineStats]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE VIEW [dbo].[VW_PlasticCoatingMachineStats]
AS
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.COMMESSA as int) OdP, po.Number, po.ID_Customer, st.CLIENTE DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(st.FOGLI_ENTRATI) CopieInput, MAX(st.FOGLI_USCITI) CopieOutput, MIN(st.DATA) StartDate, MAX(st.DATA) EndDate
,(select top 1 STATO from ECOSYSTEM.[dbo].[STATI] s1 where s1.commessa=po.id order by s1.ID desc ) Stato
FROM ECOSYSTEM.[dbo].[STATI] st
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.COMMESSA
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID = 19
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
STATO <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.COMMESSA, po.Number, po.ID_Customer, po.ID, po.Description, st.CLIENTE, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity

UNION ALL

SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.i_ID as int) OdP, po.Number, po.ID_Customer, st.i_Cliente DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(st.i_Quantita) CopieInput, MAX(st.o_Prodpezzi) CopieOutput,
MIN(DATEADD(day, DATEDIFF(day, 0, st.o_DataInizio), st.o_OraInizio)) StartDate,
MAX(DATEADD(day, DATEDIFF(day, 0, st.o_DataFine), st.o_OraFine)) EndDate
,(select top 1 LabextimStatus from EuroProgetti_DB_Ordini s1 where s1.LabextimID_Odp=po.ID order by s1.I_ID desc ) Stato
FROM EuroProgetti_DB_Ordini st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.LabextimID_Odp
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (77)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.LabextimStatus <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.i_ID, po.Number, po.ID_Customer, po.ID, po.Description, st.i_Cliente, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity

UNION ALL

SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.ID_Odp as int) OdP, po.Number, po.ID_Customer, cu.Name DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(st.CopieDaProdurre) CopieInput, SUM(st.CopieProdotte) CopieOutput,
MIN(st.DataOraInizio) StartDate,
MAX(st.DataOraFine) EndDate
,(select top 1 LabextimStatus from EuroProgetti_DB_Ordini s1 where s1.LabextimID_Odp=po.ID order by s1.I_ID desc ) Stato
FROM S7Data st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.ID_Odp
left join Customers cu on cu.Code = po.ID_Customer
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (77)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.Stato <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.ID_Odp, po.Number, po.ID_Customer, po.ID, po.Description, cu.Name, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity

GO
/****** Object:  Table [dbo].[ZechiniData]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZechiniData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NomeFile] [varchar](50) NULL,
	[DataFile] [datetime] NULL,
	[Commessa] [varchar](15) NULL,
	[PzRichiesti] [int] NULL,
	[PzFatti] [int] NULL,
	[Inizio] [datetime] NULL,
	[Fine] [datetime] NULL,
	[tMacchina] [time](7) NULL,
	[pzMephisto] [int] NULL,
	[Stato] [int] NULL,
	[DatVar] [datetime] NULL,
 CONSTRAINT [PK_ZechiniData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SarogliaData]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SarogliaData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NomeFile] [varchar](50) NULL,
	[DataFile] [datetime] NULL,
	[Commessa] [varchar](15) NULL,
	[Descrizione] [varchar](50) NULL,
	[StampaACaldo] [bit] NULL,
	[Fustellatura] [bit] NULL,
	[PzRichiesti] [int] NULL,
	[PzFatti] [int] NULL,
	[PzScarto] [int] NULL,
	[Inizio] [datetime] NULL,
	[Fine] [datetime] NULL,
	[Completato] [bit] NULL,
	[tMacchina] [time](7) NULL,
	[Stato] [int] NULL,
	[DatVar] [datetime] NULL,
 CONSTRAINT [PK_SarogliaData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_PlasticCoatingMachineStats_new]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

























CREATE VIEW [dbo].[VW_PlasticCoatingMachineStats_new]
AS

--ECOSYSTEM
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.COMMESSA as int) OdP, po.Number, po.ID_Customer, st.CLIENTE DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity,
eo.N_FOGLI CopieInput,
MAX(st.FOGLI_ENTRATI) CopieOutput, MIN(st.DATA) StartDate, MAX(st.DATA) EndDate,
--,(select top 1 STATO from ECOSYSTEM.[dbo].[STATI] s1 where s1.commessa=po.id order by s1.ID desc ) Stato
eo.Stato
, null MetriLineariLavorati,  null [MinutiMacchinaAccesa], null [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], null [MinutiMacchinaInPressa], null [MinutiMacchinaProduzione]
FROM ECOSYSTEM.[dbo].[STATI] st
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.COMMESSA
INNER JOIN ECOSYSTEM..ORDINI eo on eo.COMMESSA = ST.COMMESSA
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID = 19
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
st.STATO <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.COMMESSA, po.Number, po.ID_Customer, po.ID, po.Description, st.CLIENTE, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity, eo.STATO,
eo.N_FOGLI

UNION ALL

--EUROPROGETTI
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.LabextimID_OdP as int) OdP, po.Number, po.ID_Customer, st.i_Cliente DescCustomer, po.Description poDescription, isnull(pc.LATI, st.i_passaggi) NumLati, isnull(pc.BASE_1, st.i_larghezza) Base, isnull(pc.ALTEZZA_1, st.i_Lunghezza) Altezza, po.Quantity, MAX(st.i_Quantita) CopieInput, MAX(st.o_Pezzi) CopieOutput
--MIN(DATEADD(day, DATEDIFF(day, 0, st.o_DataInizio), st.o_OraInizio)) StartDate,
--MAX(DATEADD(day, DATEDIFF(day, 0, st.o_DataFine), st.o_OraFine)) EndDate
,MIN(DATEADD(day, DATEDIFF(day, 0, CONVERT(DATE,st.o_DataInizio,104)), st.o_OraInizio)) StartDate
,MAX(DATEADD(day, DATEDIFF(day, 0, CONVERT(DATE, st.o_DataFine, 104)), st.o_OraFine)) EndDate
,(select top 1 LabextimStatus from EuroProgetti_DB_Ordini s1 where s1.LabextimID_Odp=po.ID order by s1.I_ID desc ) Stato
, null MetriLineariLavorati, sum(DATEDIFF(second,0,cast(st.o_TimeRegistrazione as datetime))/60) [MinutiMacchinaAccesa], sum(DATEDIFF(second,0,cast(st.o_TimeAvviamento	 as datetime))/60) [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], null [MinutiMacchinaInPressa], sum(DATEDIFF(second,0,cast(st.o_TimeProduzione as datetime))/60) [MinutiMacchinaProduzione]
FROM EuroProgetti_DB_Ordini st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.LabextimID_Odp
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (77)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.LabextimStatus <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.LabextimID_OdP, po.Number, po.ID_Customer, po.ID, po.Description, st.i_Cliente, pc.LATI, st.i_Passaggi, pc.BASE_1, st.i_Larghezza, pc.ALTEZZA_1, st.i_Lunghezza ,po.Quantity

UNION ALL

--EUROPROGETTI
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.LabextimID_OdP as int) OdP, po.Number, po.ID_Customer, st.i_Cliente DescCustomer, po.Description poDescription, isnull(pc.LATI, st.i_passaggi) NumLati, isnull(pc.BASE_1, st.i_larghezza_2) Base, isnull(pc.ALTEZZA_1, st.i_Lunghezza_2) Altezza, po.Quantity, MAX(st.i_Quantita) CopieInput, MAX(st.o_pezzi_2) CopieOutput
--MIN(DATEADD(day, DATEDIFF(day, 0, st.o_DataInizio), st.o_OraInizio)) StartDate,
--MAX(DATEADD(day, DATEDIFF(day, 0, st.o_DataFine), st.o_OraFine)) EndDate
,MIN(DATEADD(day, DATEDIFF(day, 0, CONVERT(DATE,st.o_DataInizio_2,104)), st.o_OraInizio_2)) StartDate
,MAX(DATEADD(day, DATEDIFF(day, 0, CONVERT(DATE, st.o_DataFine_2, 104)), st.o_OraFine_2)) EndDate
,(select top 1 LabextimStatus from EuroProgetti_DB_Ordini s1 where s1.LabextimID_Odp=po.ID order by s1.I_ID desc ) Stato
, null MetriLineariLavorati, sum(DATEDIFF(second,0,cast(st.o_TimeRegistrazione_2 as datetime))/60) [MinutiMacchinaAccesa], sum(DATEDIFF(second,0,cast(st.o_TimeAvviamento_2	 as datetime))/60) [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], null [MinutiMacchinaInPressa], sum(DATEDIFF(second,0,cast(st.o_TimeProduzione_2 as datetime))/60) [MinutiMacchinaProduzione]
FROM EuroProgetti_DB_Ordini st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.LabextimID_Odp
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (77)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.LabextimStatus <> 0 and i_Passaggi = 2
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.LabextimID_OdP, po.Number, po.ID_Customer, po.ID, po.Description, st.i_Cliente, pc.LATI, st.i_Passaggi, pc.BASE_1, st.i_Larghezza_2, pc.ALTEZZA_1, st.i_Lunghezza_2 ,po.Quantity

UNION ALL

--SILKFOIL
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , cast(st.ID_Odp as int) OdP, po.Number, po.ID_Customer, cu.Name DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(st.CopieDaProdurre) CopieInput, SUM(st.CopieProdotte) CopieOutput,
MIN(st.DataOraInizio) StartDate,
MAX(st.DataOraFine) EndDate
,(select top 1 Stato from S7Data s1 where s1.ID_Odp=po.ID order by s1.ID desc ) Stato
, max(MetriLineariLavorati) MetriLineariLavorati, max([MinutiMacchinaAccesa]) [MinutiMacchinaAccesa],null [MinutiMacchinaAvviamento],  max([MinutiMacchinaInPassaggio]) [MinutiMacchinaInPassaggio] , max([MinutiMacchinaInPressa]) [MinutiMacchinaInPressa], null [MinutiMacchinaProduzione]
FROM S7Data st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = st.ID_Odp
left join Customers cu on cu.Code = po.ID_Customer
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (104)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.Stato <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, st.ID_Odp, po.Number, po.ID_Customer, po.ID, po.Description, cu.Name, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity

UNION ALL

--ZECHINI
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end) OdP, po.Number, po.ID_Customer, cu.Name DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(case when st.PzRichiesti = -1 then po.Quantity else st.PzRichiesti end) CopieInput, SUM(st.PzFatti) CopieOutput,
MIN(st.Inizio) StartDate,
MAX(st.Fine) EndDate
,(select top 1 Stato from ZechiniData s1 where (case when len(s1.Commessa) <=6 then Cast(s1.commessa as int) else cast(left(s1.Commessa, CHARINDEX(' ' , s1.Commessa, 1)) as int) end) =po.ID order by s1.Commessa desc ) Stato
, null MetriLineariLavorati, null [MinutiMacchinaAccesa], null [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], sum(DATEDIFF(second,0,st.tMacchina)/60)  [MinutiMacchinaInPressa], sum(DATEDIFF(second,0,st.tMacchina)/60) [MinutiMacchinaProduzione]
FROM ZechiniData st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end)
left join Customers cu on cu.Code = po.ID_Customer
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (107)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.Stato <> 0
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end), po.Number, po.ID_Customer, po.ID, po.Description, cu.Name, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity


UNION ALL

--SAROGLIA FUSTELLA
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end) OdP, po.Number, po.ID_Customer, cu.Name DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(case when st.PzRichiesti = -1 then po.Quantity else st.PzRichiesti end) CopieInput, SUM(st.PzFatti) CopieOutput,
MIN(st.Inizio) StartDate,
MAX(st.Fine) EndDate
,(select top 1 Stato from SarogliaData s1 where (case when len(s1.Commessa) <=6 then Cast(s1.commessa as int) else cast(left(s1.Commessa, CHARINDEX(' ' , s1.Commessa, 1)) as int) end) =po.ID and Fustellatura=1 order by s1.Commessa desc ) Stato
, null MetriLineariLavorati, null [MinutiMacchinaAccesa], null [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], sum(DATEDIFF(second,0,st.tMacchina)/60)  [MinutiMacchinaInPressa], sum(DATEDIFF(second,0,st.tMacchina)/60) [MinutiMacchinaProduzione]
FROM SarogliaData st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end)
left join Customers cu on cu.Code = po.ID_Customer
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (15)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.Stato <> 0 and st.Fustellatura = 1
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end), po.Number, po.ID_Customer, po.ID, po.Description, cu.Name, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity

UNION ALL

--SAROGLIA STAMPA CALDO
SELECT pm.id IdMachine, pm.Description DescMachine, po.ID_Company, cp.Description DescCompany , (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end) OdP, po.Number, po.ID_Customer, cu.Name DescCustomer, po.Description poDescription, pc.LATI NumLati, pc.BASE_1 Base, pc.ALTEZZA_1 Altezza, po.Quantity, MAX(case when st.PzRichiesti = -1 then po.Quantity else st.PzRichiesti end) CopieInput, SUM(st.PzFatti) CopieOutput,
MIN(st.Inizio) StartDate,
MAX(st.Fine) EndDate
,(select top 1 Stato from SarogliaData s1 where (case when len(s1.Commessa) <=6 then Cast(s1.commessa as int) else cast(left(s1.Commessa, CHARINDEX(' ' , s1.Commessa, 1)) as int) end) =po.ID and StampaACaldo=1 order by s1.Commessa desc ) Stato
, null MetriLineariLavorati, null [MinutiMacchinaAccesa], null [MinutiMacchinaAvviamento], null [MinutiMacchinaInPassaggio], sum(DATEDIFF(second,0,st.tMacchina)/60)  [MinutiMacchinaInPressa], sum(DATEDIFF(second,0,st.tMacchina)/60) [MinutiMacchinaProduzione]
FROM SarogliaData st
--INNER JOIN LabExtim..ProductionOrders po ON cast(cast(po.ID_Company as varchar) + substring(po.Number, 3,2) + SUBSTRING(po.Number,6, 10) as int) = st.i_ID
INNER JOIN LabExtim..ProductionOrders po ON po.ID = (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end)
left join Customers cu on cu.Code = po.ID_Customer
LEFT JOIN LabExtim..PlasticCoatingMachineParameters pc ON pc.Id_ProductionOrder = po.ID
inner join ProductionMachines pm on pm.ID in (101)
inner join Companies cp on cp.ID= po.ID_Company
WHERE 
ST.Stato <> 0 and st.StampaACaldo = 1
GROUP BY pm.ID, pm.Description, po.ID_Company,cp.Description, (case when len(st.Commessa) <=6 then Cast(st.commessa as int) else cast(left(st.Commessa, CHARINDEX(' ' , st.Commessa, 1)) as int) end), po.Number, po.ID_Customer, po.ID, po.Description, cu.Name, pc.LATI, pc.BASE_1, pc.ALTEZZA_1, po.Quantity




GO
/****** Object:  Table [dbo].[Managers]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[ID] [int] NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_QuotationDetailsUnusedOnProduction]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  VIEW [dbo].[VW_QuotationDetailsUnusedOnProduction]
AS

select cu.Code ID_Customer, cu.Name CustomerName, qd.ID_Quotation, qt.Subject, qt.ID_Manager, e.Description ManagerDesc, qd.CommonKey, qd.MacroItemKey, su.Name Supplier, qd.ItemTypeDescription, u.Description UMDesc,  qd.Quantity, po.ID ID_ProductionOrder, po.StartDate, po.DeliveryDate, isnull(po.UnusedProductsCheck,0) UnusedProductsCheck
 
from QuotationDetails qd
inner join Quotations qt on qd.ID_Quotation= qt.ID
inner join Customers cu on cu.Code= qt.CustomerCode
inner join Units u on u.ID = qd.UM
inner join Suppliers su on su.Code= qd.SupplierCode
inner join ProductionOrders po on po.ID_Quotation = qt.ID
left join Managers e on e.ID = qt.ID_Manager
where not exists 
(
--pickingitem raw
select 1
from
ProductionOrderDetails pod 
inner join ProductionOrders po on po.id = pod.ID_ProductionOrder
inner join PickingItems pi on pi.ID= pod.ID_PickingItem
inner join Units u on u.ID = pod.UMRawMaterial
where 1=1
and pi.TypeCode = 6
and RFlag is null
and po.ID_Quotation = qd.ID_Quotation
and pod.ID_PickingItem = qd.CommonKey

union 

--pickingitem sup
select 1
from
ProductionOrderDetails pod 
inner join ProductionOrders po on po.id = pod.ID_ProductionOrder
inner join PickingItems pi on pi.ID= pod.ID_PickingItemSup
left join Units u on u.ID = pod.UMRawMaterial
where 1=1
and pi.TypeCode = 6
and SFlag is null
and po.ID_Quotation = qd.ID_Quotation
and pod.ID_PickingItemSup = qd.CommonKey

union

--macroitem raw
select 1
from
ProductionOrderDetails pod 
inner join ProductionOrders po on po.id = pod.ID_ProductionOrder
inner join MacroItems pi on pi.ID= pod.ID_PickingItem
inner join Units u on u.ID = pod.UMRawMaterial
where 1=1
and pi.TypeCode = 6
and RFlag = 'M'
and po.ID_Quotation = qd.ID_Quotation
and pod.ID_PickingItem = qd.MacroItemKey

union 

--macroitem sup
select 1
from
ProductionOrderDetails pod 
inner join ProductionOrders po on po.id = pod.ID_ProductionOrder
inner join MacroItems pi on pi.ID= pod.ID_PickingItemSup
left join Units u on u.ID = pod.UMRawMaterial
where 1=1
and pi.TypeCode = 6
and SFlag = 'M'
and po.ID_Quotation = qd.ID_Quotation
and pod.ID_PickingItemSup = qd.MacroItemKey







) 
and qd.TypeCode=6
and qd.CommonKey is not null
and po.Status = 3
GO
/****** Object:  View [dbo].[Conteggio uso macrovoci]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Conteggio uso macrovoci]
AS
SELECT top 1000000 year(productiondate) Anno, ID_PickingItem Macrovoce , m.MacroItemDescription descrizione,
count(1) conteggio




  FROM [dbo].[ProductionOrderDetails]

  inner join MacroItems m on m.ID=ID_PickingItem


  where RFlag = 'M'

  group by 
  year(productiondate),
  ID_PickingItem,
  m.MacroItemDescription

  order by 2,1,3
GO
/****** Object:  View [dbo].[VW_ProductionOrderDetailsConsumptionMCount]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[VW_ProductionOrderDetailsConsumptionMCount]
AS
SELECT 

pd.ID_Company,
t.Code TypeCode,
 t.Description TypeDescription,
  it.Code ItemTypeCode,
   it.Description ItemTypeDescription,
    pd.ID_PickingItem Macrovoce ,
	 m.MacroItemDescription descrizione,
	 pd.RawMaterialQuantity,
	 pd.ID_ProductionOrder,
	 po.Number,
	pd.ProductionDate
	  ,YEAR(pd.ProductionDate) AS YearProductionDate
	  ,MONTH(pd.ProductionDate) AS MonthProductionDate 




  FROM [dbo].[ProductionOrderDetails] pd
  inner join ProductionOrders po on po.ID = pd.ID_ProductionOrder
  inner join MacroItems m on m.ID=ID_PickingItem
  inner join Types t on t.Code = m.TypeCode
  inner join ItemTypes it on it.Code = m.ItemTypeCode


  where RFlag = 'M'
GO
/****** Object:  View [dbo].[VW_ProductionExtMPS_GroupedByPhase_Lite]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







































CREATE VIEW [dbo].[VW_ProductionExtMPS_GroupedByPhase_Lite]
AS


SELECT min(mp.[ID]) ID
      ,mp.[IDProductionOrder]
	  --,po.Number
	  --,po.Description poDescription
	  ,po.ID_Manager ID_Company
	  --,po.ID_Customer
	  --,coalesce(nn.nickname, cu.Name) cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      --,mp.[IDPickingItem]
	  --,pi.ItemDescription
      --,min(mp.[IDQuotationDetail]) [IDQuotationDetail]
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      --,mp.[NumProductionMachine]
	  --,pm.IDDepartment
	  --,de.Description deDescription
	  ,case when min(mp.[IDQuotationDetail]) = -1 then '99' else min(coalesce(mpe.NewOrder, mp.[Order])) end [Order]
      ,min(mp.[ProdStart]) [ProdStart]
      --,min(mp.[Priority]) [Priority]
      ,sum(mp.[ProdTimeMin]) [ProdTimeMin]
	  ,dbo.IntToMinutes(sum([ProdTimeMin])) ProdTime
      --,max(mp.[ProdEnd]) [ProdEnd]
	  ,(select sum(TotMin) from ProductionTimeStamps where IDProductionOrder = mp.IDProductionOrder and MinIDQuotationDetail = min(mp.[IDQuotationDetail])) [ProdEffMin]
	  ,(select count(1) from ProductionTimeStamps where IDProductionOrder = mp.IDProductionOrder and MinIDQuotationDetail = min(mp.[IDQuotationDetail]) and ProdEnd is null) [isInLav]
	  --,po.Quantity
	  --,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription
	  --,qt.Note qtNote
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11, 15) order by ProdEnd) curMachineId
	  --,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11,15) order by ProdEnd) ) curMachineDescription
	  --,(select top 1 IDQuotationDetail from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseQuotationDetail
	  --,(select top 1 Status from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseStatus
	  --,(select top 1 ID from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseID
	  ,(select isnull(max(OkCopiesCount),0) from ProductionOrderDetails where ID_ProductionOrder = po.ID and ID_Phase = mp.IDPickingItem) OkCopiesCount
	  ,pm.ID_Company ID_ExternalCompany
	  ,cp.Description ExternalCompanyDescription

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join Quotations qt on po.ID_Quotation = qt.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  --left join Departments de on pm.IDDepartment = de.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  --inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3
  left join Companies cp on cp.ID = pm.ID_Company
  left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail
  --left join CustomerNicknames nn on nn.Code = po.ID_Customer


  GROUP BY 

	  mp.[IDProductionOrder]
	  ,po.ID
	  --,po.Number
	  --,po.Description
	  ,po.ID_Manager --po.ID_Company
	  --,po.ID_Customer
	  --,coalesce(nn.nickname, cu.Name)
	  ,po.Status
	  ,st.Description
      ,mp.[IDPickingItem]
	  --,pi.ItemDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description
      --,mp.[NumProductionMachine]
	  --,pm.IDDepartment
	  --,de.Description
	  --,po.Quantity
	  --,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description
	  --,qt.Note
	  ,pm.ID_Company
	  ,cp.Description
GO
/****** Object:  View [dbo].[VW_ProductionExtMPS_Lite]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE VIEW [dbo].[VW_ProductionExtMPS_Lite]
AS


SELECT min(mp.[ID]) [ID]
      ,mp.[IDProductionOrder]
      ,mp.[IDPickingItem]
	  ,mp.IDMacroItem
	  ,mp.[IDMacroItemDetail]
      ,mp.[IDQuotationDetail]
      ,case when mp.[IDQuotationDetail] = -1 then '99' else min(coalesce(mpe.NewOrder, mp.[Order])) end [Order]
	  ,mp.Status

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail and coalesce(mpe.IDMacroItemDetail, mpe.IdPickingitem) = coalesce(mp.IDMacroItemDetail, mp.IdPickingitem)

  group by 

  --mp.[ID]
       mp.[IDProductionOrder]
      ,mp.[IDPickingItem]
	  ,mp.IDMacroItem
	  ,mp.[IDMacroItemDetail]
      ,mp.[IDQuotationDetail]
	  ,mp.Status
	  ,po.ID
GO
/****** Object:  Table [dbo].[QUOPORCostsPrices]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUOPORCostsPrices](
	[ID_Quotation] [int] NULL,
	[QUOOwner] [int] NULL,
	[OwnerName] [nvarchar](135) NULL,
	[QUOSubject] [nvarchar](200) NULL,
	[PORSQuantity] [float] NULL,
	[PORID_Customer] [int] NULL,
	[PORTotCost] [float] NULL,
	[PORTotHistoricalCost] [money] NULL,
	[QUOTotCost] [float] NULL,
	[PORSProducedQuantity] [float] NULL,
	[ID] [int] NOT NULL,
	[ID_Company] [int] NULL,
	[Number] [varchar](10) NULL,
	[Status] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AccountNote] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[IDAgente1] [int] NULL,
	[DescrizioneAgente1] [nvarchar](50) NULL,
	[FATTotValue] [money] NULL,
	[ProvvTotValue] [money] NULL,
	[DataBolla] [datetime] NULL,
	[TipoRec] [varchar](1) NULL,
	[PriceCom] [nvarchar](max) NULL,
	[NonConformityCode] [int] NULL,
	[ComplaintReceived] [int] NULL,
	[CorrectiveActionCode] [int] NULL,
 CONSTRAINT [PK_VW_QUOPORCostsPrices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_QUOPORCostsPrices]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE   VIEW [dbo].[VW_QUOPORCostsPrices]
AS

SELECT cp.[ID_Quotation]
      ,cp.[QUOOwner]
      ,cp.[OwnerName]
      ,isnull(cp.[QUOSubject],'Senza nome') [QUOSubject]
      ,cp.[PORSQuantity]
      ,cp.[PORID_Customer]
	  ,cu.Name CustomerName
      ,cp.[PORTotCost]
      ,cp.[PORTotHistoricalCost]
      ,cp.[QUOTotCost]
      ,cp.[PORSProducedQuantity]
      ,cp.[ID]
	  ,cp.ID_Company
      ,cp.[Number]
      ,cp.[Status]
      ,cp.[StartDate]
      ,cp.[EndDate]
      ,cp.[AccountNote]
      ,cp.[Note]
      ,cp.[IDAgente1]
      ,cp.[DescrizioneAgente1]
      ,cp.[FATTotValue]
      ,cp.[ProvvTotValue]
      ,cp.[DataBolla]
      ,cp.[TipoRec]
      ,cp.[PriceCom]
	  ,cp.NonConformityCode
	  ,nc.Description NonConformityDescription
	  ,po.ID_Manager
	  ,po.ComplaintReceived
	  ,po.CorrectiveActionCode
	  ,ca.Description CorrectiveActionDescription

  FROM [dbo].[QUOPORCostsPrices] cp

  inner join Customers cu on Code = cp.PORID_Customer
  left join NonConformities nc on nc.ID = cp.NonConformityCode
  left join ProductionOrders po on po.ID = cp.ID
  left join CorrectiveActions ca on ca.ID = po.CorrectiveActionCode
GO
/****** Object:  View [dbo].[VW_QUOPORCostsPrices_select_bakup]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE   VIEW [dbo].[VW_QUOPORCostsPrices_select_bakup]
AS
SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END )
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue,

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  )
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as ProvvTotValue,

(SELECT   max(ts.tm_datdoc)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as DataBolla,

(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where dbo.VW_ProductionOrdersCosts.ID_Company = 1

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode



union all


SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END )
FROM CARTOLABE.dbo.movmag  dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto  = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue,

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  )
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as ProvvTotValue,

(SELECT   max(ts.tm_datdoc)
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as DataBolla,

(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where dbo.VW_ProductionOrdersCosts.ID_Company = 2

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode
GO
/****** Object:  View [dbo].[VW_FreeTypePOD]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VW_FreeTypePOD]
AS
SELECT     PO.ID, PO.Number, PO.Description, POD.FreeTypeCode, TP.Description AS TypeDescription, POD.FreeItemTypeCode, isnull(ITP.Description,'') AS ItemTypeDescription, 
                      Isnull(POD.FreeItemDescription, '') AS FreeItemDescription, POD.ProductionDate, POD.SupplierCode, SU.Name AS SupplierName, 
                      PO.ID_Customer, CL.Name AS CustomerName,
                      POD.UMRawMaterial, UM.Description AS UMDescription, 
                      POD.RawMaterialQuantity, POD.Cost,
 					  POD.Note,
					  POD.ID ID_ProductionOrderDetail
FROM         
                      
                      dbo.ProductionOrderDetails AS POD INNER JOIN 
                      dbo.ProductionOrders AS PO ON PO.ID = POD.ID_ProductionOrder left JOIN
                      dbo.ItemTypes AS ITP ON ITP.Code = POD.FreeItemTypeCode INNER JOIN 
                      dbo.Types AS TP ON POD.FreeTypeCode = TP.Code  INNER JOIN
                      dbo.Units AS UM ON POD.UMRawMaterial = UM.ID LEFT OUTER JOIN
                      dbo.Suppliers AS SU ON POD.SupplierCode = SU.Code LEFT OUTER JOIN
                     dbo.Customers AS CL ON PO.ID_Customer = CL.Code
GO
/****** Object:  View [dbo].[VW_FreeTypeProductionOrderDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_FreeTypeProductionOrderDetails]
AS
SELECT     PO.ID, PO.Number, PO.Description, POD.FreeTypeCode, TP.Description AS TypeDescription, POD.FreeItemTypeCode, ITP.Description AS ItemTypeDescription, 
                      POD.FreeItemDescription, POD.ProductionDate, POD.SupplierCode, SU.Name AS SupplierName, 
                      PO.ID_Customer, CL.Name AS CustomerName,
                      POD.UMRawMaterial, UM.Description AS UMDescription, 
                      POD.RawMaterialQuantity, POD.Cost
                      , TMA.tm_tipork, CAST(TMA.tm_anno AS varchar) + '/' + TMA.tm_serie + '/' + CAST(TMA.tm_numdoc AS varchar) AS NumDDT, 
                      MMA.mm_codart, MMA.mm_descr, MMA.mm_unmis, MMA.mm_quant, MMA.mm_ump, MMA.mm_prezzo, MMA.mm_valore, POD.Note
FROM         
                      
                      dbo.ProductionOrderDetails AS POD INNER JOIN 
                      dbo.ProductionOrders AS PO ON PO.ID = POD.ID_ProductionOrder left JOIN
                      dbo.ItemTypes AS ITP ON ITP.Code = POD.FreeItemTypeCode INNER JOIN 
                      dbo.Types AS TP ON POD.FreeTypeCode = TP.Code  INNER JOIN
                      dbo.Units AS UM ON POD.UMRawMaterial = UM.ID LEFT OUTER JOIN
                      dbo.Suppliers AS SU ON POD.SupplierCode = SU.Code LEFT OUTER JOIN
                     dbo.Customers AS CL ON PO.ID_Customer = CL.Code 
LEFT OUTER JOIN
                      (LABE.dbo.testmag AS TMA INNER JOIN
                      LABE.dbo.movmag AS MMA ON TMA.tm_tipork = MMA.mm_tipork AND TMA.tm_anno = MMA.mm_anno AND TMA.tm_serie = MMA.mm_serie AND 
                      TMA.tm_numdoc = MMA.mm_numdoc) ON PO.ID = MMA.mm_lotto
WHERE 
    ((TMA.tm_tipork IS NULL) OR 
                      (TMA.tm_tipork = 'A') OR
                      (TMA.tm_tipork = 'B'))
                      and MMA.mm_codart = '4'
                      And MMA.mm_valore <> 0

GO
/****** Object:  View [dbo].[VW_FreeTypePrepare]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_FreeTypePrepare]
AS
SELECT                PO.ID, PO.Number, 
                      POD.ProductionDate, POD.SupplierCode,
                      PO.ID_Customer,
                      CAST(TMA.tm_anno AS varchar) + '/' + TMA.tm_serie + '/' + CAST(TMA.tm_numdoc AS varchar) AS NumDDT, 
                      MMA.mm_descr
FROM         
                      
                      dbo.ProductionOrderDetails AS POD INNER JOIN 
                      dbo.ProductionOrders AS PO ON PO.ID = POD.ID_ProductionOrder left JOIN
                      dbo.ItemTypes AS ITP ON ITP.Code = POD.FreeItemTypeCode INNER JOIN 
                      dbo.Types AS TP ON POD.FreeTypeCode = TP.Code  INNER JOIN
                      dbo.Units AS UM ON POD.UMRawMaterial = UM.ID LEFT OUTER JOIN
                      dbo.Suppliers AS SU ON POD.SupplierCode = SU.Code LEFT OUTER JOIN
                     dbo.Customers AS CL ON PO.ID_Customer = CL.Code 
LEFT OUTER JOIN
                      (LABE.dbo.testmag AS TMA INNER JOIN
                      LABE.dbo.movmag AS MMA ON TMA.tm_tipork = MMA.mm_tipork AND TMA.tm_anno = MMA.mm_anno AND TMA.tm_serie = MMA.mm_serie AND 
                      TMA.tm_numdoc = MMA.mm_numdoc) ON PO.ID = MMA.mm_lotto
WHERE 
    ((TMA.tm_tipork IS NULL) OR 
                      (TMA.tm_tipork = 'A') OR
                      (TMA.tm_tipork = 'B'))
                      and MMA.mm_codart = '4'
                      And MMA.mm_valore <> 0

GO
/****** Object:  View [dbo].[VW_HitsOfMacroItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VW_HitsOfMacroItems]
AS

SELECT        p.*, IsNull(s1.Hits,0) as Hits From
[dbo].[MacroItems] p
Left Join
(Select s.[MenuItem],Sum(s.[Hits]) as Hits From
         (
                          SELECT        ID_PickingItem AS MenuItem, MacroRef, COUNT(1) AS Hits
                          FROM            dbo.ProductionOrderDetails AS ProductionOrderDetails_3
                          WHERE        (RFlag = 'M') AND (ID_PickingItem IS NOT NULL)
						  And productionDate >= dateadd(MM,-4,getdate())
                          GROUP BY ID_PickingItem, MacroRef
                          UNION ALL
                          SELECT        ID_PickingItemSup AS MenuItem, MacroRef, COUNT(1) AS Hits
                          FROM            dbo.ProductionOrderDetails AS ProductionOrderDetails_1
                          WHERE        (SFlag = 'M') AND (ID_PickingItemSup IS NOT NULL)
						  And productionDate >= dateadd(MM,-4,getdate())
                          GROUP BY ID_PickingItemSup, MacroRef) AS s
WHERE        (MenuItem IS NOT NULL) AND (MacroRef IS NULL)
Group By MenuItem) s1

  On p.ID = s1.MenuItem
GO
/****** Object:  View [dbo].[VW_ProductionExtMPS_Prod]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE VIEW [dbo].[VW_ProductionExtMPS_Prod]
AS


SELECT mp.[ID]
      ,mp.[IDProductionOrder]
	  ,po.Description poDescription
	  ,po.ID_Customer
	  ,cu.Name cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
	  ,mp.IDMacroItem
      ,mp.[IDMacroItemDetail]
	  ,mi.MacroItemDescription
	  ,qd.ID_Quotation
      ,mp.[IDQuotationDetail]
	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description deDescription
      ,case when mp.[IDQuotationDetail] = -1 then '99' else coalesce(mpe.NewOrder, mp.[Order]) end [Order]
      ,mp.[ProdStart]
      ,mp.[Priority]
      ,mp.[ProdTimeMin]
	  ,dbo.IntToMinutes([ProdTimeMin]) ProdTime
      ,mp.[ProdEnd]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription
	  ,qt.Note qtNote
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11, 15) order by ProdEnd) curMachineId
	  ,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11,15) order by ProdEnd) ) curMachineDescription
	  ,(select top 1 IDQuotationDetail from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseQuotationDetail
	  ,(select top 1 Status from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseStatus
	  ,(select top 1 ID from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseID

	  ,(select isnull(max(OkCopiesCount),0) from ProductionOrderDetails where ID_ProductionOrder = po.ID and ID_Phase = mp.IDPickingItem) OkCopiesCount
	  ,pm.ID_ExternalCompany
	  ,cp.Description ExternalCompanyDescription

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  --inner join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join Quotations qt on po.ID_Quotation = qt.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  --inner join Departments de on pm.IDDepartment = de.ID
  left join Departments de on pm.IDDepartment = de.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3

  left join Companies cp on cp.ID = pm.ID_ExternalCompany
  left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail

  
GO
/****** Object:  View [dbo].[VW_HitsOfPickingItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*Order By s.Hits Desc      */
CREATE VIEW [dbo].[VW_HitsOfPickingItems]
AS


SELECT        p.*, IsNull(s1.Hits,0) as Hits From
[dbo].[PickingItems] p
Left Join
(Select s.[MenuItem],Sum(s.[Hits]) as Hits From
         (SELECT        ID_PickingItem AS MenuItem, MacroRef, COUNT(1) AS Hits
                          FROM            dbo.ProductionOrderDetails
                          WHERE        (RFlag IS NULL) AND (ID_PickingItem IS NOT NULL)
						  And productionDate >= dateadd(MM,-4,getdate())
                          GROUP BY ID_PickingItem, MacroRef
                          UNION 
                          SELECT   ID_PickingItemSup AS MenuItem, MacroRef, COUNT(1) AS Hits
                          FROM            dbo.ProductionOrderDetails AS ProductionOrderDetails_2
                          WHERE        (SFlag IS NULL) AND (ID_PickingItemSup IS NOT NULL)
						  And productionDate >= dateadd(MM,-4,getdate())
                          GROUP BY ID_PickingItemSup, MacroRef
                          ) AS s
WHERE        (MenuItem IS NOT NULL) AND (MacroRef IS NULL) 
Group By MenuItem) s1

  On p.ID = s1.MenuItem
GO
/****** Object:  View [dbo].[VW_EmployeesWorkingDayHours]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











CREATE VIEW [dbo].[VW_EmployeesWorkingDayHours]
AS

--WITH CTE_DatesTable
-- AS
-- (
--   SELECT CAST('20100101' as datetime) AS DateValue
--   UNION ALL
--   SELECT DATEADD(dd, 1, DateValue)
--   FROM CTE_DatesTable
--   WHERE DATEADD(dd, 1, DateValue) < '20300101'
-- )


SELECT 

  b.ID
, b.UniqueName
, a.DateValue As ProductionDate
, IsNull(c.ProductionTime, 0) As ProductionTime
, IsNull(c.ID_Phase, 0) As ID_Phase
, IsNull(c.RawMaterialX, 0) As RawMaterialX
, IsNull(c.RawMaterialY, 0) As RawMaterialY
, IsNull(c.RawMaterialZ, 0) As RawMaterialZ
, IsNull(c.ItemDescription, '') As ItemDescription
, IsNull(c.TypeCode, 0) As TypeCode
, IsNull(c.TypeDescription, '') As TypeDescription
, IsNull(c.ItemTypeCode, 0) As ItemTypeCode
, IsNull(c.ItemTypeDescription, '') As ItemTypeDescription
, IsNull(c.PickingItemOrder, 0) As PickingItemOrder
--, c.YearProductionDate
--, c.MonthProductionDate
, YEAR(DateValue) AS YearProductionDate
, MONTH(DateValue) AS MonthProductionDate 
,c.ID_ProductionOrder
,c.Number
--,c.ID_Company
,c.ID_Customer
,c.CustomerName

FROM 
(
 --SELECT DateValue FROM CTE_DatesTable 

-- SELECT DATEADD(d,number,(Select MIN(ProductionDate) From dbo.ProductionOrderDetails))AS DateValue
--FROM master..spt_values
--WHERE number BETWEEN 1 AND
--DATEDIFF(d,(Select MIN(ProductionDate) From dbo.ProductionOrderDetails),(Select MAX(ProductionDate) From dbo.ProductionOrderDetails))
--AND type = 'P'

select
    dateadd(d, v1.number+v2.number*2048, '20100101') DateValue
from master..spt_values v1
    cross join (select number from master..spt_values where number<5 and type='p') v2       
where type='p'
    and (v1.number+v2.number*2048)<= datediff(d,'20100101','20261231')
	
) a
Cross Join dbo.Employees b -- (select * from dbo.Employees where ID_Company=1) b
Left Join
(
Select
  dbo.ProductionOrderDetails.ID_ProductionOrder
, dbo.ProductionOrders.Number
, dbo.ProductionOrders.ID_Customer
, dbo.Customers.Name CustomerName
--, dbo.ProductionOrderDetails.ID_Owner
, etol.LabeID ID_Owner
, dbo.ProductionOrderDetails.ID_Company
, dbo.ProductionOrderDetails.ProductionDate
, dbo.ProductionOrderDetails.ProductionTime
, dbo.ProductionOrderDetails.ID_Phase
, dbo.ProductionOrderDetails.RawMaterialX
, dbo.ProductionOrderDetails.RawMaterialY
, dbo.ProductionOrderDetails.RawMaterialZ
, dbo.PickingItems.ItemDescription
, dbo.PickingItems.TypeCode
, dbo.Types.Description AS TypeDescription
, dbo.PickingItems.ItemTypeCode
, dbo.ItemTypes.Description AS ItemTypeDescription
, dbo.PickingItems.[Order] AS PickingItemOrder
, YEAR(dbo.ProductionOrderDetails.ProductionDate) AS YearProductionDate
, MONTH(dbo.ProductionOrderDetails.ProductionDate) AS MonthProductionDate 
From
     dbo.ProductionOrderDetails INNER JOIN
     dbo.PickingItems ON dbo.ProductionOrderDetails.ID_Phase = dbo.PickingItems.ID INNER JOIN
     dbo.Types ON dbo.PickingItems.TypeCode = dbo.Types.Code INNER JOIN
     dbo.ItemTypes ON dbo.PickingItems.ItemTypeCode = dbo.ItemTypes.Code INNER JOIN
	 dbo.ProductionOrders On dbo.ProductionOrders.ID = dbo.ProductionOrderDetails.ID_ProductionOrder LEFT JOIN
	 dbo.Customers on dbo.Customers.Code = dbo.ProductionOrders.ID_Customer left join
	 VW_EmployeesToLabe etol on ProductionOrderDetails.ID_Owner = etol.LabeID or ProductionOrderDetails.ID_Owner = etol.CartoLabeID

) c 
On
  --(a.DateValue = c.ProductionDate And b.ID = c.ID_Owner)
  (a.DateValue = c.ProductionDate And b.ID = c.ID_Owner)







GO
/****** Object:  View [dbo].[VW_PickingItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VW_PickingItems]
AS
SELECT     TOP (100) PERCENT dbo.PickingItems.ID,
					  dbo.Companies.Description AS CompanyDescription,
					  dbo.Types.Description AS TypeDescription, dbo.ItemTypes.Description AS ItemTypeDescription, 
                      dbo.PickingItems.ItemDescription, dbo.Units.Description AS UnitDescription, dbo.PickingItems.Inserted, dbo.PickingItems.Multiply, dbo.PickingItems.Cost, 
                      dbo.PickingItems.Percentage, dbo.Suppliers.Name, dbo.PickingItems.Link, dbo.PickingItems.[Order], dbo.PickingItems.Template, dbo.PickingItems.ItemManufacturing, 
                      dbo.PickingItems.Date, dbo.PickingItems.TypeCode, dbo.PickingItems.ItemTypeCode, dbo.PickingItems.SupplierCode, dbo.PickingItems.ID_Company
FROM         dbo.PickingItems INNER JOIN
                      dbo.ItemTypes ON dbo.PickingItems.ItemTypeCode = dbo.ItemTypes.Code INNER JOIN
                      dbo.Types ON dbo.PickingItems.TypeCode = dbo.Types.Code INNER JOIN
                      dbo.Suppliers ON dbo.PickingItems.SupplierCode = dbo.Suppliers.Code INNER JOIN
                      dbo.Units ON dbo.PickingItems.UM = dbo.Units.ID INNER JOIN
					  dbo.Companies ON dbo.PickingItems.ID_Company = dbo.Companies.ID
ORDER BY TypeDescription, ItemTypeDescription, dbo.PickingItems.ItemDescription
GO
/****** Object:  View [dbo].[VW_MenuItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VW_MenuItems]
AS
SELECT     'P' + replace(str(ID),' ','') AS ID, ID_Company, TypeCode, ItemTypeCode, ItemDescription, Inserted, Link, [Order], Cost
FROM         PickingItems
UNION
SELECT     'M' +replace(str(ID),' ','') AS ID , ID_Company, TypeCode, ItemTypeCode, MacroItemDescription, Inserted, Link, [Order], Cost
FROM         MacroItems
GO
/****** Object:  View [dbo].[VW_UngroupablePickingItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VW_UngroupablePickingItems]
AS
SELECT     
 a.ID as IDChecked,
 f.Code as TypeCodeChecked,
 f.Description as TypeDescChecked,
 e.Code as ItemTypeCodeChecked,
 e.Description AS ItemTypeDescChecked,
 a.ItemDescription as ItemDescriptionChecked,
 a.UM as UMChecked,
 c.Description AS UMDescChecked,
 b.ID AS IDDep,
 h.Code as TypeCodeDep,
 h.Description AS TypeDescDep,
 g.Code as ItemTypeCodeDep,
 g.Description AS ItemTypeDescDep,
 b.ItemDescription AS ItemDescriptionDep,
 b.UM AS UMDep,
 d.Description AS UMDescDep,
 a.[order] as [Order],
 a.Inserted as Inserted

FROM         dbo.PickingItems AS a INNER JOIN
                      dbo.PickingItems AS b ON a.Link = b.ID AND a.UM <> b.UM INNER JOIN
                      dbo.Units AS c ON a.UM = c.ID INNER JOIN
                      dbo.Units AS d ON b.UM = d.ID INNER JOIN
                      dbo.ItemTypes AS e ON a.ItemTypeCode = e.Code INNER JOIN
                      dbo.Types AS f ON a.TypeCode = f.Code INNER JOIN
                      dbo.ItemTypes AS g ON b.ItemTypeCode = g.Code INNER JOIN
                      dbo.Types AS h ON b.TypeCode = h.Code
WHERE     (a.Link IS NOT NULL)
GO
/****** Object:  View [dbo].[VW_CalculatedDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VW_CalculatedDetails]
AS
SELECT TOP (100) PERCENT dbo.Customers.Name
	,dbo.Customers.Contact
	,dbo.Customers.Street
	,dbo.Customers.CAP
	,dbo.Customers.City
	,dbo.Customers.Province
	,dbo.Customers.Fax
	,dbo.Quotations.DATE
	,dbo.Quotations.Subject
	,dbo.QuotationDetails.ID_Quotation
	,dbo.QuotationDetails.Position
	,dbo.Types.Description AS TypeDescription
	,dbo.ItemTypes.Description AS ItemTypeDescription
	,dbo.Units.Description AS UnitDescription
	,dbo.QuotationDetails.Quantity
	,dbo.QuotationDetails.Cost
	,dbo.QuotationDetails.Price
	,dbo.QuotationDetails.Cost * dbo.QuotationDetails.Quantity AS TotalC
	,dbo.QuotationDetails.Price * dbo.QuotationDetails.Quantity AS TotalP
	,dbo.QuotationDetails.Multiply
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE QuotationDetails.Cost * QuotationDetails.Quantity
		END AS Cost1
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0)
		ELSE QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * QuotationDetails.Quantity
		END AS CostP1
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Price
		ELSE QuotationDetails.Price * QuotationDetails.Quantity
		END AS Price1
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN Quotations.Q1 = 0
							THEN 0
						ELSE QuotationDetails.Quantity / Quotations.Q1
						END
					) * Quotations.Q2
		ELSE QuotationDetails.Quantity
		END AS Batt2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END AS Cost2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * QuotationDetails.Quantity
				END
		END AS CostP2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END AS Price2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN Quotations.Q1 = 0
							THEN 0
						ELSE QuotationDetails.Quantity / Quotations.Q1
						END
					) * Quotations.Q3
		ELSE QuotationDetails.Quantity
		END AS Batt3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END AS Cost3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * QuotationDetails.Quantity
				END
		END AS CostP3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END AS Price3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN Quotations.Q1 = 0
							THEN 0
						ELSE QuotationDetails.Quantity / Quotations.Q1
						END
					) * Quotations.Q4
		ELSE QuotationDetails.Quantity
		END AS Batt4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END AS Cost4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * QuotationDetails.Quantity
				END
		END AS CostP4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END AS Price4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN Quotations.Q1 = 0
							THEN 0
						ELSE QuotationDetails.Quantity / Quotations.Q1
						END
					) * Quotations.Q5
		ELSE QuotationDetails.Quantity
		END AS Batt5
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END AS Cost5
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE QuotationDetails.Cost * (cast(QuotationDetails.Percentage AS DECIMAL) / 100.0) * QuotationDetails.Quantity
				END
		END AS CostP5
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END AS Price5
	,dbo.Quotations.Q1
	,dbo.Quotations.Q2
	,dbo.Quotations.Q3
	,dbo.Quotations.Q4
	,dbo.Quotations.Q5
	,dbo.QuotationDetails.SelectPhase
	,CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS PhaseCost
	,CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS PhasePrice
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE QuotationDetails.Cost * QuotationDetails.Quantity
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS ProdC1
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Price
		ELSE QuotationDetails.Price * QuotationDetails.Quantity
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV1
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS ProdC2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV2
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS ProdC3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV3
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS ProdC4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV4
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Cost * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Cost
				ELSE QuotationDetails.Cost * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Cost
		ELSE 0
		END AS ProdC5
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END - CASE 
		WHEN (QuotationDetails.SelectPhase = 1)
			THEN QuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV5
	,dbo.Quotations.MarkUp
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Quantity * QuotationDetails.Price
		ELSE QuotationDetails.Price * QuotationDetails.Quantity
		END * dbo.Quotations.MarkUp / 100 AS [Price%1]
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q2
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END * dbo.Quotations.MarkUp / 100 AS [Price%2]
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q3
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END * dbo.Quotations.MarkUp / 100 AS [Price%3]
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q4
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END * dbo.Quotations.MarkUp / 100 AS [Price%4]
	,CASE 
		WHEN (QuotationDetails.Multiply = 1)
			THEN QuotationDetails.Price * CASE 
					WHEN (QuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN Quotations.Q1 = 0
										THEN 0
									ELSE QuotationDetails.Quantity / Quotations.Q1
									END
								) * Quotations.Q5
					ELSE QuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (QuotationDetails.Multiply = 1)
					THEN QuotationDetails.Quantity * QuotationDetails.Price
				ELSE QuotationDetails.Price * QuotationDetails.Quantity
				END
		END * dbo.Quotations.MarkUp / 100 AS [Price%5]
	,dbo.QuotationDetails.SupplierCode
	,dbo.QuotationDetails.Percentage
	,dbo.QuotationDetails.TypeCode

FROM dbo.Customers
INNER JOIN dbo.Quotations ON dbo.Customers.Code = dbo.Quotations.CustomerCode
INNER JOIN dbo.QuotationDetails ON dbo.Quotations.ID = dbo.QuotationDetails.ID_Quotation

INNER JOIN dbo.Types ON dbo.Types.Code = dbo.QuotationDetails.TypeCode
INNER JOIN dbo.ItemTypes ON dbo.ItemTypes.Code = dbo.QuotationDetails.ItemTypeCode
INNER JOIN dbo.Units ON dbo.Units.ID = dbo.QuotationDetails.UM
WHERE (dbo.QuotationDetails.Inserted <> 0)
ORDER BY dbo.QuotationDetails.Position
GO
/****** Object:  Table [dbo].[TempQuotationDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempQuotationDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SessionUser] [int] NOT NULL,
	[ID_Quotation] [int] NOT NULL,
	[ID_QuotationDetail] [int] NOT NULL,
	[Position] [nvarchar](50) NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[CommonKey] [int] NULL,
	[MacroItemKey] [int] NULL,
	[ItemTypeCode] [int] NOT NULL,
	[ItemTypeDescription] [nvarchar](100) NULL,
	[UM] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [real] NOT NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[SelectPhase] [bit] NOT NULL,
	[MarkUp] [int] NOT NULL,
	[SupplierCode] [int] NULL,
	[Percentage] [int] NOT NULL,
	[Save] [bit] NOT NULL,
	[TotalCost]  AS ([Cost]*[Quantity]),
 CONSTRAINT [PK_TempQuotationDetails_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[SessionUser] ASC,
	[ID_Quotation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempQuotations]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempQuotations](
	[SessionUser] [int] NOT NULL,
	[ID_Quotation] [int] NOT NULL,
	[Number] [varchar](10) NULL,
	[ID_Company] [int] NULL,
	[CustomerCode] [int] NULL,
	[Date] [datetime] NULL,
	[Subject] [nvarchar](200) NULL,
	[Q1] [int] NULL,
	[Q2] [int] NULL,
	[Q3] [int] NULL,
	[Q4] [int] NULL,
	[Q5] [int] NULL,
	[MarkUp] [int] NULL,
	[ID_Owner] [int] NULL,
	[ID_Approver] [int] NULL,
	[Draft] [bit] NULL,
	[Status] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[P1] [bit] NULL,
	[P2] [bit] NULL,
	[P3] [bit] NULL,
	[P4] [bit] NULL,
	[P5] [bit] NULL,
	[PriceCom] [nvarchar](max) NULL,
	[PrintingMainText] [nvarchar](max) NULL,
	[UpdateDate] [datetime] NULL,
	[ID_Manager] [int] NULL,
	[Note1] [nvarchar](max) NULL,
	[Note2] [nvarchar](max) NULL,
 CONSTRAINT [PK_TempQuotations] PRIMARY KEY CLUSTERED 
(
	[SessionUser] ASC,
	[ID_Quotation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_TempCalculatedDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_TempCalculatedDetails]
AS
SELECT TOP (100) PERCENT dbo.Customers.Name
	,dbo.Customers.Contact
	,dbo.Customers.Street
	,dbo.Customers.CAP
	,dbo.Customers.City
	,dbo.Customers.Province
	,dbo.Customers.Fax
	,dbo.TempQuotations.DATE
	,dbo.TempQuotations.Subject
	,dbo.TempQuotationDetails.ID_Quotation
	,dbo.TempQuotationDetails.Position
	,dbo.Types.Description AS TypeDescription
	,dbo.ItemTypes.Description AS ItemTypeDescription
	,dbo.Units.Description AS UnitDescription
	,dbo.TempQuotationDetails.Quantity
	,dbo.TempQuotationDetails.Cost
	,dbo.TempQuotationDetails.Price
	,dbo.TempQuotationDetails.Cost * dbo.TempQuotationDetails.Quantity AS TotalC
	,dbo.TempQuotationDetails.Price * dbo.TempQuotationDetails.Quantity AS TotalP
	,dbo.TempQuotationDetails.Multiply
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
		END AS Cost1
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0)
		ELSE TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * TempQuotationDetails.Quantity
		END AS CostP1
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
		ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
		END AS Price1
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN TempQuotations.Q1 = 0
							THEN 0
						ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
						END
					) * TempQuotations.Q2
		ELSE TempQuotationDetails.Quantity
		END AS Batt2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END AS Cost2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * TempQuotationDetails.Quantity
				END
		END AS CostP2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END AS Price2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN TempQuotations.Q1 = 0
							THEN 0
						ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
						END
					) * TempQuotations.Q3
		ELSE TempQuotationDetails.Quantity
		END AS Batt3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END AS Cost3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * TempQuotationDetails.Quantity
				END
		END AS CostP3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END AS Price3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN TempQuotations.Q1 = 0
							THEN 0
						ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
						END
					) * TempQuotations.Q4
		ELSE TempQuotationDetails.Quantity
		END AS Batt4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END AS Cost4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * TempQuotationDetails.Quantity
				END
		END AS CostP4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END AS Price4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN (
					CASE 
						WHEN TempQuotations.Q1 = 0
							THEN 0
						ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
						END
					) * TempQuotations.Q5
		ELSE TempQuotationDetails.Quantity
		END AS Batt5
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END AS Cost5
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0)
				ELSE TempQuotationDetails.Cost * (cast(TempQuotationDetails.Percentage AS DECIMAL) / 100.0) * TempQuotationDetails.Quantity
				END
		END AS CostP5
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END AS Price5
	,dbo.TempQuotations.Q1
	,dbo.TempQuotations.Q2
	,dbo.TempQuotations.Q3
	,dbo.TempQuotations.Q4
	,dbo.TempQuotations.Q5
	,dbo.TempQuotationDetails.SelectPhase
	,CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS PhaseCost
	,CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS PhasePrice
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS ProdC1
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
		ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV1
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS ProdC2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV2
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS ProdC3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV3
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS ProdC4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV4
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Cost * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
				ELSE TempQuotationDetails.Cost * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Cost
		ELSE 0
		END AS ProdC5
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END - CASE 
		WHEN (TempQuotationDetails.SelectPhase = 1)
			THEN TempQuotationDetails.Quantity * Price
		ELSE 0
		END AS ProdV5
	,dbo.TempQuotations.MarkUp
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
		ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
		END * dbo.TempQuotations.MarkUp / 100 AS [Price%1]
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q2
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END * dbo.TempQuotations.MarkUp / 100 AS [Price%2]
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q3
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END * dbo.TempQuotations.MarkUp / 100 AS [Price%3]
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q4
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END * dbo.TempQuotations.MarkUp / 100 AS [Price%4]
	,CASE 
		WHEN (TempQuotationDetails.Multiply = 1)
			THEN TempQuotationDetails.Price * CASE 
					WHEN (TempQuotationDetails.Multiply = 1)
						THEN (
								CASE 
									WHEN TempQuotations.Q1 = 0
										THEN 0
									ELSE TempQuotationDetails.Quantity / TempQuotations.Q1
									END
								) * TempQuotations.Q5
					ELSE TempQuotationDetails.Quantity
					END
		ELSE CASE 
				WHEN (TempQuotationDetails.Multiply = 1)
					THEN TempQuotationDetails.Quantity * TempQuotationDetails.Price
				ELSE TempQuotationDetails.Price * TempQuotationDetails.Quantity
				END
		END * dbo.TempQuotations.MarkUp / 100 AS [Price%5]
	,dbo.TempQuotationDetails.SupplierCode
	,dbo.TempQuotationDetails.Percentage
	,dbo.TempQuotationDetails.TypeCode
	,dbo.TempQuotations.SessionUser
FROM dbo.Customers
INNER JOIN dbo.TempQuotations ON dbo.Customers.Code = dbo.TempQuotations.CustomerCode
INNER JOIN dbo.TempQuotationDetails ON dbo.TempQuotations.ID_Quotation = dbo.TempQuotationDetails.ID_Quotation
	AND dbo.TempQuotations.SessionUser = dbo.TempQuotationDetails.SessionUser
INNER JOIN dbo.Types ON dbo.Types.Code = dbo.TempQuotationDetails.TypeCode
INNER JOIN dbo.ItemTypes ON dbo.ItemTypes.Code = dbo.TempQuotationDetails.ItemTypeCode
INNER JOIN dbo.Units ON dbo.Units.ID = dbo.TempQuotationDetails.UM
WHERE (dbo.TempQuotationDetails.Inserted <> 0)
ORDER BY dbo.TempQuotationDetails.Position
GO
/****** Object:  View [dbo].[VW_AllQuotations]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[VW_AllQuotations]
AS
SELECT     ID, Number, ID_Company, CustomerCode, Date, Subject, Q1, Q2, Q3, Q4, Q5, MarkUp, ID_Owner, ID_Approver, Draft, Status, Note, ID_Manager
FROM         dbo.Quotations
UNION
SELECT     ID_Quotation, '', ID_Company, CustomerCode, Date, Subject, Q1, Q2, Q3, Q4, Q5, MarkUp, ID_Owner, ID_Approver, CAST(1 AS bit) AS Draft , Status, Note, ID_Manager
FROM         dbo.TempQuotations
WHERE ID_Quotation < 0
GO
/****** Object:  View [dbo].[VW_QUOPORCostsPrices_select]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE   VIEW [dbo].[VW_QUOPORCostsPrices_select]
AS
SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END ),0)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue,

(SELECT   sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  )
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as ProvvTotValue,

(SELECT   max(ts.tm_datdoc)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as DataBolla,

(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where dbo.VW_ProductionOrdersCosts.ID_Company = 1

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode

union all


SELECT     dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
  Employees.UniqueName AS OwnerName,
 dbo.VW_QuotationsCostsPrices.QUOSubject, 
 
 SUM(dbo.VW_ProductionOrdersCosts.StartQuantity) AS PORSQuantity,
 dbo.VW_ProductionOrdersCosts.ID_Customer AS PORID_Customer, 

 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsCost,0))
                      AS PORTotCost, 
 SUM(IsNull(dbo.VW_ProductionOrdersCosts.PORDirectCost,0) + IsNull(dbo.VW_ProductionOrdersCosts.PORDetailsHistoricalCost,0))
                      AS PORTotHistoricalCost, 

SUM(IsNull(dbo.VW_QuotationsCostsPrices.QUORefFixCost,0) + 
IsNull((
Case When dbo.VW_QuotationsCostsPrices.Q1 <> 0 Then
dbo.VW_QuotationsCostsPrices.QUORefVarCost / dbo.VW_QuotationsCostsPrices.Q1
Else 0 End

) * dbo.VW_ProductionOrdersCosts.StartQuantity ,0))
                      AS QUOTotCost, 


  SUM(dbo.VW_ProductionOrdersCosts.ProducedQuantity) AS PORSProducedQuantity,
dbo.VW_ProductionOrdersCosts.ID,
dbo.VW_ProductionOrdersCosts.ID_Company,
dbo.VW_ProductionOrdersCosts.Number,
dbo.VW_ProductionOrdersCosts.[Status],
dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,

--(SELECT   sum( mm_valore)
--FROM         Labe.dbo.movmag
--WHERE     (mm_tipork = 'B') AND mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) as FATTotValue

(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END ),0)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
+
(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_valore) ELSE mm_valore END ),0)
FROM CartoLabe.dbo.movmag dt
	 inner join CartoLabe.dbo.testmag ts on
			dt.mm_tipork =  ts.tm_tipork 
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie	 =  ts.tm_serie
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt   = ts.codditt
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
as FATTotValue,

(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  ),0)
FROM LABE.dbo.movmag dt
	 inner join LABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
+
(SELECT   isnull(sum( CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - mm_vprovv - mm_vprovv2) ELSE mm_vprovv + mm_vprovv2 END  ),0)
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) 
as ProvvTotValue,

Coalesce(
(SELECT   max(ts.tm_datdoc)
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
,
(SELECT   max(ts.tm_datdoc)
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie  COLLATE DATABASE_DEFAULT =  ts.tm_serie  COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID ) 
)
as DataBolla,

Coalesce(
(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM Labe.dbo.movmag dt
	 inner join Labe.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
,
(SELECT   max(ts.tm_tipork)  COLLATE DATABASE_DEFAULT
FROM CARTOLABE.dbo.movmag dt
	 inner join CARTOLABE.dbo.testmag ts on
			dt.mm_tipork COLLATE DATABASE_DEFAULT =  ts.tm_tipork COLLATE DATABASE_DEFAULT
			and dt.mm_anno	 =  ts.tm_anno
			and dt.mm_serie COLLATE DATABASE_DEFAULT =  ts.tm_serie COLLATE DATABASE_DEFAULT 
			and dt.mm_numdoc =  ts.tm_numdoc
			and dt.codditt COLLATE DATABASE_DEFAULT   = ts.codditt COLLATE DATABASE_DEFAULT
WHERE     (ts.tm_tipork IN ('A','B','C', 'E','N')) AND dt.mm_lotto = dbo.VW_ProductionOrdersCosts.ID )
)
as TipoRec,

dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode

FROM         dbo.VW_QuotationsCostsPrices RIGHT JOIN
                      dbo.VW_ProductionOrdersCosts ON dbo.VW_QuotationsCostsPrices.ID_Quotation = dbo.VW_ProductionOrdersCosts.ID_Quotation
                      LEFT JOIN
                      dbo.Employees ON VW_QuotationsCostsPrices.QUOOwner = Employees.ID
					  Left join
					  dbo.Customers ON dbo.VW_ProductionOrdersCosts.ID_Customer = Customers.Code

where dbo.VW_ProductionOrdersCosts.ID_Company = 2

GROUP BY 
dbo.VW_QuotationsCostsPrices.ID_Quotation,
 dbo.VW_QuotationsCostsPrices.QUOOwner,
 dbo.VW_QuotationsCostsPrices.QUOSubject,
 
 dbo.VW_ProductionOrdersCosts.ID_Customer,
 dbo.VW_ProductionOrdersCosts.ID,
 dbo.VW_ProductionOrdersCosts.ID_Company,
 dbo.VW_ProductionOrdersCosts.Number,
 dbo.VW_ProductionOrdersCosts.[Status],
 dbo.VW_ProductionOrdersCosts.StartDate,
dbo.VW_ProductionOrdersCosts.EndDate,
dbo.VW_ProductionOrdersCosts.AccountNote,
dbo.VW_ProductionOrdersCosts.Note,
Employees.UniqueName,
dbo.Customers.IDAgente1,
dbo.Customers.DescrizioneAgente1,
dbo.VW_QuotationsCostsPrices.PriceCom,
dbo.VW_ProductionOrdersCosts.NonConformityCode,
dbo.VW_ProductionOrdersCosts.ComplaintReceived,
dbo.VW_ProductionOrdersCosts.CorrectiveActionCode
GO
/****** Object:  View [dbo].[VW_DDTQUOPORCostsPrices_select_Company01]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE   VIEW [dbo].[VW_DDTQUOPORCostsPrices_select_Company01]
AS

SELECT 
ID_Customer,
mm_tipork collate Latin1_General_CI_AS TipoRec,
tm_serie collate Latin1_General_CI_AS SerieBolla,
tm_numdoc NumBolla,
tm_datdoc DataBolla,
--mm_descr,
sum(mm_quant) QtaBolla,
avg(mm_prezzo) PrezzoBolla,
sum(FATTotValue) FATTotValue,
sum(ProvvTotValue) ProvvTotValue,
mm_lotto ID,
ID_Company,
Number,
Status,
StartQuantity,
StartDate,
EndDate,
ID_Quotation,
QUOSubject,
QUOOwner,
OwnerName,
sum(PORPropCost) PORPropCost,
sum(PORPropDirectCost) PORPropDirectCost,
sum(QUOPropCost) QUOPropCost,
sum(PropProducedQuantity) PropProducedQuantity,
PORTotCost,
PORTotDirectCost,
QUOTotCost,
AccountNote,
Note,
IDAgente1,
DescrizioneAgente1,
PriceCom,
NonConformityCode,
ComplaintReceived,
CorrectiveActionCode

FROM
(
SELECT 
poc.ID_Customer,
mm_tipork,
ts.tm_serie,
ts.tm_numdoc,
ts.tm_datdoc,
mm_descr,
mm_quant,
mm_prezzo,
CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - dt.mm_valore) ELSE dt.mm_valore END FATTotValue,
CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - dt.mm_vprovv - dt.mm_vprovv2) ELSE dt.mm_vprovv + dt.mm_vprovv2 END ProvvTotValue,
dt.mm_lotto,
poc.ID_Company,
poc.Number,
poc.Status,
poc.StartQuantity,
poc.StartDate,
poc.EndDate,
qcp.ID_Quotation,
qcp.QUOSubject,
qcp.QUOOwner,
e.UniqueName AS OwnerName,

(select SUM(IsNull(poc1.PORDirectCost,0) + IsNull(poc1.PORDetailsCost,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto)  PORTotCost,

(select SUM(IsNull(poc1.PORDirectCost,0) + IsNull(poc1.PORDetailsCost,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 1 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PORPropCost, 

(select SUM(IsNull(poc2.PORDirectCost,0) + IsNull(poc2.PORDetailsHistoricalCost,0)) from VW_ProductionOrdersCosts poc2 where poc2.ID=dt.mm_lotto) PORTotDirectCost,

(select SUM(IsNull(poc2.PORDirectCost,0) + IsNull(poc2.PORDetailsHistoricalCost,0)) from VW_ProductionOrdersCosts poc2 where poc2.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 1 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PORPropDirectCost, 

(select SUM(IsNull(qcp3.QUORefFixCost,0) + 
IsNull((
Case When qcp3.Q1 <> 0 Then
qcp3.QUORefVarCost / qcp3.Q1
Else 0 End
) * poc3.StartQuantity ,0)) from VW_ProductionOrdersCosts poc3 left join dbo.VW_QuotationsCostsPrices qcp3 on qcp3.ID_Quotation = poc3.ID_Quotation where poc3.ID=dt.mm_lotto) AS QUOTotCost,

(select SUM(IsNull(qcp3.QUORefFixCost,0) + 
IsNull((
Case When qcp3.Q1 <> 0 Then
qcp3.QUORefVarCost / qcp3.Q1
Else 0 End
) * poc3.StartQuantity ,0)) from VW_ProductionOrdersCosts poc3 left join dbo.VW_QuotationsCostsPrices qcp3 on qcp3.ID_Quotation = poc3.ID_Quotation where poc3.ID=dt.mm_lotto)
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 1 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS QUOPropCost, 


(select SUM(IsNull(poc1.ProducedQuantity,0) + IsNull(poc1.ProducedQuantity,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto)  TotProducedQuantity,

(select SUM(IsNull(poc1.ProducedQuantity,0) + IsNull(poc1.ProducedQuantity,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 1 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PropProducedQuantity, 


poc.AccountNote,
poc.Note,
c.IDAgente1,
c.DescrizioneAgente1,
qcp.PriceCom,
poc.NonConformityCode,
poc.ComplaintReceived,
poc.CorrectiveActionCode

FROM Labe.dbo.movmag dt 

inner join Labe.dbo.testmag ts on dt.mm_tipork =  ts.tm_tipork and dt.mm_anno = ts.tm_anno and dt.mm_serie = ts.tm_serie and dt.mm_numdoc =  ts.tm_numdoc and dt.codditt   = ts.codditt
left join dbo.VW_ProductionOrdersCosts poc ON poc.ID = dt.mm_lotto
left join dbo.VW_QuotationsCostsPrices qcp on qcp.ID_Quotation = poc.ID_Quotation
left join dbo.Employees e ON qcp.QUOOwner = e.ID
left join dbo.Customers c ON poc.ID_Customer = c.Code


WHERE     
(ts.tm_tipork IN ('A','B','C','E','N')) and mm_valore <> 0

) d

Group By

ID_Customer,
mm_tipork,
tm_serie,
tm_numdoc,
tm_datdoc,
--mm_descr,
mm_lotto,
ID_Company,
Number,
Status,
StartQuantity,
StartDate,
EndDate,
ID_Quotation,
QUOSubject,
QUOOwner,
OwnerName,
PORTotCost,
PORTotDirectCost,
QUOTotCost,
AccountNote,
Note,
IDAgente1,
DescrizioneAgente1,
PriceCom,
NonConformityCode,
ComplaintReceived,
CorrectiveActionCode


GO
/****** Object:  View [dbo].[VW_ProductionExtMPS]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE VIEW [dbo].[VW_ProductionExtMPS]
AS


SELECT min(mp.[ID]) [ID]
      ,mp.[IDProductionOrder]
	  ,po.Description poDescription
	  ,po.ID_Customer
	  ,cu.Name cuName
	  ,po.Status poStatus
	  ,st.Description stDescription
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
	  ,mp.IDMacroItem
      ,mp.[IDMacroItemDetail]
	  ,mi.MacroItemDescription
	  ,qd.ID_Quotation
      ,mp.[IDQuotationDetail]
	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description pmDescription
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description deDescription
      ,case when mp.[IDQuotationDetail] = -1 then '99' else min(coalesce(mpe.NewOrder, mp.[Order])) end [Order]
      ,min(mp.[ProdStart])[ProdStart]
      ,mp.[Priority]
      ,sum(mp.[ProdTimeMin]) [ProdTimeMin]
	  ,dbo.IntToMinutes(sum([ProdTimeMin])) ProdTime
      ,max(mp.[ProdEnd]) [ProdEnd]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description mpstDescription
	  ,qt.Note qtNote
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11, 15) order by ProdEnd) curMachineId
	  ,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status in (11,15) order by ProdEnd) ) curMachineDescription
	  ,(select top 1 IDQuotationDetail from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseQuotationDetail
	  ,(select top 1 Status from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseStatus
	  ,(select top 1 ID from ProductionMPS where Status in (11, 15) and IDProductionOrder = po.ID order by ProdEnd) curPhaseID

	  ,(select isnull(max(OkCopiesCount),0) from ProductionOrderDetails where ID_ProductionOrder = po.ID and ID_Phase = mp.IDPickingItem) OkCopiesCount
	  ,pm.ID_ExternalCompany
	  ,cp.Description ExternalCompanyDescription

  FROM [dbo].[ProductionMPS] mp


  inner join ProductionOrders po on mp.IDProductionOrder = po.id
  inner join PickingItems pi on mp.IDPickingItem = pi.ID
  left join MacroItemDetails md on mp.IDMacroItemDetail = md.id
  left join MacroItems mi on mi.ID = md.ID_MacroItem
  --inner join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  left join QuotationDetails qd on mp.IDQuotationDetail = qd.ID
  inner join Quotations qt on po.ID_Quotation = qt.ID
  inner join ProductionMachines pm on mp.IDProductionMachine = pm.ID
  --inner join Departments de on pm.IDDepartment = de.ID
  left join Departments de on pm.IDDepartment = de.ID
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code
  inner join Statuses st1 on mp.Status =st1.ID and st1.StatusType = 3

  left join Companies cp on cp.ID = pm.ID_ExternalCompany
  --sostituito il 05/06/2023
  --left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail
  left join ProductionMPSExceptions mpe on mpe.IDQuotationDetail = mp.IDQuotationDetail and coalesce(mpe.IDMacroItemDetail, mpe.IdPickingitem) = coalesce(mp.IDMacroItemDetail, mp.IdPickingitem)

  group by 

  --mp.[ID]
       mp.[IDProductionOrder]
	  ,po.Description
	  ,po.ID_Customer
	  ,cu.Name
	  ,po.Status
	  ,st.Description
      ,mp.[IDPickingItem]
	  ,pi.ItemDescription
	  ,mp.IDMacroItem
      ,mp.[IDMacroItemDetail]
	  ,mi.MacroItemDescription
	  ,qd.ID_Quotation
      ,mp.[IDQuotationDetail]
	  ,qd.ItemTypeDescription
      ,mp.[IDProductionMachine]
	  ,pm.Description
      ,mp.[NumProductionMachine]
	  ,pm.IDDepartment
	  ,de.Description
      --,case when mp.[IDQuotationDetail] = -1 then '99' else coalesce(mpe.NewOrder, mp.[Order]) end
      ,mp.[Priority]
	  ,po.Quantity
	  ,po.DeliveryDate
	  ,mp.Status
	  ,st1.Description
	  ,qt.Note
	  ,pm.ID_ExternalCompany
	  ,cp.Description
	  ,po.ID
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [smallint] NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_Employees]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE VIEW [dbo].[VW_Employees]
AS
SELECT e.[ID]
	  ,u.UserName
      ,e.[CompanyCode]
      ,e.[Name]
      ,e.[Surname]
      --,e.[HireDate]
      ,e.[LeavingDate]
      --,e.[ID_Manager]
      --,e.[ID_Dept]
      --,e.[ID_Machine]
      ,e.[UniqueName]
      ,u.UserId [UserGUID]
      --,e.[Role]
	  ,ur.Description roleDesc
      ,e.[ID_Company]
	  ,c.Description companyDesc
  FROM [dbo].[Employees] e 
  left join aspnetdb..aspnet_Users u on e.UserGUID = u.UserId
  left join Companies c on c.ID = e.ID_Company
  left join UserRoles ur on ur.ID = e.Role

  --where u.UserId is not null
GO
/****** Object:  View [dbo].[VW_ProductionMPSSnapshots]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE VIEW [dbo].[VW_ProductionMPSSnapshots]
AS


SELECT 
       po.id
	  ,po.Description poDescription
	  ,po.ID_Customer
	  ,cu.Name cuName
	  ,po.ID_Quotation
	  ,po.Quantity
	  ,po.StartDate
	  ,po.Status poStatus
	  ,st.Description poStatudDesc
	  ,(select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status = 11 order by ProdEnd) curMachineId
	  ,(select top 1 Description from ProductionMachines where id = (select top 1 IDProductionMachine from ProductionMPS where IDProductionOrder = po.ID and Status = 11 order by ProdEnd) ) curMachineDescription
	  ,(select COUNT(1) from ProductionMPS where IDProductionOrder = po.ID and Status = 11) OpenPhases
	  ,po.DeliveryDate

  FROM 
  ProductionOrders po 
  inner join Statuses st on po.Status =st.ID and st.StatusType = 1
  inner join Customers cu on po.ID_Customer = cu.Code






GO
/****** Object:  View [dbo].[VW_QuotationFreeItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE   VIEW [dbo].[VW_QuotationFreeItems]
AS


	  SELECT

	   qd.ID
      ,qd.[ID_Quotation]
	  ,q.CustomerCode
	  ,cu.Name CustomerName
	  ,q.Subject
	  ,po.ID ID_ProductionOrder
	  ,q.Date CreationDate
	  ,q.ID_Manager
	  ,mg.Description ManagerDescription
	  ,qd.TypeCode
	  ,tp.Description TypeDescription
	  ,qd.ItemTypeCode 
	  ,itp.Description ItemTypeDescription
	  ,qd.ItemTypeDescription FreeTypeDescription
      ,qd.[SupplierCode]
	  ,su.Name SupplierName
      ,qd.UM
	  ,un.Description UMDescription
	  ,qd.[Cost]
      ,qd.[Price]
      ,qd.[Quantity]
      ,qd.[Inserted]
      ,qd.[Multiply]
      ,qd.[SelectPhase]
      ,qd.[MarkUp]
      ,qd.[Percentage]
      ,qd.[Save]


  FROM [LabExtim].[dbo].[QuotationDetails] qd
  
  inner join Quotations q on q.ID = qd.ID_Quotation
  left join ProductionOrders po on po.ID_Quotation = qd.ID_Quotation
  left join Types tp on qd.TypeCode = tp.Code
  left join ItemTypes itp on qd.ItemTypeCode = itp.Code
  left join Units un on qd.UM = un.ID
  left join Customers cu on cu.Code = q.CustomerCode
  left join Suppliers su on qd.SupplierCode = su.Code
  left join Managers mg on mg.ID= q.ID_Manager

  
  
  where commonkey is null and MacroItemKey is null and position <> ''
 
GO
/****** Object:  Table [dbo].[CustomersMarkUps]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersMarkUps](
	[Code] [int] NOT NULL,
	[MarkUp] [int] NULL,
	[Distance] [int] NULL,
 CONSTRAINT [PK_CustomersMarkUps] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Markup]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Markup]
AS
SELECT        dbo.Customers.Code, dbo.Customers.Type, dbo.Customers.Name, dbo.CustomersMarkUps.MarkUp, dbo.Customers.Email, dbo.Customers.Street, dbo.Customers.CAP, dbo.Customers.City, dbo.Customers.Province, 
                         dbo.Customers.P_IVA, dbo.Customers.Note, dbo.Customers.Phone
FROM            dbo.Customers INNER JOIN
                         dbo.CustomersMarkUps ON dbo.Customers.Code = dbo.CustomersMarkUps.Code
GO
/****** Object:  View [dbo].[VW_DDTQUOPORCostsPrices_select_Company02]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO













CREATE   VIEW [dbo].[VW_DDTQUOPORCostsPrices_select_Company02]
AS

SELECT 
ID_Customer,
mm_tipork collate Latin1_General_CI_AS TipoRec,
tm_serie collate Latin1_General_CI_AS SerieBolla,
tm_numdoc NumBolla,
tm_datdoc DataBolla,
--mm_descr,
sum(mm_quant) QtaBolla,
avg(mm_prezzo) PrezzoBolla,
sum(FATTotValue) FATTotValue,
sum(ProvvTotValue) ProvvTotValue,
mm_lotto ID,
ID_Company,
Number,
Status,
StartQuantity,
StartDate,
EndDate,
ID_Quotation,
QUOSubject,
QUOOwner,
OwnerName,
sum(PORPropCost) PORPropCost,
sum(PORPropDirectCost) PORPropDirectCost,
sum(QUOPropCost) QUOPropCost,
sum(PropProducedQuantity) PropProducedQuantity,
PORTotCost,
PORTotDirectCost,
QUOTotCost,
AccountNote,
Note,
IDAgente1,
DescrizioneAgente1,
PriceCom,
NonConformityCode,
ComplaintReceived,
CorrectiveActionCode

FROM
(
SELECT 
poc.ID_Customer,
mm_tipork,
ts.tm_serie,
ts.tm_numdoc,
ts.tm_datdoc,
mm_descr,
mm_quant,
mm_prezzo,
CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - dt.mm_valore) ELSE dt.mm_valore END FATTotValue,
CASE WHEN ts.tm_tipork LIKE 'N' THEN (0 - dt.mm_vprovv - dt.mm_vprovv2) ELSE dt.mm_vprovv + dt.mm_vprovv2 END ProvvTotValue,
dt.mm_lotto,
poc.ID_Company,
poc.Number,
poc.Status,
poc.StartQuantity,
poc.StartDate,
poc.EndDate,
qcp.ID_Quotation,
qcp.QUOSubject,
qcp.QUOOwner,
e.UniqueName AS OwnerName,

(select SUM(IsNull(poc1.PORDirectCost,0) + IsNull(poc1.PORDetailsCost,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto)  PORTotCost,

(select SUM(IsNull(poc1.PORDirectCost,0) + IsNull(poc1.PORDetailsCost,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 2 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PORPropCost, 

(select SUM(IsNull(poc2.PORDirectCost,0) + IsNull(poc2.PORDetailsHistoricalCost,0)) from VW_ProductionOrdersCosts poc2 where poc2.ID=dt.mm_lotto) PORTotDirectCost,

(select SUM(IsNull(poc2.PORDirectCost,0) + IsNull(poc2.PORDetailsHistoricalCost,0)) from VW_ProductionOrdersCosts poc2 where poc2.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 2 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PORPropDirectCost, 

(select SUM(IsNull(qcp3.QUORefFixCost,0) + 
IsNull((
Case When qcp3.Q1 <> 0 Then
qcp3.QUORefVarCost / qcp3.Q1
Else 0 End
) * poc3.StartQuantity ,0)) from VW_ProductionOrdersCosts poc3 left join dbo.VW_QuotationsCostsPrices qcp3 on qcp3.ID_Quotation = poc3.ID_Quotation where poc3.ID=dt.mm_lotto) AS QUOTotCost,

(select SUM(IsNull(qcp3.QUORefFixCost,0) + 
IsNull((
Case When qcp3.Q1 <> 0 Then
qcp3.QUORefVarCost / qcp3.Q1
Else 0 End
) * poc3.StartQuantity ,0)) from VW_ProductionOrdersCosts poc3 left join dbo.VW_QuotationsCostsPrices qcp3 on qcp3.ID_Quotation = poc3.ID_Quotation where poc3.ID=dt.mm_lotto)
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 2 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS QUOPropCost, 


(select SUM(IsNull(poc1.ProducedQuantity,0) + IsNull(poc1.ProducedQuantity,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto)  TotProducedQuantity,

(select SUM(IsNull(poc1.ProducedQuantity,0) + IsNull(poc1.ProducedQuantity,0)) from VW_ProductionOrdersCosts poc1 where poc1.ID=dt.mm_lotto) 
*
dt.mm_valore / (select SUM(IsNull(dt1.mm_valore,0)) FROM  Labe.dbo.movmag dt1
inner join Labe.dbo.testmag ts1 on dt1.mm_tipork =  ts1.tm_tipork and dt1.mm_anno = ts1.tm_anno and dt1.mm_serie = ts1.tm_serie and dt1.mm_numdoc =  ts1.tm_numdoc and dt1.codditt   = ts1.codditt WHERE (ts1.tm_tipork IN ('A','B','C','E','N')) 
and dt1.mm_lotto = dt.mm_lotto
and poc.ID_Company = 2 having SUM(IsNull(dt1.mm_valore,0)) <> 0) AS PropProducedQuantity, 


poc.AccountNote,
poc.Note,
c.IDAgente1,
c.DescrizioneAgente1,
qcp.PriceCom,
poc.NonConformityCode,
poc.ComplaintReceived,
poc.CorrectiveActionCode

FROM Labe.dbo.movmag dt 

inner join Labe.dbo.testmag ts on dt.mm_tipork =  ts.tm_tipork and dt.mm_anno = ts.tm_anno and dt.mm_serie = ts.tm_serie and dt.mm_numdoc =  ts.tm_numdoc and dt.codditt   = ts.codditt
inner join dbo.VW_ProductionOrdersCosts poc ON poc.ID = dt.mm_lotto
left join dbo.VW_QuotationsCostsPrices qcp on qcp.ID_Quotation = poc.ID_Quotation
left join dbo.Employees e ON qcp.QUOOwner = e.ID
left join dbo.Customers c ON poc.ID_Customer = c.Code


WHERE     
(ts.tm_tipork IN ('A','B','C','E','N')) and mm_valore <> 0

) d

Group By

ID_Customer,
mm_tipork,
tm_serie,
tm_numdoc,
tm_datdoc,
--mm_descr,
mm_lotto,
ID_Company,
Number,
Status,
StartQuantity,
StartDate,
EndDate,
ID_Quotation,
QUOSubject,
QUOOwner,
OwnerName,
PORTotCost,
PORTotDirectCost,
QUOTotCost,
AccountNote,
Note,
IDAgente1,
DescrizioneAgente1,
PriceCom,
NonConformityCode,
ComplaintReceived,
CorrectiveActionCode


GO
/****** Object:  Table [dbo].[QuotationTemplates]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationTemplates](
	[ID] [int] IDENTITY(2,1) NOT NULL,
	[TypeCode] [int] NOT NULL,
	[ItemTypeCode] [int] NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Q1] [int] NULL,
	[Q2] [int] NULL,
	[Q3] [int] NULL,
	[Q4] [int] NULL,
	[Q5] [int] NULL,
	[UM] [int] NOT NULL,
	[Cost] [decimal](18, 4) NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[Percentage] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Link] [nvarchar](50) NULL,
	[PILink] [nvarchar](50) NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Template] [int] NULL,
	[ItemManufacturing] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[Note1] [nvarchar](max) NULL,
	[Note2] [nvarchar](max) NULL,
 CONSTRAINT [PK_QuotationTemplates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VW_MenuQuotationTemplates]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[VW_MenuQuotationTemplates]
AS


SELECT 
	   'T' + cast(id as varchar) ID
      ,[TypeCode]
      ,[ItemTypeCode]
      ,[Description] ItemDescription
      ,[Inserted]
      ,[Link]
      ,[Order]
      ,cast(0 as decimal) Cost
  FROM [dbo].[QuotationTemplates]






GO
/****** Object:  View [dbo].[VW_FreeTypeDDT]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VW_FreeTypeDDT]
AS
SELECT    
MMA.mm_lotto,
                      TMA.tm_tipork, CAST(TMA.tm_anno AS varchar) + '/' + TMA.tm_serie + '/' + CAST(TMA.tm_numdoc AS varchar) AS NumDDT, 
                      MMA.mm_codart, Isnull(MMA.mm_descr,'') AS mm_descr, MMA.mm_unmis, MMA.mm_quant, MMA.mm_ump, MMA.mm_prezzo, MMA.mm_valore, 
                      (TMA.tm_datdoc) AS DataDDT, (TMA.tm_conto) AS CodCliente
FROM         
                      
                     
                      LABE.dbo.testmag AS TMA INNER JOIN
                      LABE.dbo.movmag AS MMA ON TMA.tm_tipork = MMA.mm_tipork AND TMA.tm_anno = MMA.mm_anno AND TMA.tm_serie = MMA.mm_serie AND 
                      TMA.tm_numdoc = MMA.mm_numdoc 
WHERE 
    ((TMA.tm_tipork IS NULL) OR 
                      (TMA.tm_tipork = 'A') OR
                      (TMA.tm_tipork = 'B'))
                      and MMA.mm_codart = '4'
                      And MMA.mm_valore <> 0 AND MMA.mm_lotto <> 0



GO
/****** Object:  Table [dbo].[Categories]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [char](1) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CloseOfDays]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CloseOfDays](
	[Id_User] [int] NOT NULL,
	[ProductionDate] [datetime] NOT NULL,
	[ID_Company] [int] NULL,
	[Note] [varchar](max) NULL,
 CONSTRAINT [PK_CloseOfDays] PRIMARY KEY CLUSTERED 
(
	[Id_User] ASC,
	[ProductionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configuration]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuration](
	[ConfigKey] [char](4) NOT NULL,
	[ConfigValue] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[ConfigKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerOrders]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerOrders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [int] NULL,
	[CustomerOrderCode] [nvarchar](50) NULL,
	[ID_Quotation] [int] NULL,
	[OrderDate] [datetime] NULL,
	[Quantity] [real] NULL,
	[ConfirmDate] [datetime] NULL,
	[LastDeliveryDate] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_CustomerOrders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomersKm]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersKm](
	[Code] [int] NOT NULL,
	[Distance] [int] NULL,
 CONSTRAINT [PK_CustomersKm] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomersToLabe]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersToLabe](
	[CodeCartolabe] [int] NOT NULL,
	[CodeLabe] [int] NOT NULL,
 CONSTRAINT [PK_CustomersToLabe] PRIMARY KEY CLUSTERED 
(
	[CodeCartolabe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryTripDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryTripDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_DeliveryTrip] [int] NOT NULL,
	[ID_ProductionOrder] [int] NULL,
	[Direction] [char](1) NOT NULL,
	[Quota] [decimal](18, 4) NULL,
	[ID_Owner] [int] NULL,
	[InsertDate] [datetime] NULL,
 CONSTRAINT [PK_DeliveryTripDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryTrips]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryTrips](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NULL,
	[ID_Company] [int] NULL,
	[CustomerCode] [int] NULL,
	[LocationCode] [int] NULL,
	[MacroRef] [int] NULL,
	[ID_Owner] [int] NULL,
	[StartDate] [datetime] NULL,
	[Status] [int] NULL,
	[EndDate] [datetime] NULL,
	[Note] [varchar](255) NULL,
 CONSTRAINT [PK_DeliveryTrips] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dependencies]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dependencies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LinkCode] [int] NULL,
	[Link] [char](10) NULL,
	[Dependent] [char](10) NULL,
 CONSTRAINT [PK_Dependencies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Find_Quotations]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Find_Quotations](
	[ID] [int] NOT NULL,
	[ID_Quotation] [int] NULL,
	[Number] [varchar](10) NULL,
	[CustomerCode] [int] NULL,
	[Subject] [nvarchar](200) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ID_Owner] [int] NULL,
	[ID_Manager] [int] NULL,
 CONSTRAINT [PK_Find_Quotations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Find_QuotationTemplates]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Find_QuotationTemplates](
	[ID] [int] NOT NULL,
	[ID_QuotationTemplate] [int] NULL,
	[TypeCode] [int] NULL,
	[ItemTypeCode] [int] NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Find_QuotationTemplates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Holydays]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holydays](
	[Day] [date] NOT NULL,
 CONSTRAINT [PK_Holydays] PRIMARY KEY CLUSTERED 
(
	[Day] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportFieldMappingInfo]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportFieldMappingInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OriginTableName] [nchar](50) NOT NULL,
	[DestTableName] [nchar](50) NOT NULL,
	[OriginFieldName] [nchar](50) NOT NULL,
	[DestFieldName] [nchar](50) NOT NULL,
 CONSTRAINT [PK_ImportFieldMappingInfo] PRIMARY KEY CLUSTERED 
(
	[OriginTableName] ASC,
	[DestTableName] ASC,
	[OriginFieldName] ASC,
	[DestFieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemDisplayModes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemDisplayModes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NOT NULL,
 CONSTRAINT [PK_ItemDisplayModes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Links]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Links](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NULL,
 CONSTRAINT [PK_Links] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Code] [int] IDENTITY(500000001,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Type] [nvarchar](1) NULL,
	[Contact] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](18) NULL,
	[Fax] [nvarchar](18) NULL,
	[Street] [nvarchar](70) NULL,
	[CAP] [nvarchar](9) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](2) NULL,
	[ID_Company] [int] NULL,
	[Distance] [int] NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locks]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LockTypeCode] [int] NOT NULL,
	[IDRef] [int] NOT NULL,
	[GUIDUser] [char](36) NOT NULL,
	[SessionID] [char](100) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Locks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LockTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LockTypes](
	[ID] [int] NOT NULL,
	[Description] [nchar](30) NOT NULL,
 CONSTRAINT [PK_LockTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MacroItems_COPY]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MacroItems_COPY](
	[ID] [int] NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[ItemTypeCode] [int] NOT NULL,
	[MacroItemDescription] [nvarchar](255) NULL,
	[UM] [int] NOT NULL,
	[Cost] [decimal](18, 4) NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[Percentage] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Link] [nvarchar](50) NULL,
	[PILink] [nvarchar](50) NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Template] [int] NULL,
	[ItemManufacturing] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[ExpandInStats] [bit] NOT NULL,
 CONSTRAINT [PK_MacroItems_COPY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItems](
	[MenuType] [int] NOT NULL,
	[Position] [nvarchar](20) NOT NULL,
	[Standard] [bit] NOT NULL,
	[Selectable] [bit] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](255) NULL,
	[ToolTip] [nvarchar](1000) NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[MenuType] ASC,
	[Position] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PickingItems_COPY]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PickingItems_COPY](
	[ID] [int] NOT NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[ItemTypeCode] [int] NOT NULL,
	[ItemDescription] [nvarchar](255) NULL,
	[UM] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[SupplierCode] [int] NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[Percentage] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Link] [nvarchar](50) NULL,
	[MILink] [nvarchar](50) NULL,
	[Order] [nvarchar](50) NOT NULL,
	[Template] [int] NULL,
	[ItemManufacturing] [int] NULL,
	[StandardPercentageCode] [int] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_PickingItems_COPY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionMachinesToPickingItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionMachinesToPickingItems](
	[IDPickingItem] [int] NOT NULL,
	[IDProductionMachine] [int] NOT NULL,
	[Priority] [int] NULL,
 CONSTRAINT [PK_ProductionMachinesToPickingItems] PRIMARY KEY CLUSTERED 
(
	[IDProductionMachine] ASC,
	[IDPickingItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionOrderTechSpecs]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionOrderTechSpecs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_ProductionOrder] [int] NULL,
	[ID_ProductionOrderDetail] [int] NULL,
	[ID_QuotationDetail] [int] NULL,
	[ID_Owner] [int] NULL,
	[ProductionDate] [datetime] NULL,
	[ID_Phase] [int] NULL,
	[CodiceMarcaFilm] [varchar](255) NULL,
	[ClicheReso] [varchar](1) NULL,
	[ClicheCondizioni] [varchar](1) NULL,
	[StampaTemperatura] [varchar](50) NULL,
	[AltreInfo] [varchar](255) NULL,
	[CodiceMarcaInchiostro] [varchar](255) NULL,
	[Ricetta] [bit] NULL,
	[TelaioNumeroFili] [varchar](50) NULL,
	[GelatinaSpessore] [varchar](50) NULL,
	[RaclaInclinazione] [varchar](50) NULL,
	[RaclaDurezzaSpigolo] [varchar](50) NULL,
	[FustellaResa] [varchar](1) NULL,
	[FustellaCondizioni] [varchar](1) NULL,
	[ControCordonatori] [varchar](256) NULL,
	[AltreNoteDaProduzione] [varchar](512) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ProductionOrderTechNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queries]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queries](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NULL,
 CONSTRAINT [PK_Queries] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationTemplateDetails]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationTemplateDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_QuotationTemplate] [int] NOT NULL,
	[Position] [nvarchar](50) NULL,
	[ID_Company] [int] NULL,
	[TypeCode] [int] NOT NULL,
	[CommonKey] [int] NULL,
	[MacroItemKey] [int] NULL,
	[ItemTypeCode] [int] NOT NULL,
	[ItemTypeDescription] [nvarchar](50) NULL,
	[UM] [int] NOT NULL,
	[Cost] [money] NULL,
	[Price] [money] NULL,
	[Quantity] [real] NOT NULL,
	[Inserted] [bit] NOT NULL,
	[Multiply] [bit] NOT NULL,
	[SelectPhase] [bit] NOT NULL,
	[MarkUp] [int] NULL,
	[SupplierCode] [int] NULL,
	[Save] [bit] NOT NULL,
	[Percentage] [int] NULL,
 CONSTRAINT [PK_QuotationTemplateDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ReportTypeCode] [int] NOT NULL,
 CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportTexts]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportTexts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReportTypeCode] [int] NOT NULL,
	[TextTypeCode] [int] NOT NULL,
	[Text] [nvarchar](max) NULL,
	[ID_Ref01] [int] NULL,
	[ID_Ref02] [int] NULL,
	[CreateDate] [datetime] NULL,
	[Standard] [bit] NOT NULL,
 CONSTRAINT [PK_ReportTypeDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Selectors]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selectors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QueryCode] [int] NULL,
	[SelectorType] [int] NULL,
	[SelectorCode] [int] NULL,
 CONSTRAINT [PK_Selectors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SelectorTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SelectorTypes](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NULL,
 CONSTRAINT [PK_SelectorTypes] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpecTypes]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecTypes](
	[Cod] [int] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_SpecTypes] PRIMARY KEY CLUSTERED 
(
	[Cod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StandardPercentages]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardPercentages](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[Percentage] [int] NOT NULL,
	[Description] [nchar](100) NULL,
 CONSTRAINT [PK_StandardPercentages] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StickingErrors]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StickingErrors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PhaseType] [nvarchar](255) NULL,
	[PhaseDescription] [nvarchar](255) NULL,
	[UM] [nvarchar](255) NULL,
	[Quantity] [real] NULL,
	[Cost1] [float] NULL,
	[Price1] [float] NULL,
	[Cost2] [float] NULL,
	[Price2] [float] NULL,
	[Cost3] [float] NULL,
	[Price3] [float] NULL,
 CONSTRAINT [PK_StickingErrors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockItems]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockItems](
	[Code] [nvarchar](10) NULL,
	[ItemType] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[UM] [nvarchar](255) NULL,
	[Cost] [money] NULL,
	[SupplierCode] [nvarchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SuppliersToLabe]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuppliersToLabe](
	[CodeCartolabe] [int] NOT NULL,
	[CodeLabe] [int] NOT NULL,
 CONSTRAINT [PK_SuppliersToLabe] PRIMARY KEY CLUSTERED 
(
	[CodeCartolabe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToolsConditions]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolsConditions](
	[id] [varchar](1) NOT NULL,
	[name] [varchar](255) NULL,
 CONSTRAINT [PK_ToolsConditions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToolsReturnedTo]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolsReturnedTo](
	[id] [varchar](1) NOT NULL,
	[name] [varchar](255) NULL,
 CONSTRAINT [PK_ToolsReturnedTo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnitConverters]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnitConverters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nchar](30) NOT NULL,
	[ID_UserUnit] [int] NOT NULL,
	[ID_FinalUnit] [int] NOT NULL,
	[Quotient] [int] NOT NULL,
 CONSTRAINT [PK_UnitConverters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [CustomerCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [CustomerCode] ON [dbo].[CustomerOrders]
(
	[CustomerCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Status]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Status] ON [dbo].[CustomerOrders]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [City]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [City] ON [dbo].[Customers]
(
	[City] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Name]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Name] ON [dbo].[Customers]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Province]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Province] ON [dbo].[Customers]
(
	[Province] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [DataBollaNumBolla]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [DataBollaNumBolla] ON [dbo].[DDTQUOPORCostsPrices]
(
	[DataBolla] ASC,
	[NumBolla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID] ON [dbo].[DDTQUOPORCostsPrices]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Guid]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Guid] ON [dbo].[Employees]
(
	[UserGUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Name_Surname]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Name_Surname] ON [dbo].[Employees]
(
	[Name] ASC,
	[Surname] ASC
)
INCLUDE([UserGUID],[ID_Company]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Code]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Code] ON [dbo].[ItemTypes]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Order]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Order] ON [dbo].[ItemTypes]
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Code]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Code] ON [dbo].[Links]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Common]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Common] ON [dbo].[Locks]
(
	[LockTypeCode] ASC,
	[IDRef] ASC,
	[GUIDUser] ASC,
	[SessionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Description]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Description] ON [dbo].[LockTypes]
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_MacroItem]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_MacroItem] ON [dbo].[MacroItemDetails]
(
	[ID_MacroItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Position]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Position] ON [dbo].[MacroItemDetails]
(
	[Position] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ItemTypeCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ItemTypeCode] ON [dbo].[MacroItems]
(
	[ItemTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Order]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Order] ON [dbo].[MacroItems]
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [TypeCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [TypeCode] ON [dbo].[MacroItems]
(
	[TypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ItemDescription]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ItemDescription] ON [dbo].[PickingItems]
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ItemTypeCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ItemTypeCode] ON [dbo].[PickingItems]
(
	[ItemTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Order]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Order] ON [dbo].[PickingItems]
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [TypeCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [TypeCode] ON [dbo].[PickingItems]
(
	[TypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [NonClusteredIndex-20170518-172416]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170518-172416] ON [dbo].[ProductionMachinesToPickingItems]
(
	[IDProductionMachine] ASC,
	[Priority] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDPickingItem]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [IDPickingItem] ON [dbo].[ProductionMPS]
(
	[IDPickingItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER INDEX [IDPickingItem] ON [dbo].[ProductionMPS] DISABLE
GO
/****** Object:  Index [IDProductionMachine]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [IDProductionMachine] ON [dbo].[ProductionMPS]
(
	[IDProductionMachine] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IDProductionOrder]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [IDProductionOrder] ON [dbo].[ProductionMPS]
(
	[IDProductionOrder] ASC,
	[Status] ASC
)
INCLUDE([IDQuotationDetail]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ProdEnd]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ProdEnd] ON [dbo].[ProductionMPS]
(
	[ProdEnd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER INDEX [ProdEnd] ON [dbo].[ProductionMPS] DISABLE
GO
/****** Object:  Index [ProdStart]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ProdStart] ON [dbo].[ProductionMPS]
(
	[ProdStart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER INDEX [ProdStart] ON [dbo].[ProductionMPS] DISABLE
GO
/****** Object:  Index [ProdTimeMin]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ProdTimeMin] ON [dbo].[ProductionMPS]
(
	[ProdTimeMin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER INDEX [ProdTimeMin] ON [dbo].[ProductionMPS] DISABLE
GO
/****** Object:  Index [<Name of Missing Index, sysname,>]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[ProductionMPSExceptions]
(
	[IDQuotationDetail] ASC
)
INCLUDE([NewOrder]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Owner]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Owner] ON [dbo].[ProductionOrderDetails]
(
	[ID_Owner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Phase]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Phase] ON [dbo].[ProductionOrderDetails]
(
	[ID_Phase] ASC
)
INCLUDE([ProductionTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_PickingItem]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_PickingItem] ON [dbo].[ProductionOrderDetails]
(
	[ID_PickingItem] ASC
)
INCLUDE([ProductionTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_PickingItemSup]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_PickingItemSup] ON [dbo].[ProductionOrderDetails]
(
	[ID_PickingItemSup] ASC
)
INCLUDE([ProductionTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_ProductionOrder]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_ProductionOrder] ON [dbo].[ProductionOrderDetails]
(
	[ID_ProductionOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_QuotationDetail]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_QuotationDetail] ON [dbo].[ProductionOrderDetails]
(
	[ID_QuotationDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ProductionDate]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ProductionDate] ON [dbo].[ProductionOrderDetails]
(
	[ProductionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ProductionDateId_Owner]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ProductionDateId_Owner] ON [dbo].[ProductionOrderDetails]
(
	[ID_Owner] ASC,
	[ProductionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [SupplierCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [SupplierCode] ON [dbo].[ProductionOrderDetails]
(
	[SupplierCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [DeliveryDate]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [DeliveryDate] ON [dbo].[ProductionOrders]
(
	[DeliveryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Customer]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Customer] ON [dbo].[ProductionOrders]
(
	[ID_Customer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_CustomerOrder]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_CustomerOrder] ON [dbo].[ProductionOrders]
(
	[ID_CustomerOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Quotation]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Quotation] ON [dbo].[ProductionOrders]
(
	[ID_Quotation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Number]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Number] ON [dbo].[ProductionOrders]
(
	[ID_Company] ASC,
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Status]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Status] ON [dbo].[ProductionOrders]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [YearStartDate]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [YearStartDate] ON [dbo].[ProductionOrders]
(
	[YearStartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IdPoIdQd]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [IdPoIdQd] ON [dbo].[ProductionTimeStamps]
(
	[IDProductionOrder] ASC,
	[MinIDQuotationDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Code]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Code] ON [dbo].[Queries]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [DataBolla]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [DataBolla] ON [dbo].[QUOPORCostsPrices]
(
	[DataBolla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [PORID_Customer]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [PORID_Customer] ON [dbo].[QUOPORCostsPrices]
(
	[PORID_Customer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [<Name of Missing Index, sysname,>]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[QuotationDetails]
(
	[ID_Quotation] ASC,
	[ID_Company] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Quotation]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Quotation] ON [dbo].[QuotationDetails]
(
	[ItemTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [CustomerCode]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [CustomerCode] ON [dbo].[Quotations]
(
	[CustomerCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Date]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Date] ON [dbo].[Quotations]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_Owner]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_Owner] ON [dbo].[Quotations]
(
	[ID_Owner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Subject]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Subject] ON [dbo].[Quotations]
(
	[Subject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ID_QuotationTemplate]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [ID_QuotationTemplate] ON [dbo].[QuotationTemplateDetails]
(
	[ID_QuotationTemplate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Code]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Code] ON [dbo].[SelectorTypes]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [StatusType]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [StatusType] ON [dbo].[Statuses]
(
	[StatusType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Name]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Name] ON [dbo].[Suppliers]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Code]    Script Date: 07/04/2026 11:38:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Code] ON [dbo].[Types]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Order]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Order] ON [dbo].[Types]
(
	[Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Description]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Description] ON [dbo].[UnitConverters]
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Description]    Script Date: 07/04/2026 11:38:22 ******/
CREATE NONCLUSTERED INDEX [Description] ON [dbo].[Units]
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomersMarkUps] ADD  CONSTRAINT [DF_CustomersMarkUps_MarkUp]  DEFAULT ((0)) FOR [MarkUp]
GO
ALTER TABLE [dbo].[MacroItemDetails] ADD  CONSTRAINT [DF_MacroItemDetails_Position]  DEFAULT (N'ZZZZZZZZZZ') FOR [Position]
GO
ALTER TABLE [dbo].[MacroItemDetails] ADD  CONSTRAINT [DF_MacroItemDetails_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[MacroItemDetails] ADD  CONSTRAINT [DF_MacroItemDetails_Inserted]  DEFAULT ((0)) FOR [Inserted]
GO
ALTER TABLE [dbo].[MacroItemDetails] ADD  CONSTRAINT [DF_MacroItemDetails_Multiply]  DEFAULT ((0)) FOR [Multiply]
GO
ALTER TABLE [dbo].[MacroItemDetails] ADD  CONSTRAINT [DF_MacroItemDetails_SelectPhase]  DEFAULT ((1)) FOR [SelectPhase]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_Inserted]  DEFAULT ((1)) FOR [Inserted]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_Multiply]  DEFAULT ((1)) FOR [Multiply]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_Order]  DEFAULT (N'ZZZZZZZZZZ') FOR [Order]
GO
ALTER TABLE [dbo].[MacroItems] ADD  CONSTRAINT [DF_MacroItems_ExpandInStats]  DEFAULT ((0)) FOR [ExpandInStats]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Inserted]  DEFAULT ((1)) FOR [Inserted]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Multiply]  DEFAULT ((1)) FOR [Multiply]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Percentage]  DEFAULT ((100)) FOR [Percentage]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_Order]  DEFAULT (N'ZZZZZZZZZZ') FOR [Order]
GO
ALTER TABLE [dbo].[PickingItems] ADD  CONSTRAINT [DF_PickingItems_ItemManufacturing]  DEFAULT ((1)) FOR [ItemManufacturing]
GO
ALTER TABLE [dbo].[ProductionMachines] ADD  CONSTRAINT [DF_ProductionMachines_Inserted]  DEFAULT ((1)) FOR [Inserted]
GO
ALTER TABLE [dbo].[ProductionOrderDetails] ADD  CONSTRAINT [DF_ProductionOrderDetails_QuantityOver]  DEFAULT ((0)) FOR [QuantityOver]
GO
ALTER TABLE [dbo].[ProductionOrderDetails] ADD  CONSTRAINT [DF_ProductionOrderDetails_DirectSupply]  DEFAULT ((0)) FOR [DirectSupply]
GO
ALTER TABLE [dbo].[ProductionOrderDetails] ADD  CONSTRAINT [CreateTS_DF]  DEFAULT (getdate()) FOR [CreateTS]
GO
ALTER TABLE [dbo].[ProductionOrders] ADD  CONSTRAINT [DF_ProductionOrders_DirectSupply]  DEFAULT ((0)) FOR [DirectSupply]
GO
ALTER TABLE [dbo].[ProductionOrders] ADD  CONSTRAINT [DF_ProductionOrders_ComplaintReceived]  DEFAULT ((0)) FOR [ComplaintReceived]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] ADD  CONSTRAINT [DF_ProductionOrderTechSpecs_Status]  DEFAULT ((17)) FOR [Status]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Position]  DEFAULT (N'ZZZZZZZZZZ') FOR [Position]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Inserted]  DEFAULT ((0)) FOR [Inserted]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Multiply]  DEFAULT ((0)) FOR [Multiply]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_SelectPhase]  DEFAULT ((1)) FOR [SelectPhase]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_MarkUp]  DEFAULT ((0)) FOR [MarkUp]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Percentage]  DEFAULT ((100)) FOR [Percentage]
GO
ALTER TABLE [dbo].[QuotationDetails] ADD  CONSTRAINT [DF_QuotationDetails_Save]  DEFAULT ((0)) FOR [Save]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] ADD  CONSTRAINT [DF_QuotationTemplateDetails_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] ADD  CONSTRAINT [DF_QuotationTemplateDetails_SelectPhase]  DEFAULT ((0)) FOR [SelectPhase]
GO
ALTER TABLE [dbo].[QuotationTemplates] ADD  CONSTRAINT [DF_QuotationTemplates_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[QuotationTemplates] ADD  CONSTRAINT [DF_QuotationTemplates_Inserted]  DEFAULT ((1)) FOR [Inserted]
GO
ALTER TABLE [dbo].[QuotationTemplates] ADD  CONSTRAINT [DF_QuotationTemplates_Multiply]  DEFAULT ((1)) FOR [Multiply]
GO
ALTER TABLE [dbo].[QuotationTemplates] ADD  CONSTRAINT [DF_QuotationTemplates_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[QuotationTemplates] ADD  CONSTRAINT [DF_QuotationTemplates_Order]  DEFAULT (N'ZZZZZZZZZZ') FOR [Order]
GO
ALTER TABLE [dbo].[StandardPercentages] ADD  CONSTRAINT [DF_Standard_Percentage]  DEFAULT ((0)) FOR [Percentage]
GO
ALTER TABLE [dbo].[Suppliers] ADD  CONSTRAINT [DF_Suppliers_MarkUp]  DEFAULT ((0)) FOR [MarkUp]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Position]  DEFAULT (N'ZZZZZZZZZZ') FOR [Position]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Inserted]  DEFAULT ((0)) FOR [Inserted]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Multiply]  DEFAULT ((0)) FOR [Multiply]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_SelectPhase]  DEFAULT ((1)) FOR [SelectPhase]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_MarkUp]  DEFAULT ((0)) FOR [MarkUp]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Percentage]  DEFAULT ((100)) FOR [Percentage]
GO
ALTER TABLE [dbo].[TempQuotationDetails] ADD  CONSTRAINT [DF_TempQuotationDetails_Save]  DEFAULT ((0)) FOR [Save]
GO
ALTER TABLE [dbo].[CloseOfDays]  WITH NOCHECK ADD  CONSTRAINT [FK_CloseOfDays_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[CloseOfDays] CHECK CONSTRAINT [FK_CloseOfDays_Companies]
GO
ALTER TABLE [dbo].[CloseOfDays]  WITH NOCHECK ADD  CONSTRAINT [FK_CloseOfDays_Employees] FOREIGN KEY([Id_User])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[CloseOfDays] CHECK CONSTRAINT [FK_CloseOfDays_Employees]
GO
ALTER TABLE [dbo].[CustomerNicknames]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerNicknames_Customers] FOREIGN KEY([Code])
REFERENCES [dbo].[Customers] ([Code])
GO
ALTER TABLE [dbo].[CustomerNicknames] CHECK CONSTRAINT [FK_CustomerNicknames_Customers]
GO
ALTER TABLE [dbo].[CustomerOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerOrders_Customers] FOREIGN KEY([CustomerCode])
REFERENCES [dbo].[Customers] ([Code])
GO
ALTER TABLE [dbo].[CustomerOrders] CHECK CONSTRAINT [FK_CustomerOrders_Customers]
GO
ALTER TABLE [dbo].[CustomerOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerOrders_Quotations] FOREIGN KEY([ID_Quotation])
REFERENCES [dbo].[Quotations] ([ID])
GO
ALTER TABLE [dbo].[CustomerOrders] CHECK CONSTRAINT [FK_CustomerOrders_Quotations]
GO
ALTER TABLE [dbo].[CustomerOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomerOrders_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[CustomerOrders] CHECK CONSTRAINT [FK_CustomerOrders_Statuses]
GO
ALTER TABLE [dbo].[CustomersMarkUps]  WITH NOCHECK ADD  CONSTRAINT [FK_CustomersMarkUps_Customers] FOREIGN KEY([Code])
REFERENCES [dbo].[Customers] ([Code])
GO
ALTER TABLE [dbo].[CustomersMarkUps] CHECK CONSTRAINT [FK_CustomersMarkUps_Customers]
GO
ALTER TABLE [dbo].[DeliveryTripDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTripDetails_DeliveryTrips] FOREIGN KEY([ID_DeliveryTrip])
REFERENCES [dbo].[DeliveryTrips] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTripDetails] CHECK CONSTRAINT [FK_DeliveryTripDetails_DeliveryTrips]
GO
ALTER TABLE [dbo].[DeliveryTripDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTripDetails_Employees] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTripDetails] CHECK CONSTRAINT [FK_DeliveryTripDetails_Employees]
GO
ALTER TABLE [dbo].[DeliveryTripDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTripDetails_ProductionOrders] FOREIGN KEY([ID_ProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTripDetails] CHECK CONSTRAINT [FK_DeliveryTripDetails_ProductionOrders]
GO
ALTER TABLE [dbo].[DeliveryTrips]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTrips_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTrips] CHECK CONSTRAINT [FK_DeliveryTrips_Companies]
GO
ALTER TABLE [dbo].[DeliveryTrips]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTrips_Customers] FOREIGN KEY([CustomerCode])
REFERENCES [dbo].[Customers] ([Code])
GO
ALTER TABLE [dbo].[DeliveryTrips] CHECK CONSTRAINT [FK_DeliveryTrips_Customers]
GO
ALTER TABLE [dbo].[DeliveryTrips]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTrips_Employees] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTrips] CHECK CONSTRAINT [FK_DeliveryTrips_Employees]
GO
ALTER TABLE [dbo].[DeliveryTrips]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTrips_Locations] FOREIGN KEY([LocationCode])
REFERENCES [dbo].[Locations] ([Code])
GO
ALTER TABLE [dbo].[DeliveryTrips] CHECK CONSTRAINT [FK_DeliveryTrips_Locations]
GO
ALTER TABLE [dbo].[DeliveryTrips]  WITH NOCHECK ADD  CONSTRAINT [FK_DeliveryTrips_MacroItems] FOREIGN KEY([MacroRef])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[DeliveryTrips] CHECK CONSTRAINT [FK_DeliveryTrips_MacroItems]
GO
ALTER TABLE [dbo].[Departments]  WITH NOCHECK ADD  CONSTRAINT [FK_Departments_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[Departments] CHECK CONSTRAINT [FK_Departments_Companies]
GO
ALTER TABLE [dbo].[Dependencies]  WITH NOCHECK ADD  CONSTRAINT [FK_Dependencies_Links] FOREIGN KEY([LinkCode])
REFERENCES [dbo].[Links] ([Code])
GO
ALTER TABLE [dbo].[Dependencies] CHECK CONSTRAINT [FK_Dependencies_Links]
GO
ALTER TABLE [dbo].[Employees]  WITH NOCHECK ADD  CONSTRAINT [FK_Employees_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Companies]
GO
ALTER TABLE [dbo].[Employees]  WITH NOCHECK ADD  CONSTRAINT [FK_Employees_Employees] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Employees]
GO
ALTER TABLE [dbo].[Employees]  WITH NOCHECK ADD  CONSTRAINT [FK_Employees_UserRoles] FOREIGN KEY([Role])
REFERENCES [dbo].[UserRoles] ([ID])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_UserRoles]
GO
ALTER TABLE [dbo].[Find_Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Find_Quotations_Customers] FOREIGN KEY([CustomerCode])
REFERENCES [dbo].[Customers] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Find_Quotations] CHECK CONSTRAINT [FK_Find_Quotations_Customers]
GO
ALTER TABLE [dbo].[Find_Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Find_Quotations_Employees] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[Find_Quotations] CHECK CONSTRAINT [FK_Find_Quotations_Employees]
GO
ALTER TABLE [dbo].[Find_Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Find_Quotations_Managers] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Managers] ([ID])
GO
ALTER TABLE [dbo].[Find_Quotations] CHECK CONSTRAINT [FK_Find_Quotations_Managers]
GO
ALTER TABLE [dbo].[ItemTypes]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemTypes_Categories] FOREIGN KEY([Category])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[ItemTypes] CHECK CONSTRAINT [FK_ItemTypes_Categories]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Companies]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_DayFractions] FOREIGN KEY([DayFraction])
REFERENCES [dbo].[DayFractions] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_DayFractions]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Employees] FOREIGN KEY([ID_Applicant])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Employees]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Employees1] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Employees1]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Employees2] FOREIGN KEY([ID_Approver])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Employees2]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_LeaveTypes] FOREIGN KEY([LeaveType])
REFERENCES [dbo].[LeaveTypes] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_LeaveTypes]
GO
ALTER TABLE [dbo].[LeaveRequests]  WITH CHECK ADD  CONSTRAINT [FK_LeaveRequests_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[LeaveRequests] CHECK CONSTRAINT [FK_LeaveRequests_Statuses]
GO
ALTER TABLE [dbo].[Locations]  WITH NOCHECK ADD  CONSTRAINT [FK_Locations_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Companies]
GO
ALTER TABLE [dbo].[MacroItemDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItemDetails_MacroItems] FOREIGN KEY([ID_MacroItem])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[MacroItemDetails] CHECK CONSTRAINT [FK_MacroItemDetails_MacroItems]
GO
ALTER TABLE [dbo].[MacroItemDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItemDetails_PickingItems] FOREIGN KEY([CommonKey])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[MacroItemDetails] CHECK CONSTRAINT [FK_MacroItemDetails_PickingItems]
GO
ALTER TABLE [dbo].[MacroItems]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItems_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[MacroItems] CHECK CONSTRAINT [FK_MacroItems_Companies]
GO
ALTER TABLE [dbo].[MacroItems]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItems_ItemTypes] FOREIGN KEY([ItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[MacroItems] CHECK CONSTRAINT [FK_MacroItems_ItemTypes]
GO
ALTER TABLE [dbo].[MacroItems]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItems_Types] FOREIGN KEY([TypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[MacroItems] CHECK CONSTRAINT [FK_MacroItems_Types]
GO
ALTER TABLE [dbo].[MacroItems]  WITH NOCHECK ADD  CONSTRAINT [FK_MacroItems_Units] FOREIGN KEY([UM])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[MacroItems] CHECK CONSTRAINT [FK_MacroItems_Units]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_Companies]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_ItemDisplayModes] FOREIGN KEY([ItemManufacturing])
REFERENCES [dbo].[ItemDisplayModes] ([ID])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_ItemDisplayModes]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_ItemTypes] FOREIGN KEY([ItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_ItemTypes]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_StandardPercentages] FOREIGN KEY([StandardPercentageCode])
REFERENCES [dbo].[StandardPercentages] ([Code])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_StandardPercentages]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_Suppliers] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Suppliers] ([Code])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_Suppliers]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_Types] FOREIGN KEY([TypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_Types]
GO
ALTER TABLE [dbo].[PickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_PickingItems_Units] FOREIGN KEY([UM])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[PickingItems] CHECK CONSTRAINT [FK_PickingItems_Units]
GO
ALTER TABLE [dbo].[PlasticCoatingMachineParameters]  WITH NOCHECK ADD  CONSTRAINT [FK_PlasticCoatingMachineParameters_ProductionOrders] FOREIGN KEY([Id_ProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[PlasticCoatingMachineParameters] CHECK CONSTRAINT [FK_PlasticCoatingMachineParameters_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionMachines]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMachines_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[ProductionMachines] CHECK CONSTRAINT [FK_ProductionMachines_Companies]
GO
ALTER TABLE [dbo].[ProductionMachines]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMachines_Companies1] FOREIGN KEY([ID_ExternalCompany])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[ProductionMachines] CHECK CONSTRAINT [FK_ProductionMachines_Companies1]
GO
ALTER TABLE [dbo].[ProductionMachines]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMachines_Departments] FOREIGN KEY([IDDepartment])
REFERENCES [dbo].[Departments] ([ID])
GO
ALTER TABLE [dbo].[ProductionMachines] CHECK CONSTRAINT [FK_ProductionMachines_Departments]
GO
ALTER TABLE [dbo].[ProductionMachinesToPickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMachinesToPickingItems_PickingItems] FOREIGN KEY([IDPickingItem])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionMachinesToPickingItems] CHECK CONSTRAINT [FK_ProductionMachinesToPickingItems_PickingItems]
GO
ALTER TABLE [dbo].[ProductionMachinesToPickingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMachinesToPickingItems_ProductionMachines] FOREIGN KEY([IDProductionMachine])
REFERENCES [dbo].[ProductionMachines] ([ID])
GO
ALTER TABLE [dbo].[ProductionMachinesToPickingItems] CHECK CONSTRAINT [FK_ProductionMachinesToPickingItems_ProductionMachines]
GO
ALTER TABLE [dbo].[ProductionMPS]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPS_MacroItems] FOREIGN KEY([IDMacroItem])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPS] CHECK CONSTRAINT [FK_ProductionMPS_MacroItems]
GO
ALTER TABLE [dbo].[ProductionMPS]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPS_PickingItems] FOREIGN KEY([IDPickingItem])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPS] CHECK CONSTRAINT [FK_ProductionMPS_PickingItems]
GO
ALTER TABLE [dbo].[ProductionMPS]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPS_ProductionMachines] FOREIGN KEY([IDProductionMachine])
REFERENCES [dbo].[ProductionMachines] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPS] CHECK CONSTRAINT [FK_ProductionMPS_ProductionMachines]
GO
ALTER TABLE [dbo].[ProductionMPS]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPS_ProductionOrders] FOREIGN KEY([IDProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPS] CHECK CONSTRAINT [FK_ProductionMPS_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionMPS]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPS_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPS] CHECK CONSTRAINT [FK_ProductionMPS_Statuses]
GO
ALTER TABLE [dbo].[ProductionMPSExceptions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPSExceptions_PickingItems] FOREIGN KEY([IDPickingItem])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPSExceptions] CHECK CONSTRAINT [FK_ProductionMPSExceptions_PickingItems]
GO
ALTER TABLE [dbo].[ProductionMPSExceptions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPSExceptions_ProductionMachines] FOREIGN KEY([OldIDProductionMachine])
REFERENCES [dbo].[ProductionMachines] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPSExceptions] CHECK CONSTRAINT [FK_ProductionMPSExceptions_ProductionMachines]
GO
ALTER TABLE [dbo].[ProductionMPSExceptions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPSExceptions_ProductionOrders] FOREIGN KEY([IDProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPSExceptions] CHECK CONSTRAINT [FK_ProductionMPSExceptions_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionMPSExceptions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionMPSExceptions_QuotationDetails] FOREIGN KEY([IDQuotationDetail])
REFERENCES [dbo].[QuotationDetails] ([ID])
GO
ALTER TABLE [dbo].[ProductionMPSExceptions] CHECK CONSTRAINT [FK_ProductionMPSExceptions_QuotationDetails]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Companies]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_DeliveryTrips] FOREIGN KEY([ID_DeliveryTrip])
REFERENCES [dbo].[DeliveryTrips] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_DeliveryTrips]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Employees] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Employees]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_ItemTypes] FOREIGN KEY([FreeItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_ItemTypes]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_PickingItems] FOREIGN KEY([ID_Phase])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_PickingItems]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_ProductionOrders] FOREIGN KEY([ID_ProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_QuotationDetails] FOREIGN KEY([ID_QuotationDetail])
REFERENCES [dbo].[QuotationDetails] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_QuotationDetails]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Suppliers] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Suppliers] ([Code])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Suppliers]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Suppliers1] FOREIGN KEY([SupplierCodeSup])
REFERENCES [dbo].[Suppliers] ([Code])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Suppliers1]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Types] FOREIGN KEY([FreeTypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Types]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Units] FOREIGN KEY([UMProduct])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Units]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Units1] FOREIGN KEY([UMRawMaterial])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Units1]
GO
ALTER TABLE [dbo].[ProductionOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderDetails_Units2] FOREIGN KEY([UMUser])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderDetails] CHECK CONSTRAINT [FK_ProductionOrderDetails_Units2]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Companies]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_CorrectiveActions] FOREIGN KEY([CorrectiveActionCode])
REFERENCES [dbo].[CorrectiveActions] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_CorrectiveActions]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_CustomerOrders] FOREIGN KEY([ID_CustomerOrder])
REFERENCES [dbo].[CustomerOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_CustomerOrders]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Customers] FOREIGN KEY([ID_Customer])
REFERENCES [dbo].[Customers] ([Code])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Customers]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Employees] FOREIGN KEY([ID_Contractor])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Employees]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Managers] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Managers] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Managers]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_NonConformities] FOREIGN KEY([NonConformityCode])
REFERENCES [dbo].[NonConformities] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_NonConformities]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_ProductionOrders] FOREIGN KEY([ID])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Quotations] FOREIGN KEY([ID_Quotation])
REFERENCES [dbo].[Quotations] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Quotations]
GO
ALTER TABLE [dbo].[ProductionOrders]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrders_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrders] CHECK CONSTRAINT [FK_ProductionOrders_Statuses]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_Employees] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_Employees]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_PickingItems] FOREIGN KEY([ID_Phase])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_PickingItems]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ProductionOrderDetails] FOREIGN KEY([ID_ProductionOrderDetail])
REFERENCES [dbo].[ProductionOrderDetails] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ProductionOrderDetails]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ProductionOrders] FOREIGN KEY([ID_ProductionOrder])
REFERENCES [dbo].[ProductionOrders] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ProductionOrders]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_QuotationDetails] FOREIGN KEY([ID_QuotationDetail])
REFERENCES [dbo].[QuotationDetails] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_QuotationDetails]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_Statuses]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsConditions] FOREIGN KEY([ClicheCondizioni])
REFERENCES [dbo].[ToolsConditions] ([id])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsConditions]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsConditions1] FOREIGN KEY([FustellaCondizioni])
REFERENCES [dbo].[ToolsConditions] ([id])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsConditions1]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsReturnedTo] FOREIGN KEY([ClicheReso])
REFERENCES [dbo].[ToolsReturnedTo] ([id])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsReturnedTo]
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsReturnedTo1] FOREIGN KEY([FustellaResa])
REFERENCES [dbo].[ToolsReturnedTo] ([id])
GO
ALTER TABLE [dbo].[ProductionOrderTechSpecs] CHECK CONSTRAINT [FK_ProductionOrderTechSpecs_ToolsReturnedTo1]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_Companies]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_ItemTypes] FOREIGN KEY([ItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_ItemTypes]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_MacroItems] FOREIGN KEY([MacroItemKey])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_MacroItems]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_PickingItems] FOREIGN KEY([CommonKey])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_PickingItems]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_Quotations] FOREIGN KEY([ID_Quotation])
REFERENCES [dbo].[Quotations] ([ID])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_Quotations]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_Suppliers] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Suppliers] ([Code])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_Suppliers]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_Types] FOREIGN KEY([TypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_Types]
GO
ALTER TABLE [dbo].[QuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDetails_Units] FOREIGN KEY([UM])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[QuotationDetails] CHECK CONSTRAINT [FK_QuotationDetails_Units]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Approver] FOREIGN KEY([ID_Approver])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Approver]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Companies]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Customers] FOREIGN KEY([CustomerCode])
REFERENCES [dbo].[Customers] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Customers]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Managers] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Managers] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Managers]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Owner] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Owner]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Quotations] FOREIGN KEY([ID])
REFERENCES [dbo].[Quotations] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Quotations]
GO
ALTER TABLE [dbo].[Quotations]  WITH NOCHECK ADD  CONSTRAINT [FK_Quotations_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotations_Statuses]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_Companies]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_ItemTypes] FOREIGN KEY([ItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_ItemTypes]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_MacroItems] FOREIGN KEY([MacroItemKey])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_MacroItems]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_PickingItems] FOREIGN KEY([CommonKey])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_PickingItems]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_QuotationTemplates] FOREIGN KEY([ID_QuotationTemplate])
REFERENCES [dbo].[QuotationTemplates] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_QuotationTemplates]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_Suppliers] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Suppliers] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_Suppliers]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_Types] FOREIGN KEY([TypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_Types]
GO
ALTER TABLE [dbo].[QuotationTemplateDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationTemplateDetails_Units] FOREIGN KEY([UM])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[QuotationTemplateDetails] CHECK CONSTRAINT [FK_QuotationTemplateDetails_Units]
GO
ALTER TABLE [dbo].[Selectors]  WITH NOCHECK ADD  CONSTRAINT [FK_Selectors_Queries] FOREIGN KEY([QueryCode])
REFERENCES [dbo].[Queries] ([Code])
GO
ALTER TABLE [dbo].[Selectors] CHECK CONSTRAINT [FK_Selectors_Queries]
GO
ALTER TABLE [dbo].[Selectors]  WITH NOCHECK ADD  CONSTRAINT [FK_Selectors_SelectorTypes] FOREIGN KEY([SelectorType])
REFERENCES [dbo].[SelectorTypes] ([Code])
GO
ALTER TABLE [dbo].[Selectors] CHECK CONSTRAINT [FK_Selectors_SelectorTypes]
GO
ALTER TABLE [dbo].[Statuses]  WITH NOCHECK ADD  CONSTRAINT [FK_Statuses_StatusTypes] FOREIGN KEY([StatusType])
REFERENCES [dbo].[StatusTypes] ([ID])
GO
ALTER TABLE [dbo].[Statuses] CHECK CONSTRAINT [FK_Statuses_StatusTypes]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_Companies]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_ItemTypes] FOREIGN KEY([ItemTypeCode])
REFERENCES [dbo].[ItemTypes] ([Code])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_ItemTypes]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_MacroItems] FOREIGN KEY([MacroItemKey])
REFERENCES [dbo].[MacroItems] ([ID])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_MacroItems]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_PickingItems] FOREIGN KEY([CommonKey])
REFERENCES [dbo].[PickingItems] ([ID])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_PickingItems]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_Suppliers] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Suppliers] ([Code])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_Suppliers]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_TempQuotations] FOREIGN KEY([SessionUser], [ID_Quotation])
REFERENCES [dbo].[TempQuotations] ([SessionUser], [ID_Quotation])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_TempQuotations]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_Types] FOREIGN KEY([TypeCode])
REFERENCES [dbo].[Types] ([Code])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_Types]
GO
ALTER TABLE [dbo].[TempQuotationDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotationDetails_Units] FOREIGN KEY([UM])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[TempQuotationDetails] CHECK CONSTRAINT [FK_TempQuotationDetails_Units]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Approver] FOREIGN KEY([ID_Approver])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Approver]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Companies] FOREIGN KEY([ID_Company])
REFERENCES [dbo].[Companies] ([ID])
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Companies]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Customers] FOREIGN KEY([CustomerCode])
REFERENCES [dbo].[Customers] ([Code])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Customers]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Managers] FOREIGN KEY([ID_Manager])
REFERENCES [dbo].[Managers] ([ID])
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Managers]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Owner] FOREIGN KEY([ID_Owner])
REFERENCES [dbo].[Employees] ([ID])
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Owner]
GO
ALTER TABLE [dbo].[TempQuotations]  WITH NOCHECK ADD  CONSTRAINT [FK_TempQuotations_Statuses] FOREIGN KEY([Status])
REFERENCES [dbo].[Statuses] ([ID])
GO
ALTER TABLE [dbo].[TempQuotations] CHECK CONSTRAINT [FK_TempQuotations_Statuses]
GO
ALTER TABLE [dbo].[Types]  WITH NOCHECK ADD  CONSTRAINT [FK_Types_Categories] FOREIGN KEY([Category])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Types] CHECK CONSTRAINT [FK_Types_Categories]
GO
ALTER TABLE [dbo].[UnitConverters]  WITH NOCHECK ADD  CONSTRAINT [FK_UnitConverters_Units] FOREIGN KEY([ID_UserUnit])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[UnitConverters] CHECK CONSTRAINT [FK_UnitConverters_Units]
GO
ALTER TABLE [dbo].[UnitConverters]  WITH NOCHECK ADD  CONSTRAINT [FK_UnitConverters_Units1] FOREIGN KEY([ID_FinalUnit])
REFERENCES [dbo].[Units] ([ID])
GO
ALTER TABLE [dbo].[UnitConverters] CHECK CONSTRAINT [FK_UnitConverters_Units1]
GO
/****** Object:  Trigger [dbo].[ProductionOrderDetails_ProducedQuantity]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE TRIGGER [dbo].[ProductionOrderDetails_ProducedQuantity] ON [dbo].[ProductionOrderDetails]
AFTER insert, UPDATE
AS

BEGIN
 
	if update (OkCopiesCount) or update (KoCopiesCount)
	begin
		UPDATE dbo.ProductionOrderDetails
		SET ProducedQuantity = isnull(inserted.OkCopiesCount,0) + isnull(inserted.KoCopiesCount,0)
		From  dbo.ProductionOrderDetails myAlias inner join inserted on
		myAlias.Id = inserted.id and (inserted.OkCopiesCount is not null)
	end

END
GO
ALTER TABLE [dbo].[ProductionOrderDetails] ENABLE TRIGGER [ProductionOrderDetails_ProducedQuantity]
GO
/****** Object:  Trigger [dbo].[ProductionOrderDetails_UPDATE_INSERT]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ProductionOrderDetails_UPDATE_INSERT] ON [dbo].[ProductionOrderDetails]
AFTER UPDATE
AS

BEGIN

   UPDATE dbo.ProductionOrderDetails
    SET UpdateTS = CURRENT_TIMESTAMP
    From  dbo.ProductionOrderDetails myAlias
    WHERE exists ( select null from inserted ProductionOrderDetails where myAlias.Id = ProductionOrderDetails.id)

END
GO
ALTER TABLE [dbo].[ProductionOrderDetails] ENABLE TRIGGER [ProductionOrderDetails_UPDATE_INSERT]
GO
/****** Object:  Trigger [dbo].[insNumber]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[insNumber]
   ON  [dbo].[ProductionOrders]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --declare @lastOfYear int

	--select @lastOfYear = max(cast(right(po.number,5) as int)) from ProductionOrders po inner join inserted i on po.YearStartDate  = i.yearstartdate

	update po  set po.number = dbo.GetProductionOrderNumber_MultiCompany2(i.id, i.startdate, i.id_company) from ProductionOrders po inner join inserted i on po.id=i.id

END
GO
ALTER TABLE [dbo].[ProductionOrders] ENABLE TRIGGER [insNumber]
GO
/****** Object:  Trigger [dbo].[insQtNumber]    Script Date: 07/04/2026 11:38:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[insQtNumber]
   ON  [dbo].[Quotations]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --declare @lastOfYear int

	--select @lastOfYear = max(cast(right(po.number,5) as int)) from ProductionOrders po inner join inserted i on po.YearStartDate  = i.yearstartdate

	update qt  set qt.number = dbo.GetQuotationNumber_MultiCompany2(i.id, i.Date, i.id_company) from Quotations qt inner join inserted i on qt.id=i.id

END
GO
ALTER TABLE [dbo].[Quotations] ENABLE TRIGGER [insQtNumber]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 301
               Right = 231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CustomersMarkUps"
            Begin Extent = 
               Top = 6
               Left = 269
               Bottom = 102
               Right = 460
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Markup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Markup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[42] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ITP"
            Begin Extent = 
               Top = 197
               Left = 710
               Bottom = 301
               Right = 893
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PO"
            Begin Extent = 
               Top = 6
               Left = 480
               Bottom = 125
               Right = 663
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "POD"
            Begin Extent = 
               Top = 6
               Left = 701
               Bottom = 419
               Right = 891
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "TP"
            Begin Extent = 
               Top = 152
               Left = 309
               Bottom = 256
               Right = 492
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "UM"
            Begin Extent = 
               Top = 318
               Left = 111
               Bottom = 407
               Right = 294
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SU"
            Begin Extent = 
               Top = 209
               Left = 43
               Bottom = 328
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TMA"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 235
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_FreeTypeProductionOrderDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    End
         Begin Table = "MMA"
            Begin Extent = 
               Top = 6
               Left = 259
               Bottom = 125
               Right = 442
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 28
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 6915
         Alias = 3075
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_FreeTypeProductionOrderDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VW_FreeTypeProductionOrderDetails'
GO
