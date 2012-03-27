using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace warmup.infrastructure.console
{
    public class CommandRunner
    {
        public static int Run(string process, string arguments, bool waitForExit)
        {
            return Run(process, arguments, waitForExit, true);
        }

        public static int Run(string process, string arguments, bool waitForExit, bool assertFullPath)
        {
            var processPath = assertFullPath ? Path.GetFullPath(process) : process;

            Console.WriteLine("Attempting to run '{0}' with arguments '{1}'", processPath, arguments);

            int exitCode = -1;
            ProcessStartInfo psi = new ProcessStartInfo(processPath, arguments)
                                       {
                                           WorkingDirectory =
                                               Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                           UseShellExecute = false,
                                           RedirectStandardOutput = true,
                                           RedirectStandardError = true,
                                           CreateNoWindow = true
                                       };

            StreamReader standardOutput;
            StreamReader standardError;

            using (Process p = new Process())
            {
                p.StartInfo = psi;
                p.Start();

                standardOutput = p.StandardOutput;
                standardError = p.StandardError;
                if (waitForExit)
                {
                    p.WaitForExit();
                }
                exitCode = p.ExitCode;
            }

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(standardError.ReadToEnd());
            Console.ResetColor();
            Console.WriteLine(standardOutput.ReadToEnd());

            return exitCode;
        }
    }
}