# Prepare separated databases for VC v3

This article is relevant to you if you are:

* migrating VirtoCommerce (VC) from v2 to v3
* multiple databases (DB) are used by the solution (some of the VC modules use own, separate DB).

## Prepare SeoUrlKeyword table

1. Make DB backups
1. Identify the modules which use **SeoUrl** functionality and use different DB than VC Platform. E.g., Catalog, Customer, Store modules.
1. Run the script in each of the previously identified module DB:

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
    
    !!! note
        If `SeoUrlKeyword` table already exists, need to drop it first and then run the above script.

1. Copy data from the main DB to each newly created table:

    ```sql
    INSERT INTO [SeoUrlKeyword]
    	([Id], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [Keyword], [StoreId], [IsActive], [Language], [Title], [MetaDescription], [MetaKeywords], [ImageAltDescription], [ObjectId], [ObjectType])
    SELECT [Id], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [Keyword], [StoreId], [IsActive], [Language], [Title],   [MetaDescription], [MetaKeywords], [ImageAltDescription], [ObjectId], [ObjectType]
    FROM [<vc-v2-main>].[dbo].[SeoUrlKeyword] 
    ```
	
	!!! note
    	Change the DB name from **`<vc-v2-main>`** to the actual main source DB.

## Prepare PlatformDynamicPropertyObjectValue table

1. Make DB backups
1. Identify the modules where entities support **DynamicProperty** and use different DB than VC Platform. E.g., Customer, Cart, Order, Store modules.
1. need to update PlatformDynamicProperty in MAIN DataBase
    ```sql
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CartModule.Core.Model.LineItem'     WHERE ObjectType = 'VirtoCommerce.Domain.Cart.Model.LineItem'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CartModule.Core.Model.Payment'      WHERE ObjectType = 'VirtoCommerce.Domain.Cart.Model.Payment'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CartModule.Core.Model.Shipment'     WHERE ObjectType = 'VirtoCommerce.Domain.Cart.Model.Shipment'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CartModule.Core.Model.ShoppingCart' WHERE ObjectType = 'VirtoCommerce.Domain.Cart.Model.ShoppingCart'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CustomerModule.Core.Model.Contact' WHERE ObjectType = 'VirtoCommerce.Domain.Customer.Model.Contact'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CustomerModule.Core.Model.Organization' WHERE ObjectType = 'VirtoCommerce.Domain.Customer.Model.Organization'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CustomerModule.Core.Model.Employee' WHERE ObjectType = 'VirtoCommerce.Domain.Customer.Model.Employee'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.CustomerModule.Core.Model.Vendor' WHERE ObjectType = 'VirtoCommerce.Domain.Customer.Model.Vendor'               UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.OrderModule.Core.Model.CustomerOrder'  WHERE ObjectType = 'VirtoCommerce.Domain.Order.Model.CustomerOrder'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.OrderModule.Core.Model.LineItem'       WHERE ObjectType = 'VirtoCommerce.Domain.Order.Model.LineItem'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.OrderModule.Core.Model.PaymentIn'      WHERE ObjectType = 'VirtoCommerce.Domain.Order.Model.PaymentIn'
    UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.OrderModule.Core.Model.Shipment'       WHERE ObjectType = 'VirtoCommerce.Domain.Order.Model.Shipment'           UPDATE [PlatformDynamicProperty] SET ObjectType = 'VirtoCommerce.StoreModule.Core.Model.Store' WHERE ObjectType = 'VirtoCommerce.Domain.Store.Model.Store'
    ```
1. Run the script in each of the previously identified module DB:
    
    ```sql
    CREATE TABLE [dbo].[PlatformDynamicProperty](
        [Id] [nvarchar](64) NOT NULL,
        [ObjectType] [nvarchar](256) NULL,
        [Name] [nvarchar](256) NULL,
        [ValueType] [nvarchar](64) NOT NULL,
        [IsArray] [bit] NOT NULL,
        [IsDictionary] [bit] NOT NULL,
        [IsMultilingual] [bit] NOT NULL,
        [IsRequired] [bit] NOT NULL,
        [CreatedDate] [datetime] NOT NULL,
        [ModifiedDate] [datetime] NULL,
        [CreatedBy] [nvarchar](64) NULL,
        [ModifiedBy] [nvarchar](64) NULL,
        [Description] [nvarchar](256) NULL,
        [DisplayOrder] [int] NULL,
     CONSTRAINT [PK_dbo.PlatformDynamicProperty] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    
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
	
	!!! note
        If `PlatformDynamicPropertyObjectValue` table already exists, need to drop it first and then run the above script.
    
2. Copy data from the main DB to each newly created table:

    ```sql
    INSERT INTO [dbo].[PlatformDynamicPropertyObjectValue]
               ([Id],[ObjectType],[ObjectId],[Locale],[ValueType],[ShortTextValue],[LongTextValue],[DecimalValue],[IntegerValue],[BooleanValue],[DateTimeValue],[PropertyId],[DictionaryItemId],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy])
    SELECT 
        [Id],[ObjectType],[ObjectId],[Locale],[ValueType],[ShortTextValue],[LongTextValue],[DecimalValue],[IntegerValue],[BooleanValue]
        ,[DateTimeValue],[PropertyId],[DictionaryItemId],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy]
    FROM [<vc-v2-main>].[dbo].[PlatformDynamicPropertyObjectValue]
    ```
    
	!!! note
    	Change the DB name from **`<vc-v2-main>`** to the actual main source DB.
