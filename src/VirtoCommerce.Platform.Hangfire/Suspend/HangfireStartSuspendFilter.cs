using System;
using Hangfire.States;

namespace VirtoCommerce.Platform.Hangfire.Suspend
{
    /// <summary>
    /// This filter allows to postpone Hangfire jobs starts
    /// </summary>
    public class HangfireStartSuspendFilter : IElectStateFilter, IHangfireStartSuspend
    {
        public bool Suspend { get; set; } = true;
        public int DelayNextStateForSeconds { get; set; } = 10;

        public void OnStateElection(ElectStateContext context)
        {
            var processing = context.CandidateState as ProcessingState;
            if (processing == null || !Suspend)
            {
                return;
            }

            context.CandidateState = new ScheduledState(TimeSpan.FromSeconds(DelayNextStateForSeconds)) { Reason = "Awaiting for platform startup complete." };
        }
    }
}
