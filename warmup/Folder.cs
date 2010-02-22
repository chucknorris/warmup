using System;
using System.IO;

namespace warmup
{
    class Folder : IExporter
    {
        public void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var baseDir = Path.Combine(sourceControlWarmupLocation, templateName);
            Console.WriteLine("Copying to: {0}", targetDir.FullPath);
            CopyDirectory(baseDir, targetDir.FullPath);
        }
        public static void CopyDirectory(string source, string destination)
        {
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            var files = Directory.GetFileSystemEntries(source);
            foreach (var element in files)
            {
                // Sub directories

                if (Directory.Exists(element))
                    CopyDirectory(element, destination + Path.GetFileName(element));
                // Files in directory

                else
                    File.Copy(element, destination + Path.GetFileName(element), true);
            }
        }

    }
}