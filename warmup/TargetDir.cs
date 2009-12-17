namespace warmup
{
    using System;
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
            var startingPoint = new DirectoryInfo(FullPath);

            //move all directories
            MoveAllDirectories(startingPoint, name);

            //move all files
            MoveAllFiles(startingPoint, name);

            //replace file content
            ReplaceTokensInTheFiles(startingPoint, name);
        }

        private void ReplaceTokensInTheFiles(DirectoryInfo point, string name)
        {
            foreach (var info in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                //don't do this on exe's or dll's
                if (new[] {".exe", ".dll", ".pdb"}.Contains(info.Extension))
                    continue;

                //process contents
                var contents = File.ReadAllText(info.FullName);
                contents = contents.Replace("__NAME__", name);
                File.WriteAllText(info.FullName, contents);
            }
        }

        private void MoveAllFiles(DirectoryInfo point, string name)
        {
            foreach (var file in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var moveTo = file.FullName.Replace("__NAME__", name);
                file.MoveTo(moveTo);
            }
        }

        private void MoveAllDirectories(DirectoryInfo dir, string name)
        {
            DirectoryInfo workingDirectory = dir;
            if (workingDirectory.Name.StartsWith("__"))
            {
                var moveTo = dir.FullName.Replace("__NAME__", name);
                workingDirectory.MoveTo(moveTo);
                workingDirectory = new DirectoryInfo(moveTo);
            }

            foreach (var info in workingDirectory.GetDirectories())
            {
                MoveAllDirectories(info, name);
            }
        }
    }
}