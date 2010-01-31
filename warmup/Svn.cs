namespace warmup
{
    using System;
    using System.Diagnostics;

    public class Svn
    {
        public static void Export(Uri sourceLocation, TargetDir target)
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
    }
}