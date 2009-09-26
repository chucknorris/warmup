namespace warmup
{
    using System;
    using System.Diagnostics;

    public class Svn
    {
        public static void Export(Uri sourceLocation, TargetDir target)
        {
            var psi = new ProcessStartInfo("svn", string.Format("export {0} {1}", sourceLocation, target.FullPath));
            using (var p = Process.Start(psi))
            {
                //log
                p.WaitForExit();
            }
        }
    }
}