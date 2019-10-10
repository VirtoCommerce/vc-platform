namespace VirtoCommerce.Platform.Core.Common
{
    public static class MigrationName
    {
        public static string GetUpdateV2MigrationName(string moduleName)
        {
            return GetUpdateV2MigrationNameByOwnerName(moduleName, "VirtoCommerce");
        }

        public static string GetUpdateV2MigrationNameByOwnerName(string moduleName, string ownerName)
        {
            return $"20000000000000_Update{moduleName.Replace($"{ownerName}.", "")}V2";
        }
    }
}
