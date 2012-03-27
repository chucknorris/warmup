using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using warmup.infrastructure.settings;

namespace warmup.infrastructure.exporters
{
    public abstract class BaseExporter : IExporter
    {
        public static void CopyDirectory(string source, string destination, IList<string> ignoredFolders)
        {
            if (ignoredFolders.Contains(Path.GetFileName(source), StringComparer.InvariantCultureIgnoreCase)) return;

            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar) destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            string[] files = Directory.GetFileSystemEntries(source);
            foreach (var element in files)
            {
                // Sub directories

                if (Directory.Exists(element))
                {
                    CopyDirectory(element, destination + Path.GetFileName(element), ignoredFolders);
                }
                // Files in directory
                else File.Copy(element, destination + Path.GetFileName(element), true);
            }
        }

        public static List<string> GetIgnoredFolders()
        {
            var folders = new List<string>();
            foreach (IgnoredFolder ignoredFolder in WarmupConfiguration.settings.IgnoredFolderCollection)
            {
                folders.Add(string.Format("{0}", ignoredFolder.Folder));
            }
            return folders;
        }

        public abstract void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir);
    }
}