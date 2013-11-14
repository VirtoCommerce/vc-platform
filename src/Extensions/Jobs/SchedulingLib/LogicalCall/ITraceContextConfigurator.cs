namespace VirtoCommerce.Scheduling.LogicalCall
{
    public interface ITraceContextConfigurator
    {
        TraceContextConfiguration GetDefault();
        TraceContextConfiguration GetDefault(string service, string method);
    }
}