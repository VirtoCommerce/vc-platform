using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class MigrationName
    {
        public static string GetUpdateV2MigrationName(string moduleName)
        {
            return $"20000000000000_Update{moduleName.Replace(".", "").Replace("VirtoCommerce", "")}V2";
        }

        [Obsolete("use GetUpdateV2MigrationName")]
        public static string GetUpdateV2MigrationNameByOwnerName(string moduleName, string ownerName)
        {
            return GetUpdateV2MigrationName(moduleName);
        }
    }
}
