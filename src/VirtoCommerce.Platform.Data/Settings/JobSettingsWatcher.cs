using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class JobSettingsWatcher : IEventHandler<ObjectSettingChangedEvent>
    {
        private readonly Dictionary<string, List<Expression<Func<string, Expression<Func<Task>>, Task>>>> subscriptions
            = new Dictionary<string, List<Expression<Func<string, Expression<Func<Task>>, Task>>>>();

        private readonly ISettingsManager _settingsManager;

        public JobSettingsWatcher(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public Task Handle(ObjectSettingChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries.Where(x => x.EntryState == EntryState.Modified
                                              || x.EntryState == EntryState.Added))
            {
                if (subscriptions.TryGetValue(changedEntry.NewEntry.Name, out var settingSubscriptions))
                {
                    foreach (var handler in settingSubscriptions)
                    {
                        var compiledHandler = handler.Compile();
                        compiledHandler(null, null);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void WatchJobSetting(string enablerName, string cronExpressionName, string jobId, Expression<Func<Task>> methodCall)
        {
            Expression<Func<string, Expression<Func<Task>>, Task>> handler = (recurringJobId, func) => ExecuteJob(jobId, methodCall, enablerName, cronExpressionName);
            RegisterHandler(enablerName, handler);
            RegisterHandler(cronExpressionName, handler);
        }

        private void RegisterHandler(string settingName, Expression<Func<string, Expression<Func<Task>>, Task>> handler)
        {
            if (subscriptions.TryGetValue(settingName, out var settingSubscriptions))
            {
                settingSubscriptions.Add(handler);
            }
            else
            {
                subscriptions.Add(settingName, new List<Expression<Func<string, Expression<Func<Task>>, Task>>> { handler });
            }
        }

        private async Task ExecuteJob(string recurringJobId, Expression<Func<Task>> methodCall, string enableJobName, string cronExpressionName)
        {
            var processJobEnable = await _settingsManager.GetValueAsync(enableJobName, true);
            if (processJobEnable)
            {
                RecurringJob.RemoveIfExists(recurringJobId);
                var cronExpression = _settingsManager.GetValue(cronExpressionName, string.Empty);
                RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists(recurringJobId);
            }
        }



    }

    public class MethodKey
    {
        public static string GetKey<T>(Expression<Func<T>> method, params string[] paramMembers)
        {
            var keys = new Dictionary<string, string>();
            string scope = null;
            string prefix = null;
            ParameterInfo[] formalParams = null;
            object[] actual = null;

            var methodCall = method.Body as MethodCallExpression;
            if (methodCall != null)
            {
                scope = methodCall.Method.DeclaringType.FullName;
                prefix = methodCall.Method.Name;

                IEnumerable<Expression> actualParams = methodCall.Arguments;
                actual = actualParams.Select(GetValueOfParameter<T>).ToArray();
                formalParams = methodCall.Method.GetParameters();
            }
            else
            {
                // TODO: Check if the supplied expression is something that makes sense to evaluate as a method, e.g. MemberExpression (method.Body as MemberExpression)

                var objectMember = Expression.Convert(method.Body, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                var m = getter();


                var m2 = ((System.Delegate)m);

                var delegateDeclaringType = m2.Method.DeclaringType;
                var actualMethodDeclaringType = delegateDeclaringType.DeclaringType;
                scope = actualMethodDeclaringType.FullName;
                var ar = m2.Target;
                formalParams = m2.Method.GetParameters();
                //var m = (System.MulticastDelegate)((Expression.Lambda<Func<object>>(Expression.Convert(method.Body, typeof(object)))).Compile()())

                //throw new ArgumentException("Caller is not a method", "method");
            }


            // null list of paramMembers should disregard all parameters when creating key.
            if (paramMembers != null)
            {
                for (var i = 0; i < formalParams.Length; i++)
                {
                    var par = formalParams[i];
                    // empty list of paramMembers should be treated as using all parameters 
                    if (paramMembers.Length == 0 || paramMembers.Contains(par.Name))
                    {
                        var value = actual[i];
                        keys.Add(par.Name, value.ToString());
                    }
                }

                if (paramMembers.Length != 0 && keys.Count != paramMembers.Length)
                {
                    var notFound = paramMembers.Where(x => !keys.ContainsKey(x));
                    var notFoundString = string.Join(", ", notFound);

                    throw new ArgumentException("Unable to find the following parameters in supplied method: " + notFoundString, "paramMembers");
                }
            }

            return scope + "¤" + prefix + "¤" + keys.ToString();

        }


        private static object GetValueOfParameter<T>(Expression parameter)
        {
            LambdaExpression lambda = Expression.Lambda(parameter);
            var compiledExpression = lambda.Compile();
            var value = compiledExpression.DynamicInvoke();
            return value;
        }
    }
}
