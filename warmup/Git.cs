namespace warmup
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class Git
    {
        public static void Clone(Uri sourceLocation, TargetDir target)
        {
            string[] separation_characters = new string[] { ".git" };
            string[] piecesOfPath = sourceLocation.ToString().Split(separation_characters, StringSplitOptions.RemoveEmptyEntries);
            if (piecesOfPath != null && piecesOfPath.Length > 0)
            {
                string sourceLocationToGit = piecesOfPath[0] + ".git";

                var psi = new ProcessStartInfo("git",
                string.Format("clone {0} {1}", sourceLocationToGit, target.FullPath));

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                //todo: better error handling
                string output, error = "";
                using (var p = Process.Start(psi))
                {
                    output = p.StandardOutput.ReadToEnd();
                    error = p.StandardError.ReadToEnd();
                }

                Console.WriteLine(output);
                Console.WriteLine(error);

                string git_directory = Path.Combine(target.FullPath, ".git");
                if (Directory.Exists(git_directory))
                {
                    Directory.Delete(git_directory);
                }
                
            }

        }
    }
}