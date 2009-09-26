namespace warmup
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;

    internal class Program
    {
        static void Main(string[] args)
        {
            //parse out command line
            // warmup web FHLBank.Grouping

            //get content to FHLBank.Grouping\
            string templateName = args[0];
            string name = args[1];
            var baseUri = new Uri(ConfigurationManager.AppSettings["svn"] + templateName);
            var td = new TargetDir(name);
            //Svn.Export(baseUri, td);
            td.ReplaceTokens(name);
        }
    }


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

    [DebuggerDisplay("{FullPath}")]
    public class TargetDir
    {
        readonly string _path;

        public TargetDir(string path)
        {
            _path = path;
        }

        public string FullPath
        {
            get { return Path.GetFullPath(_path); }
        }

        public void ReplaceTokens(string name)
        {
            var di = new DirectoryInfo(FullPath);
            //directories
            foreach (var info in di.GetDirectories())
            {
                foreach (var directory in info.GetDirectories())
                {
                    if (info.Name.StartsWith("__"))
                    {
                        info.MoveTo(info.FullName.Replace("__NAME__", name));
                    }
                }

                if (info.Name.StartsWith("__"))
                {
                    info.MoveTo(info.FullName.Replace("__NAME__", name));
                }
            }

            foreach (var info in di.GetFiles("*.*", SearchOption.AllDirectories))
            {
                info.MoveTo(info.FullName.Replace("__NAME__", name));
                //process contents
                var contents = File.ReadAllText(info.FullName);
                contents = contents.Replace("__NAME__", name);
                File.WriteAllText(info.FullName, contents);
            }
        }
    }
}