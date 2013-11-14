namespace VirtoCommerce.Scheduling.LogicalCall
{
    public struct ContextName
    {
        public string Service { get; private set; }
        public string Method { get; private set; }
        public string Value { get; private set; }

        public ContextName(string userService, string method)
            : this()
        {
            Service = userService;
            Method = method;

            Value = string.Empty;
            var pointRequired = false;

            if (!string.IsNullOrEmpty(Service))
            {
                Value += Service;
                pointRequired = true;
            }
            if (!string.IsNullOrEmpty(method))
            {
                if (pointRequired)
                {
                    Value += ".";
                }
                Value += method;
            }
        }


    }
}