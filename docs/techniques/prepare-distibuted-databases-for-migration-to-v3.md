# Prepare distributed databases for migration to v.3

If there are distributed databases for some modules like Customer, Catalog then need to do some actions

## Preapare SeoUrlKeyword table

1. make backup
1. need to create the table to each database of modules where use functionality SeoUrl
    - like that Catalog, Customer, Store
```sql
CREATE TABLE [dbo].[SeoUrlKeyword](
		[Id] [nvarchar](128) NOT NULL,
		[Language] [nvarchar](5) NULL,
		[Keyword] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[Title] [nvarchar](255) NULL,
		[MetaDescription] [nvarchar](1024) NULL,
		[MetaKeywords] [nvarchar](255) NULL,
		[ImageAltDescription] [nvarchar](255) NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedDate] [datetime] NULL,
		[CreatedBy] [nvarchar](64) NULL,
		[ModifiedBy] [nvarchar](64) NULL,
		[ObjectId] [nvarchar](255) NOT NULL DEFAULT (''),
		[ObjectType] [nvarchar](64) NOT NULL DEFAULT (''),
		[StoreId] [nvarchar](128) NULL,
	 CONSTRAINT [PK_dbo.SeoUrlKeyword] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
```
NOTE: _if there is the table need to remove it then create again the table_

2. Then need to copy data from main database to each database of modules

```sql
INSERT INTO [SeoUrlKeyword]
	([Id], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [Keyword], [StoreId], [IsActive], [Language], [Title], [MetaDescription], [MetaKeywords], [ImageAltDescription], [ObjectId], [ObjectType])
SELECT [Id], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [Keyword], [StoreId], [IsActive], [Language], [Title],   [MetaDescription], [MetaKeywords], [ImageAltDescription], [ObjectId], [ObjectType]
FROM [dev-main].[dbo].[SeoUrlKeyword] 
```
NOTE: _instead of **[dev-main]** change database name to your source_

## Prepare PlatformDynamicPropertyObjectValue table

1. make backup
1. need to create table PlatformDynamicPropertyObjectValue to each database of modules where  
    - like that Customer, Cart, Order, Store

```sql
CREATE TABLE [dbo].[PlatformDynamicPropertyObjectValue](
	[Id] [nvarchar](64) NOT NULL,
	[ObjectType] [nvarchar](256) NULL,
	[ObjectId] [nvarchar](128) NULL,
	[Locale] [nvarchar](64) NULL,
	[ValueType] [nvarchar](64) NOT NULL,
	[ShortTextValue] [nvarchar](512) NULL,
	[LongTextValue] [nvarchar](max) NULL,
	[DecimalValue] [decimal](18, 5) NULL,
	[IntegerValue] [int] NULL,
	[BooleanValue] [bit] NULL,
	[DateTimeValue] [datetime] NULL,
	[PropertyId] [nvarchar](64) NOT NULL,
	[DictionaryItemId] [nvarchar](64) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](64) NULL,
	[ModifiedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_dbo.PlatformDynamicPropertyObjectValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```
NOTE: _if there is the table need to remove it then create again the table_

2. Then need to copy data from main database to each database of modules

```sql
INSERT INTO [dbo].[PlatformDynamicPropertyObjectValue]
           ([Id],[ObjectType],[ObjectId],[Locale],[ValueType],[ShortTextValue],[LongTextValue],[DecimalValue],[IntegerValue],[BooleanValue],[DateTimeValue],[PropertyId],[DictionaryItemId],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy])
SELECT 
    [Id],[ObjectType],[ObjectId],[Locale],[ValueType],[ShortTextValue],[LongTextValue],[DecimalValue],[IntegerValue],[BooleanValue]
    ,[DateTimeValue],[PropertyId],[DictionaryItemId],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy]
FROM [dev-main].[dbo].[PlatformDynamicPropertyObjectValue]
```

NOTE: _instead of **[dev-main]** change database name to your source_
