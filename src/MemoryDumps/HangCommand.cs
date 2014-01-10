using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FubuCore.CommandLine;

namespace MemoryDumps
{
    [CommandDescription("Causes the process to hang", Name = "hang")]
    public class HangCommand : FubuCommand<HangInput>
    {
        private readonly WaitHandle _waitHandle = new ManualResetEvent(false);

        public override bool Execute(HangInput input)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 4; i++)
            {
                tasks.Add(Task.Run(new Action(SimulatedBusThread)));
            }

            Task.WaitAll(tasks.ToArray());
            return true;
        }

        private void SimulatedBusThread()
        {
            _waitHandle.WaitOne();
        }
    }

    public class HangInput
    {
    }
}