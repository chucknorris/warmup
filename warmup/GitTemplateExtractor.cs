using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

namespace warmup
{
    public class GitTemplateExtractor
    {
        private readonly TargetDir _target;
        private readonly string _templateName;

        public GitTemplateExtractor(TargetDir target, string templateName)
        {
            _target = target;
            _templateName = templateName;
        }

        public void Extract()
        {
            DirectoryInfo dir = new DirectoryInfo(_target.FullPath);
            var directories = dir.GetDirectories();
            var files = dir.GetFiles();
            //TODO: check f.Name == _templateName + "." + f.Extension; what f.Name returns.
            if (!directories.Any(di => di.Name == _templateName) &&
                !files.Any(f => f.Name == _templateName + "." + f.Extension)) return;
            //Clean parent
            foreach (var directory in directories)
                if (directory.Name != _templateName)
                    DeleteDirectory(directory);
            foreach (var file in files)
                if (file.Name != _templateName + "." + file.Extension)
                    file.Delete();
            var templateDir = directories.FirstOrDefault(d => d.Name == _templateName);
            if (templateDir != null) MoveTemplateContentUp(templateDir, dir);
        }

        private static void DeleteDirectory(DirectoryInfo directory)
        {
            foreach (var dir in directory.GetDirectories())
                DeleteDirectory(dir);
            foreach (var file in directory.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }
            directory.Attributes = FileAttributes.Normal;
            directory.Delete();
        }

        private static void MoveTemplateContentUp(DirectoryInfo templateDir, DirectoryInfo destinationDir)
        {
            foreach (var dir in templateDir.GetDirectories())
                dir.MoveTo(Path.Combine(destinationDir.FullName, dir.Name));
            foreach (var file in templateDir.GetFiles())
                file.MoveTo(Path.Combine(destinationDir.FullName, file.Name));
            templateDir.Delete();
        }
    }
}