namespace VirtoCommerce.Platform.Core.Security
{
    public class Permission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Flag used to define that permission has a bounded scope (used only within role)
        /// </summary>
        public bool ScopeBounded { get; set; }
        /// <summary>
        /// Id of the module which has registered this permission.
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// Display name of the group to which this permission belongs. The '|' character is used to separate Child and parent groups.
        /// </summary>
        public string GroupName { get; set; }
    }
}
