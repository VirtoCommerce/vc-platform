using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.PlatformTools;

namespace VirtoCommerce.Scheduling.Windows
{
    // rp: by the Alex's idea the web app's background thread should be the general method to launch
    // the job's scheduler, when the  windows service is "advanced"
    // What you should know: 
    // 1) IIS web-gardens are forbidden
    // 2) unhandled exception ruin process
    // 3) IIS tries to restart  AppDomain each 29 hours
    // 4) web.config modification restarts AppDomain 
    // 5) AppDomain can tear down after some period of inactivity
    public class SchedulerHost : IRegisteredObject
    {
        public enum StartUpOption { Primary, Secondary, None }
        // windows service platform scheduler is reused there
        private readonly JobScheduler _jobScheduler;
        private Thread _thread;
        private readonly ILogger _traceSource;
        private SchedulerHost(bool isPrimary, Func<Type, object> containerResolve, Func<IAppConfigRepository> repositoryResolve)
        {
            _traceSource = new VirtoCommerceTraceSource("VirtoCommerce.ScheduleService.Trace");

            Trace.TraceInformation("SchedulerHost constructor started");
            try
            {
                _jobScheduler = new JobScheduler(isPrimary,
                    t =>
                    {
                        object o;
                        try
                        {
                            o = containerResolve(t);
                        }
                        catch (Exception ex)
                        {
                            _traceSource.Error(ex.ToString());
                            throw;
                        }
                        return (IJobActivity)o;
                    },
                    repositoryResolve, _traceSource
                    ); // reuse host container
            }
            catch (Exception ex)
            {
                _traceSource.Error(ex.ToString());
                throw;
            }

            _traceSource.Info("SchedulerHost constructor finished");
        }

        public void Start()
        {
            _traceSource.Info("SchedulerHost.Start started");
            try
            {
                _thread = new Thread(_jobScheduler.Start);
                _thread.Start();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            _traceSource.Info("SchedulerHost.Start finished");
        }

        public void Stop(bool immediate = false) //...may be it is better to ignore immediate calls
        {
            _traceSource.Info("SchedulerHost constructor started");
            try
            {
                if (_thread != null && _thread.IsAlive)
                {
                    if (!immediate)
                    {
                        _jobScheduler.Stop();
                    }
                    else
                    {
                        _thread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                _traceSource.Error(ex.ToString());
            }
            finally
            {
                try
                {
                    HostingEnvironment.UnregisterObject(this);
                    _traceSource.Info("SchedulerHost successfully unregistered!");
                }
                catch (Exception ex)
                {
                    _traceSource.Error("Error caught on unregistering SchedulerHost!/n" + ex);
                }

            }
            _traceSource.Info("SchedulerHost constructor finished");
        }

        public static void CreateScheduler(IUnityContainer container, string schedulerHostStartUpOptionText)
        {
            if (string.IsNullOrEmpty(schedulerHostStartUpOptionText))
            {
                Trace.TraceError("SchedulerHost parameter not found in Web.Config. Scheduler not started");
            }
            else
            {
                StartUpOption startUpOption;
                try
                {
                    startUpOption =
                        (StartUpOption)
                        Enum.Parse(typeof(StartUpOption), schedulerHostStartUpOptionText);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(
                        "Can't parse SchedulerHost parameter from web.config. Scheduler host not started. Expected values: Primary, Secondary, None." +
                        Environment.NewLine + ex);
                    startUpOption = StartUpOption.None;
                }
                if (startUpOption != StartUpOption.None)
                {
                    var schedulerHost = new SchedulerHost(startUpOption == StartUpOption.Primary,
                                                          t => container.Resolve(t, null),
                                                          () =>
                                                          (IAppConfigRepository)container.Resolve(typeof(IAppConfigRepository), null)
                        );
                    HostingEnvironment.RegisterObject(schedulerHost);
                    schedulerHost.Start();
                }
            }
        }
    }
}