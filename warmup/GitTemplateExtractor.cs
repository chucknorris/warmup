using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace warmup
{
    public class GitTemplateExtractor
    {
        private const StringComparison Comparison = StringComparison.InvariantCultureIgnoreCase;
        private readonly TargetDir _target;
        private readonly string _templateName;

        public GitTemplateExtractor(TargetDir target, string templateName)
        {
            _target = target;
            _templateName = templateName;
        }

        public void Extract()
        {
            var topParent = new DirectoryInfo(_target.FullPath);
            var directories = topParent.GetDirectories();
            var files = topParent.GetFiles();

            if (TemplateNotFound(directories, files)) return;
            
            CleanTopParent(directories, files);

            var templateDir = directories.FirstOrDefault(d => d.Name.Equals(_templateName, Comparison));
            if (templateDir != null) MoveTemplateContent(templateDir, topParent);
        }

        private void CleanTopParent(IEnumerable<DirectoryInfo> directories, IEnumerable<FileInfo> files)
        {
            foreach (var directory in directories.Where(directory => 
                directory.Name != _templateName))
                DeleteDirectory(directory);
            foreach (var file in files.Where(file => 
                !file.Name.Equals(_templateName + file.Extension, Comparison)))
                SafeDeleteFile(file);
        }

        private bool TemplateNotFound(IEnumerable<DirectoryInfo> directories, IEnumerable<FileInfo> files)
        {
            return !directories.Any(di => di.Name.Equals(_templateName, Comparison)) &&
                   !files.Any(f => f.Name.Equals(_templateName + f.Extension, Comparison));
        }

        private static void SafeDeleteFile(FileInfo file)
        {
            file.Attributes = FileAttributes.Normal;
            file.Delete();
        }

        private static void DeleteDirectory(DirectoryInfo directory)
        {
            foreach (var dir in directory.GetDirectories())
                DeleteDirectory(dir);
            foreach (var file in directory.GetFiles())
                SafeDeleteFile(file);
            directory.Attributes = FileAttributes.Normal;
            directory.Delete();
        }

        private static void MoveTemplateContent(DirectoryInfo templateDir, DirectoryInfo destinationDir)
        {
            foreach (var dir in templateDir.GetDirectories())
                dir.MoveTo(Path.Combine(destinationDir.FullName, dir.Name));
            foreach (var file in templateDir.GetFiles())
                file.MoveTo(Path.Combine(destinationDir.FullName, file.Name));
            templateDir.Delete();
        }
    }
}