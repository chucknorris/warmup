namespace warmup
{
    using System;
    using System.Diagnostics;

    public class Git
    {
        public static void Clone(Uri sourceLocation, TargetDir target)
        {
            var psi = new ProcessStartInfo("git", string.Format("clone {0} {1}", sourceLocation, target.FullPath));
            using (var p = Process.Start(psi))
            {
                //log
                p.WaitForExit();
            }
        }
    }
}