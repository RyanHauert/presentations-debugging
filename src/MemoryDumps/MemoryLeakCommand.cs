using System.Threading.Tasks;
using FubuCore.CommandLine;
using NLog;

namespace MemoryDumps
{
    [CommandDescription("Simulates a memory leak", Name = "leak")]
    public class MemoryLeakCommand : FubuCommand<MemoryLeakInput>
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override bool Execute(MemoryLeakInput input)
        {
            Parallel.For(0, 8, x =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    _logger.Info("A log statement");
                }
            });

            return true;
        }
    }

    public class MemoryLeakInput
    {
    }
}