using System;
using System.Threading;
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace MemoryDumps
{
    public class SlowTarget : WrapperTargetBase
    {
        public SlowTarget(Target wrappedTarget)
        {
            WrappedTarget = wrappedTarget;
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WrappedTarget.WriteAsyncLogEvent(logEvent);
        }
    }
}