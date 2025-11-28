namespace ExtendedServiceProvider
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
        public object? ServiceKey { get; init; }

        public override bool Equals(object? obj) => Match(obj);

        public override bool Match(object? obj)
        {
            if (obj is InjectAttribute attribute)
            {
                return object.Equals(ServiceKey, attribute.ServiceKey);
            }
            return false;
        }

        public override int GetHashCode() => ServiceKey?.GetHashCode() ?? 0;

        public override bool IsDefaultAttribute() => ServiceKey == null;
    }
}
