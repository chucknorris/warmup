namespace warmup
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

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
                
                //don't do this on exe's or dll's
                if(new []{"exe","dll"}.Contains(info.Extension))
                    continue;

                //process contents
                var contents = File.ReadAllText(info.FullName);
                contents = contents.Replace("__NAME__", name);
                File.WriteAllText(info.FullName, contents);
            }
        }
    }
}