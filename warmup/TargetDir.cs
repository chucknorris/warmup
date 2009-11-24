namespace warmup
{
    using System.Diagnostics;
    using System.IO;

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
                string newname = info.Name;
                if (info.Name.StartsWith("__"))
                {
                    newname = info.FullName.Replace("__NAME__", name);
                    info.MoveTo(newname);
                    di = new DirectoryInfo(newname);
                }

                
                foreach (var directory in info.GetDirectories())
                {
                    if (directory.Name.StartsWith("__"))
                    {
                        directory.MoveTo(directory.FullName.Replace("__NAME__", name));
                    }
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