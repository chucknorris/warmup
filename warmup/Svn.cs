using warmup.settings;

namespace warmup
{
    using System;
    using System.Diagnostics;

    public class Svn : IExporter
    {
        public static void SvnExport(Uri sourceLocation, TargetDir target)
        {
            var psi = new ProcessStartInfo("svn",
                string.Format("export {0} {1}", sourceLocation, target.FullPath));

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            //todo: better error handling
            Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);
            string output, error = "";
            using (var p = Process.Start(psi))
            {
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
            }

            Console.WriteLine(output);
            Console.WriteLine(error);
        }

        public void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + templateName);
            Console.WriteLine("svn exporting to: {0}", targetDir.FullPath);
            SvnExport(baseUri, targetDir);
        }
    }
}