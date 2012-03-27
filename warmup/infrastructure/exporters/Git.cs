using System;
using System.Diagnostics;
using warmup.infrastructure.extractors;
using warmup.infrastructure.settings;

namespace warmup.infrastructure.exporters
{
    public class Git : BaseExporter
    {
        public static void Clone(Uri sourceLocation, TargetDir target)
        {
            var separationCharacters = new[] {".git"};
            string[] piecesOfPath = sourceLocation.ToString().Split(separationCharacters, StringSplitOptions.RemoveEmptyEntries);
            if (piecesOfPath != null && piecesOfPath.Length > 0)
            {
                string sourceLocationToGit = piecesOfPath[0] + ".git";

                var psi = new ProcessStartInfo("cmd",string.Format(" /c git clone {0} {1}", sourceLocationToGit, target.FullPath));

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                //todo: better error handling
                Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);
                string output, error = "";
                using (Process p = Process.Start(psi))
                {
                    output = p.StandardOutput.ReadToEnd();
                    error = p.StandardError.ReadToEnd();
                }

                Console.WriteLine(output);
                Console.WriteLine(error);

                var templateName = piecesOfPath[1];
                GitTemplateExtractor extractor = new GitTemplateExtractor(target, templateName);
                extractor.Extract();
                //string git_directory = Path.Combine(target.FullPath, ".git");
                //if (Directory.Exists(git_directory))
                //{
                //    Console.WriteLine("Deleting {0} directory", git_directory);
                //    Directory.Delete(git_directory, true);
                //}
            }
        }

        public override void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + templateName);
            Console.WriteLine("git exporting to: {0}", targetDir.FullPath);
            Clone(baseUri, targetDir);
        }
    }
}