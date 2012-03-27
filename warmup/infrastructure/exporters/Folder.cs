using System;
using System.IO;

namespace warmup.infrastructure.exporters
{
    internal class Folder : BaseExporter
    {
        public override void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            string baseDir = Path.Combine(sourceControlWarmupLocation, templateName);
            Console.WriteLine("Copying to: {0}", targetDir.FullPath);
            CopyDirectory(baseDir, targetDir.FullPath, GetIgnoredFolders());
        }
    }
}