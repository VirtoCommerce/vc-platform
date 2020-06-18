/****** Object:  StoredProcedure [dbo].[UpdateStatistic]    Script Date: 07/31/2014 15:05:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateStatistic]
	@Key nvarchar(32),
	@Value nvarchar(64),
	@Name nvarchar(64) = NULL
AS
BEGIN
	MERGE Statistic AS target
	USING (SELECT @Key, @Value) AS source (source_key, source_value)
	ON (target.[Key] = source_key)
	WHEN MATCHED THEN
		UPDATE SET Value = source_value, LastModified = GETDATE()
	WHEN NOT MATCHED THEN
		INSERT ([StatisticId],[Key],[Value],[Name],[Created],[LastModified],[Discriminator])
		VALUES (NEWID(),@Key,@Value,@Name,GETDATE(),GETDATE(),N'Statistic');
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateStatistics]    Script Date: 07/31/2014 15:05:27 ******/
CREATE PROCEDURE [dbo].[UpdateStatistics]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
    SET NOCOUNT ON;

	DECLARE @d_count int
	-- update NeedAttention orders
	SELECT @d_count = count(*) FROM OrderGroup where [Status]='OnHold'
	EXEC UpdateStatistic 'OrdersNeedAttention', @d_count
	
    -- update ProcessedToday orders
    SELECT @d_count = count(*) FROM OrderGroup where [Status]='Completed' and [LastModified] >= CAST(GETDATE() As Date) and [Discriminator] = 'Order'
	EXEC UpdateStatistic 'OrdersProcessedToday', @d_count
	
	-- update ProcessedToday ACTIVE ORDERS
    SELECT @d_count = count(*) FROM OrderGroup where [Status]='InProgress' and [Discriminator] = 'Order'
	EXEC UpdateStatistic 'OrdersActive', @d_count, 'ACTIVE ORDERS'
	
	-- update sales chart data.
	-- calculate all orders's statistics for the last 2 years.
	MERGE Statistic AS target
	USING (SELECT CONVERT(NVARCHAR(7), Created, 102), Count(*)
			FROM OrderGroup
			WHERE [Status] IS NOT NULL AND [Status]!= 'Cancelled' AND
					[Created] > DATEADD(year,-2,DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())+1, 0)) AND
					[Discriminator] = 'Order'
			GROUP BY CONVERT(NVARCHAR(7), Created, 102)) AS source (source_key, source_value)
	ON (target.[Key] = 'order.sales.chart.'+source.source_key)
	-- update only current month
	WHEN MATCHED AND source_key = CONVERT(NVARCHAR(7), GETDATE(), 102) THEN
		UPDATE SET Value = source_value, LastModified = GETDATE()
	-- delete non-existing sales statistics. It's probably older than 2 years.
	WHEN NOT MATCHED BY SOURCE AND target.[Key] LIKE 'order.sales.chart.%' THEN
		DELETE
	WHEN NOT MATCHED THEN
		INSERT ([StatisticId],[Key],[Value],[Name],[Created],[LastModified],[Discriminator])
		VALUES (NEWID(),'order.sales.chart.'+source_key,source_value,DATENAME(MM,source_key+'.01'),GETDATE(),GETDATE(),N'Statistic');
END
GO

DELETE FROM Statistic;

INSERT INTO [SystemJob] ([SystemJobId],[Name],[Description],[JobClassType],[JobAssemblyPath],[LoadFromGac],[IsEnabled],[Schedule],[Priority],[Period],[LastExecuted],[NextExecute],[AllowMultipleInstances],[LastModified],[Created],[Discriminator]) VALUES (N'fd005089-dd1b-4748-a661-77fd60c84b11',N'Update statistics',N'Update statistics displayed in dashboard', N'VirtoCommerce.Scheduling.Jobs.UpdateStatisticsWork,VirtoCommerce.Scheduling.Jobs',NULL,0,1,NULL,1,300,NULL,NULL,0,N'20140729 06:28:30.027',N'20140729 06:28:30.027',N'SystemJob');
