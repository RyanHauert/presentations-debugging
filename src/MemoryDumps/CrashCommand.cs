using System;
using System.Threading;
using FubuCore.CommandLine;

namespace MemoryDumps
{
    [CommandDescription("Causes the process to crash", Name = "crash")]
    public class CrashCommand : FubuCommand<CrashInput>
    {
        public override bool Execute(CrashInput input)
        {
            ThreadPool.QueueUserWorkItem(_ => { throw new InvalidOperationException("I made the app crash"); });
            return true;
        }
    }

    public class CrashInput
    {
    }
}