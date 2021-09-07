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
            public static class Claims
            {
                public const string PermissionClaimType = "permission";
                public const char PermissionClaimTypeDelimiter = ';';
                public const string UserNameClaimType = "username";
                public const string LimitedPermissionsClaimType = "limited_permissions";
                public const string MemberIdClaimType = "memberId";
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

                public const string AssetAccess = "platform:asset:access",
                                    AssetDelete = "platform:asset:delete",
                                    AssetUpdate = "platform:asset:update",
                                    AssetCreate = "platform:asset:create",
                                    AssetRead = "platform:asset:read";

                public const string ModuleQuery = "platform:module:read",
                                    ModuleAccess = "platform:module:access",
                                    ModuleManage = "platform:module:manage";

                public const string SettingQuery = "platform:setting:read",
                                    SettingAccess = "platform:setting:access",
                                    SettingUpdate = "platform:setting:update";

                public const string DynamicPropertiesQuery = "platform:dynamic_properties:read",
                                    DynamicPropertiesCreate = "platform:dynamic_properties:create",
                                    DynamicPropertiesAccess = "platform:dynamic_properties:access",
                                    DynamicPropertiesUpdate = "platform:dynamic_properties:update",
                                    DynamicPropertiesDelete = "platform:dynamic_properties:delete";

                public const string SecurityQuery = "platform:security:read",
                                    SecurityCreate = "platform:security:create",
                                    SecurityAccess = "platform:security:access",
                                    SecurityUpdate = "platform:security:update",
                                    SecurityDelete = "platform:security:delete",
                                    SecurityVerifyEmail = "platform:security:verifyEmail",
                                    SecurityLoginOnBehalf = "platform:security:loginOnBehalf";

                public const string BackgroundJobsManage = "background_jobs:manage";

                public const string PlatformExportImportAccess = "platform:exportImport:access",
                                    PlatformImport = "platform:import",
                                    PlatformExport = "platform:export";

                public static string[] AllPermissions { get; } = new[] { ResetCache, AssetAccess, AssetDelete, AssetUpdate, AssetCreate, AssetRead, ModuleQuery, ModuleAccess, ModuleManage,
                                              SettingQuery, SettingAccess, SettingUpdate, DynamicPropertiesQuery, DynamicPropertiesCreate, DynamicPropertiesAccess, DynamicPropertiesUpdate, DynamicPropertiesDelete,
                                              SecurityQuery, SecurityCreate, SecurityAccess,  SecurityUpdate,  SecurityDelete, BackgroundJobsManage, PlatformExportImportAccess, PlatformImport, PlatformExport, SecurityLoginOnBehalf ,
                                              SecurityVerifyEmail
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
            public static class Security
            {
                public static SettingDescriptor SecurityAccountTypes { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Platform.Security.AccountTypes",
                    GroupName = "Platform|Security",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    AllowedValues = Enum.GetNames(typeof(UserType)),
                    DefaultValue = UserType.Manager
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
                    AllowedValues = new string[] {
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
                        ".rules"
                    },
                    DefaultValue = ".asax",
                };


                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return SecurityAccountTypes;
                        yield return EnablePruneExpiredTokensJob;
                        yield return CronPruneExpiredTokensJob;
                        yield return FileExtensionsBlackList;
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
                    AllowedValues = new[]
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
                                               "  \"contrast_logo\": \"/images/contrast-logo.png\"\n" +
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

            public static class Other
            {
                public static SettingDescriptor AccountStatuses { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerce.Other.AccountStatuses",
                    GroupName = "Platform|Other",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "New",
                    IsDictionary = true,
                    AllowedValues = new[] { "New", "Approved", "Rejected", "Deleted" }
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return AccountStatuses;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings => Security.AllSettings
                .Concat(Setup.AllSettings)
                .Concat(UserProfile.AllSettings)
                .Concat(UserInterface.AllSettings)
                .Concat(Other.AllSettings);
        }
    }
}
