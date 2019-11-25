using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Security.Migrations
{
    public partial class UpdateSecurityV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory'))
                IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'VirtoCommerce.Foundation.Data.Security.Identity.SecurityDbContext'))
                    BEGIN

	                    BEGIN
		                    ALTER TABLE [AspNetRoles] ADD [NormalizedName] nvarchar(256);
		                    ALTER TABLE [AspNetRoles] ADD [ConcurrencyStamp] nvarchar(max);
		                    ALTER TABLE [AspNetRoles] ADD [Description] nvarchar(max);
		                    EXEC ('DROP INDEX AspNetRoles.RoleNameIndex')
		                    EXEC ('CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName]) WHERE ([NormalizedName] IS NOT NULL)')
	                    END
	                    
	                    BEGIN
		                    ALTER TABLE [AspNetUserLogins] ADD [ProviderDisplayNames] nvarchar(max);
		                    ALTER TABLE [AspNetUsers] ADD [NormalizedUserName] nvarchar(256);
		                    ALTER TABLE [AspNetUsers] ADD [NormalizedEmail] nvarchar(256);
		                    ALTER TABLE [AspNetUsers] ADD [ConcurrencyStamp] nvarchar(max);
		                    ALTER TABLE [AspNetUsers] ADD [LockoutEnd] datetimeoffset;
		                    ALTER TABLE [AspNetUsers] ADD [StoreId] nvarchar(128);
		                    ALTER TABLE [AspNetUsers] ADD [MemberId] nvarchar(128);
		                    ALTER TABLE [AspNetUsers] ADD [IsAdministrator] BIT NOT NULL Default(0);
		                    ALTER TABLE [AspNetUsers] ADD [PhotoUrl] nvarchar(2048);
		                    ALTER TABLE [AspNetUsers] ADD [UserType] nvarchar(64);
		                    ALTER TABLE [AspNetUsers] ADD [PasswordExpired] bit NOT NULL Default(0);

                            EXEC ('UPDATE [dbo].[AspNetUsers] SET StoreId = pa.StoreId, MemberId = pa.MemberId, IsAdministrator = pa.IsAdministrator, UserType = pa.UserType, PasswordExpired = pa.PasswordExpired FROM PlatformAccount pa WHERE [dbo].[AspNetUsers].Id = pa.Id')
	                    END

	                    BEGIN
		                    CREATE TABLE [dbo].[AspNetRoleClaims](
			                    [Id] [int] IDENTITY(1,1) NOT NULL,
			                    [RoleId] [nvarchar](128) NOT NULL,
			                    [ClaimType] [nvarchar](max) NULL,
			                    [ClaimValue] [nvarchar](max) NULL,
		                     CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
		                    (
			                    [Id] ASC
		                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	                    END

	                    BEGIN
		                    ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
			                    REFERENCES [dbo].[AspNetRoles] ([Id])
			                    ON DELETE CASCADE
		                    ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]

		                    CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId])
	                    END

                        BEGIN
                            CREATE TABLE [dbo].[AspNetUserTokens](
	                            [UserId] [nvarchar](128) NOT NULL,
	                            [LoginProvider] [nvarchar](450) NOT NULL,
	                            [Name] [nvarchar](450) NOT NULL,
	                            [Value] [nvarchar](max) NULL,
                             CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
                            (
	                            [UserId] ASC,
	                            [LoginProvider] ASC,
	                            [Name] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                            EXEC (N'ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
                                REFERENCES [dbo].[AspNetUsers] ([Id])
                                ON DELETE CASCADE')

                            EXEC (N'ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]')
                        END

	                    BEGIN
		                    CREATE TABLE [dbo].[OpenIddictApplications](
			                    [ClientId] [nvarchar](450) NOT NULL,
			                    [ClientSecret] [nvarchar](max) NULL,
			                    [ConcurrencyToken] [nvarchar](max) NULL,
			                    [ConsentType] [nvarchar](max) NULL,
			                    [DisplayName] [nvarchar](max) NULL,
			                    [Id] [nvarchar](450) NOT NULL,
			                    [Permissions] [nvarchar](max) NULL,
			                    [PostLogoutRedirectUris] [nvarchar](max) NULL,
			                    [Properties] [nvarchar](max) NULL,
			                    [RedirectUris] [nvarchar](max) NULL,
			                    [Type] [nvarchar](max) NOT NULL,
		                     CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY CLUSTERED 
		                    (
			                    [Id] ASC
		                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	                    
		                    CREATE TABLE [dbo].[OpenIddictAuthorizations](
			                    [ApplicationId] [nvarchar](450) NULL,
			                    [ConcurrencyToken] [nvarchar](max) NULL,
			                    [Id] [nvarchar](450) NOT NULL,
			                    [Properties] [nvarchar](max) NULL,
			                    [Scopes] [nvarchar](max) NULL,
			                    [Status] [nvarchar](max) NOT NULL,
			                    [Subject] [nvarchar](max) NOT NULL,
			                    [Type] [nvarchar](max) NOT NULL,
		                     CONSTRAINT [PK_OpenIddictAuthorizations] PRIMARY KEY CLUSTERED 
		                    (
			                    [Id] ASC
		                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		                    ALTER TABLE [dbo].[OpenIddictAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId] FOREIGN KEY([ApplicationId])
		                    REFERENCES [dbo].[OpenIddictApplications] ([Id])

		                    ALTER TABLE [dbo].[OpenIddictAuthorizations] CHECK CONSTRAINT [FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId]

		                    CREATE TABLE [dbo].[OpenIddictScopes](
			                    [ConcurrencyToken] [nvarchar](max) NULL,
			                    [Description] [nvarchar](max) NULL,
			                    [DisplayName] [nvarchar](max) NULL,
			                    [Id] [nvarchar](450) NOT NULL,
			                    [Name] [nvarchar](450) NOT NULL,
			                    [Properties] [nvarchar](max) NULL,
			                    [Resources] [nvarchar](max) NULL,
		                     CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY CLUSTERED 
		                    (
			                    [Id] ASC
		                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		                    CREATE TABLE [dbo].[OpenIddictTokens](
			                    [ApplicationId] [nvarchar](450) NULL,
			                    [AuthorizationId] [nvarchar](450) NULL,
			                    [CreationDate] [datetimeoffset](7) NULL,
			                    [ExpirationDate] [datetimeoffset](7) NULL,
			                    [ConcurrencyToken] [nvarchar](max) NULL,
			                    [Id] [nvarchar](450) NOT NULL,
			                    [Payload] [nvarchar](max) NULL,
			                    [Properties] [nvarchar](max) NULL,
			                    [ReferenceId] [nvarchar](450) NULL,
			                    [Status] [nvarchar](max) NULL,
			                    [Subject] [nvarchar](max) NOT NULL,
			                    [Type] [nvarchar](max) NOT NULL,
		                     CONSTRAINT [PK_OpenIddictTokens] PRIMARY KEY CLUSTERED 
		                    (
			                    [Id] ASC
		                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		                    ALTER TABLE [dbo].[OpenIddictTokens]  WITH CHECK ADD  CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId] FOREIGN KEY([ApplicationId])
		                    REFERENCES [dbo].[OpenIddictApplications] ([Id])

		                    ALTER TABLE [dbo].[OpenIddictTokens] CHECK CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId]

		                    ALTER TABLE [dbo].[OpenIddictTokens]  WITH CHECK ADD  CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY([AuthorizationId])
		                    REFERENCES [dbo].[OpenIddictAuthorizations] ([Id])

		                    ALTER TABLE [dbo].[OpenIddictTokens] CHECK CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId]
	                    END

	                    BEGIN
		                    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES ('20180411094757_InitialSecurity', '2.2.3-servicing-35854')
	                    END

	                    BEGIN
		                    EXEC (N'UPDATE [AspNetUsers] SET NormalizedUserName = UPPER([UserName]), [NormalizedEmail] = UPPER([Email])')
		                    EXEC (N'INSERT INTO [AspNetRoles] ([Id],[Name],[NormalizedName],[Description]) SELECT Id, [Name], UPPER([NAME]), [Description] FROM PlatformRole')
                            EXEC (N'INSERT INTO [dbo].[AspNetUserRoles] ([UserId],[RoleId]) SELECT AccountId, RoleId FROM PlatformRoleAssignment')
                            EXEC (N'INSERT INTO [dbo].[AspNetRoleClaims] ([RoleId],[ClaimType],[ClaimValue]) SELECT [RoleId], ''permission'', [PermissionId] FROM [PlatformRolePermission]')
	                    END

                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
