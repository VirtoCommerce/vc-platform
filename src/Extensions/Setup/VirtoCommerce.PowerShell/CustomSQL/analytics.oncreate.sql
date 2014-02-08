/**
	ANALYTICS SCRIPT

	- this script can be ran on custom build to add analytics to the database
	- modify analytics fields below to use it in your build process
	- script is includes "oncreate" in naming convention, so it will run only when new database is created
*/

declare @fieldid nvarchar(128)
select @fieldid = SettingId from [Setting] where Name = 'AnalyticsField'
INSERT INTO [dbo].[SettingValue]
           ([SettingValueId]
           ,[ValueType]
           ,[ShortTextValue]
           ,[LongTextValue]
           ,[DecimalValue]
           ,[IntegerValue]
           ,[BooleanValue]
           ,[DateTimeValue]
           ,[Locale]
           ,[SettingId]
           ,[LastModified]
           ,[Created])
     VALUES
           (
				NEWID(),
				'LongText',
				NULL,N'<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push([''_setAccount'', ''UA-490257-44'']);
  _gaq.push([''_setDomainName'', ''virtocommerce.com'']);
  _gaq.push([''_trackPageview'']);

  (function() {
    var ga = document.createElement(''script''); ga.type = ''text/javascript''; ga.async = true;
    ga.src = (''https:'' == document.location.protocol ? ''https://ssl'' : ''http://www'') + ''.google-analytics.com/ga.js'';
    var s = document.getElementsByTagName(''script'')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>',
			0.00,0,0,NULL,NULL,@fieldid,getdate(),getdate()
					  )
GO