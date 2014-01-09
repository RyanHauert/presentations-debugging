using System;
using System.Threading.Tasks;
using FubuCore.CommandLine;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using StructureMap;

namespace MemoryDumps
{
    public class Program
    {
        private static Logger Logger;

        public static void Main(string[] args)
        {
            var commands = Bootstrap();
            var executor = new CommandExecutor(commands);
            var command = Help(executor);

            while (command != "exit")
            {
                executor.Execute(command);
                command = Help(executor);
            }
        }

        private static CommandFactory Bootstrap()
        {
            ConfigureNLog();
            ConfigureErrorLogging();

            var container = ObjectFactory.Container;
            container.Configure(x => { });
            var commands = new CommandFactory();
            commands.RegisterCommands(typeof(IFubuCommand).Assembly);
            commands.RegisterCommands(typeof(Program).Assembly);
            return commands;
        }

        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget();
            consoleTarget.Layout = "${logger} ${message}";

            var slowTarget = new SlowTarget(consoleTarget);
            var asyncTarget = new AsyncTargetWrapper(slowTarget);
            config.AddTarget("slow", asyncTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, asyncTarget));
            LogManager.Configuration = config;
            Logger = LogManager.GetCurrentClassLogger();
        }

        private static void ConfigureErrorLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Logger.ErrorException("A exception was thrown from a Task and was not observed", e.Exception);

            // This will prevent the exception from killing the process.
            e.SetObserved();
        }

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.ErrorException("An unhandled exception was thrown", (Exception)e.ExceptionObject);
        }

        private static string Help(CommandExecutor executor)
        {
            executor.Execute("help");
            var command = Console.ReadLine();
            return command;
        }
    }
}