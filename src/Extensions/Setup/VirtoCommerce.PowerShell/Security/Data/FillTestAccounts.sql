INSERT INTO [AspNetUsers] ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEndDateUtc],[LockoutEnabled],[AccessFailedCount],[UserName]) VALUES (N'2',NULL,1,N'AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==',N'',NULL,0,0,NULL,1,0,N'test');
GO
INSERT INTO [Account] ([AccountId],[StoreId],[MemberId],[UserName],[RegisterType],[AccountState],[LastModified],[Created],[Discriminator]) VALUES (N'2',N'SampleStore',N'2',N'test',2,1,NULL,NULL,'Account');
GO