using System.Collections.Generic;
using warmup.settings;

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
        private readonly string _replacementToken;

        public TargetDir(string path)
        {
            _path = path;
            _replacementToken = WarmupConfiguration.settings.ReplacementToken;
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

            startingPoint = new DirectoryInfo(startingPoint.FullName.Replace(_replacementToken, name));

            //move all files
            MoveAllFiles(startingPoint, name);

            //replace file content
            ReplaceTokensInTheFiles(startingPoint, name);
        }

        private void ReplaceTokensInTheFiles(DirectoryInfo point, string name)
        {
            List<string> ignoredExtensions = GetIgnoredExtensions();
            foreach (var info in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (ignoredExtensions.Contains(info.Extension)) continue;
                //skip the .git directory
                if (new[] { "\\.git\\" }.Contains(info.FullName)) continue;

                //process contents
                var contents = File.ReadAllText(info.FullName);
                contents = contents.Replace(_replacementToken, name);
                File.WriteAllText(info.FullName, contents);
            }
        }

        private static List<string> GetIgnoredExtensions()
        {
            var extension = new List<string>();
            foreach (IgnoredFileType ignoredFileType in WarmupConfiguration.settings.IgnoredFileTypeCollection)
            {
                extension.Add(string.Format(".{0}", ignoredFileType.Extension));
            }
            return extension;
        }

        private void MoveAllFiles(DirectoryInfo point, string name)
        {
            foreach (var file in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var moveTo = file.FullName.Replace(_replacementToken, name);
                try
                {

                    file.MoveTo(moveTo);
                }
                catch (Exception)
                {
                    Console.WriteLine("Trying to move '{0}' to '{1}'", file.FullName, moveTo);
                    throw;
                }

            }
        }

        private void MoveAllDirectories(DirectoryInfo dir, string name)
        {
            DirectoryInfo workingDirectory = dir;
            if (workingDirectory.Name.Contains(_replacementToken))
            {
                var newFolderName = dir.Name.Replace(_replacementToken, name);
                var moveTo = Path.Combine(dir.Parent.FullName, newFolderName);

                try
                {
                    workingDirectory.MoveTo(moveTo);
                    workingDirectory = new DirectoryInfo(moveTo);
                }
                catch (Exception)
                {
                    Console.WriteLine("Trying to move '{0}' to '{1}'", workingDirectory.FullName, moveTo);
                    throw;
                }
            }

            foreach (var info in workingDirectory.GetDirectories())
            {
                MoveAllDirectories(info, name);
            }
        }
    }
}