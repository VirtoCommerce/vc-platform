using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core
{
    public static class PlatformConstants
    {
        public static class Security
        {
            public static class GrantTypes
            {
                public const string Impersonate = "impersonate";
                public const string ExternalSignIn = "external_sign_in";
            }

            public static class Claims
            {
                public const string PermissionClaimType = "permission";
                public const char PermissionClaimTypeDelimiter = ';';
                public const string UserNameClaimType = "username";
                public const string LimitedPermissionsClaimType = "limited_permissions";
                public const string MemberIdClaimType = "memberId";

                /// <summary>
                /// Represents Operator User ID after impersonation 
                /// </summary>
                public const string OperatorUserId = "vc_operator_user_id";

                /// <summary>
                /// Represents Operator User Name after impersonation
                /// </summary>
                public const string OperatorUserName = "vc_operator_name";
            }

            public static class SystemRoles
            {
                public const string Customer = "__customer";
                public const string Manager = "__manager";
                public const string Administrator = "__administrator";
            }

            public static class Permissions
            {
                public const string ResetCache = "cache:reset";

                public const string AssetAccess = "platform:asset:access";
                public const string AssetDelete = "platform:asset:delete";
                public const string AssetUpdate = "platform:asset:update";
                public const string AssetCreate = "platform:asset:create";
                public const string AssetRead = "platform:asset:read";

                public const string ModuleQuery = "platform:module:read";
                public const string ModuleAccess = "platform:module:access";
                public const string ModuleManage = "platform:module:manage";

                public const string SettingQuery = "platform:setting:read";
                public const string SettingAccess = "platform:setting:access";
                public const string SettingUpdate = "platform:setting:update";

                public const string DynamicPropertiesQuery = "platform:dynamic_properties:read";
                public const string DynamicPropertiesCreate = "platform:dynamic_properties:create";
                public const string DynamicPropertiesAccess = "platform:dynamic_properties:access";
                public const string DynamicPropertiesUpdate = "platform:dynamic_properties:update";
                public const string DynamicPropertiesDelete = "platform:dynamic_properties:delete";

                public const string SecurityQuery = "platform:security:read";
                public const string SecurityCreate = "platform:security:create";
                public const string SecurityAccess = "platform:security:access";
                public const string SecurityUpdate = "platform:security:update";
                public const string SecurityDelete = "platform:security:delete";
                public const string SecurityVerifyEmail = "platform:security:verifyEmail";
                public const string SecurityLoginOnBehalf = "platform:security:loginOnBehalf";
                public const string SecurityConfirmEmail = "platform:security:confirmEmail";
                public const string SecurityGenerateToken = "platform:security:generateToken";
                public const string SecurityVerifyToken = "platform:security:verifyToken";

                public const string BackgroundJobsManage = "background_jobs:manage";

                public const string PlatformExportImportAccess = "platform:exportImport:access";
                public const string PlatformImport = "platform:import";
                public const string PlatformExport = "platform:export";

                public static string[] AllPermissions { get; } = new[] { ResetCache, AssetAccess, AssetDelete, AssetUpdate, AssetCreate, AssetRead, ModuleQuery, ModuleAccess, ModuleManage,
                                              SettingQuery, SettingAccess, SettingUpdate, DynamicPropertiesQuery, DynamicPropertiesCreate, DynamicPropertiesAccess, DynamicPropertiesUpdate, DynamicPropertiesDelete,
                                              SecurityQuery, SecurityCreate, SecurityAccess,  SecurityUpdate,  SecurityDelete, BackgroundJobsManage, PlatformExportImportAccess, PlatformImport, PlatformExport, SecurityLoginOnBehalf ,
                                              SecurityVerifyEmail, SecurityConfirmEmail, SecurityGenerateToken, SecurityVerifyToken,
                };
            }

            public static class Changes
            {
                public const string UserUpdated = "UserUpdated";
                public const string UserPasswordChanged = "UserPasswordChanged";
                public const string RoleAdded = "RoleAdded";
                public const string RoleRemoved = "RoleRemoved";
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor Languages { get; } = new()
                {
                    Name = "VirtoCommerce.Core.General.Languages",
                    GroupName = "Platform|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "en-US",
                    IsDictionary = true,
                    AllowedValues = new object[] { "en-US", "fr-FR", "de-DE", "ja-JP" },
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return Languages;
                    }
                }
            }

            public static class Security
            {
                public static SettingDescriptor SecurityAccountTypes { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.AccountTypes",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    IsLocalizable = true,
                    AllowedValues = Enum.GetNames(typeof(UserType)).ToArray<object>(),
                    DefaultValue = UserType.Customer.ToString(),
                };

                public static SettingDescriptor DefaultAccountType { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.DefaultAccountType",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = SecurityAccountTypes.DefaultValue,
                };

                public static SettingDescriptor AccountStatuses { get; } = new()
                {
                    Name = "VirtoCommerce.Other.AccountStatuses",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    IsLocalizable = true,
                    AllowedValues = new object[] { "New", "Approved", "Rejected", "Deleted" },
                    DefaultValue = "New",
                };

                public static SettingDescriptor DefaultAccountStatus { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.DefaultAccountStatus",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = AccountStatuses.DefaultValue,
                };

                public static SettingDescriptor DefaultExternalAccountStatus { get; } = new()
                {
                    Name = "VirtoCommerce.Platform.Security.DefaultExternalAccountStatus",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "Approved",
                };

                public static readonly SettingDescriptor EnablePruneExpiredTokensJob = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.Security.EnablePruneExpiredTokensJob",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                public static readonly SettingDescriptor CronPruneExpiredTokensJob = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.Security.CronPruneExpiredTokensJob",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "0 0 */1 * *"
                };
                public static SettingDescriptor FileExtensionsBlackList { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.Security.FileExtensionsBlackList",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    AllowedValues = new object[] {
                        ".asax",
                        ".ascx",
                        ".master",
                        ".skin",
                        ".browser",
                        ".sitemap",
                        ".config",
                        ".cs",
                        ".csproj",
                        ".vb",
                        ".vbproj",
                        ".webinfo",
                        ".licx",
                        ".resx",
                        ".resources",
                        ".mdb",
                        ".vjsproj",
                        ".java",
                        ".jsl",
                        ".ldb",
                        ".dsdgm",
                        ".ssdgm",
                        ".lsad",
                        ".ssmap",
                        ".cd",
                        ".dsprototype",
                        ".lsaprototype",
                        ".sdm",
                        ".sdmDocument",
                        ".mdf",
                        ".ldf",
                        ".ad",
                        ".dd",
                        ".ldd",
                        ".sd",
                        ".adprototype",
                        ".lddprototype",
                        ".exclude",
                        ".refresh",
                        ".compiled",
                        ".msgx",
                        ".vsdisco",
                        ".rules",
                        ".exe"
                    },
                    DefaultValue = ".asax",
                };

                public static SettingDescriptor FileExtensionsWhiteList { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.Security.FileExtensionsWhiteList",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    AllowedValues = Array.Empty<object>(),
                    DefaultValue = "_none", // fake default value to fix empty dictionary saving issue
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return SecurityAccountTypes;
                        yield return DefaultAccountType;
                        yield return AccountStatuses;
                        yield return DefaultAccountStatus;
                        yield return DefaultExternalAccountStatus;
                        yield return EnablePruneExpiredTokensJob;
                        yield return CronPruneExpiredTokensJob;
                        yield return FileExtensionsBlackList;
                        yield return FileExtensionsWhiteList;
                    }
                }
            }

            public static class Setup
            {
                public static SettingDescriptor SetupStep { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.SetupStep",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = null
                };

                public static SettingDescriptor SampleDataState { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.SampleDataState",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = ExportImport.SampleDataState.Undefined
                };

                public static SettingDescriptor ModulesAutoInstallState { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.ModulesAutoInstallState",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = AutoInstallState.Undefined
                };

                public static SettingDescriptor ModulesAutoInstalled { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.ModulesAutoInstalled",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                public static SettingDescriptor SendDiagnosticData { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.SendDiagnosticData",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                /// <summary>
                /// This setting controlled from LicenseController.
                /// </summary>
                public static SettingDescriptor TrialExpirationDate { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.TrialExpirationDate",
                    GroupName = "Platform|Setup",
                    ValueType = SettingValueType.DateTime,
                    IsHidden = true,
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return SetupStep;
                        yield return SampleDataState;
                        yield return ModulesAutoInstallState;
                        yield return ModulesAutoInstalled;
                        yield return SendDiagnosticData;
                        yield return TrialExpirationDate;
                    }
                }
            }

            public static class UserProfile
            {
                public static SettingDescriptor MainMenuState { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.MainMenu.State",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.Json,
                };

                public static SettingDescriptor Language { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.Language",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "en"
                };

                public static SettingDescriptor RegionalFormat { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.RegionalFormat",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "en"
                };

                public static SettingDescriptor TimeZone { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.TimeZone",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.ShortText
                };

                public static SettingDescriptor UseTimeAgo { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.UseTimeAgo",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                public static SettingDescriptor FullDateThreshold { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.FullDateThreshold",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.Integer,
                };

                public static SettingDescriptor FullDateThresholdUnit { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.FullDateThresholdUnit",
                    ValueType = SettingValueType.ShortText,
                    GroupName = "Platform|User Profile",
                    DefaultValue = "Never",
                    AllowedValues = new object[]
                                {
                                    "Never",
                                    "Seconds",
                                    "Minutes",
                                    "Hours",
                                    "Days",
                                    "Weeks",
                                    "Months",
                                    "Quarters",
                                    "Years"
                                }
                };

                public static SettingDescriptor ShowMeridian { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.ShowMeridian",
                    GroupName = "Platform|User Profile",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return MainMenuState;
                        yield return Language;
                        yield return RegionalFormat;
                        yield return TimeZone;
                        yield return UseTimeAgo;
                        yield return FullDateThreshold;
                        yield return FullDateThresholdUnit;
                        yield return ShowMeridian;
                    }
                }
            }

            public static class UserInterface
            {
                public static SettingDescriptor Customization { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.UI.Customization",
                    GroupName = "Platform|User Interface",
                    ValueType = SettingValueType.Json,
                    DefaultValue = "{\n" +
                                               "  \"title\": \"Virto Commerce\",\n" +
                                               "  \"logo\": \"/images/logo.png\",\n" +
                                               "  \"contrast_logo\": \"/images/logo-small.svg\"\n" +
                                               "}"
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return Customization;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings =>
                General.AllGeneralSettings
                .Concat(Security.AllSettings)
                .Concat(Setup.AllSettings)
                .Concat(UserProfile.AllSettings)
                .Concat(UserInterface.AllSettings);
        }
    }
}
