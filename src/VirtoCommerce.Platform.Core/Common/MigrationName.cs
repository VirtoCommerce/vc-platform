namespace VirtoCommerce.Platform.Core.Common
{
    public static class MigrationName
    {
        /// <summary>
        /// It's important thing of naming for correct migration to v.3
        /// The migration from v.2 should be applied first
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns>20000000000000_UpdateModuleNameV2</returns>
        public static string GetUpdateV2MigrationName(string moduleName)
        {
            return $"20000000000000_Update{moduleName.Replace(".", "").Replace("VirtoCommerce", "")}V2";
        }
    }
}
