// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace warmup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using infrastructure.settings;

    [DebuggerDisplay("{FullPath}")]
    public class TargetDir
    {
        readonly string _path;
        readonly string _replacementToken;

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

        void ReplaceTokensInTheFiles(DirectoryInfo point, string name)
        {
            List<string> ignoredExtensions = GetIgnoredExtensions();
            foreach (var info in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (ignoredExtensions.Contains(info.Extension, StringComparer.InvariantCultureIgnoreCase)) continue;

                //skip the .git directory
                if (new[] {"\\.git\\"}.Contains(info.FullName)) continue;

                // skip readonly and hidden files
                if (info.IsReadOnly || (info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;

                //process contents
                string contents = File.ReadAllText(info.FullName);
                
                // replace main token
                contents = contents.Replace(_replacementToken, name);

                // replace custom tokens
                foreach (TextReplaceItem replaceItem in GetReplaceTokens())
                {
                    contents = contents.Replace(replaceItem.Find, replaceItem.Replace);
                }

                var originalFileEncoding = GetFileEncoding(info.FullName);
                File.WriteAllText(info.FullName, contents, originalFileEncoding);
            }
        }

        private Encoding GetFileEncoding(string fileName)
        {
            Encoding encoding = null;

            var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (file.CanSeek)
            {
                //use ASCII as the default if it doesn't match any of the others.
                encoding = Encoding.ASCII;

                //determine encoding based on first 4 bytes of the file.
                var bom = new byte[4];
                file.Read(bom, 0, 4);
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                    encoding = Encoding.UTF8;
                if (bom[0] == 0xff && bom[1] == 0xfe)
                    encoding = Encoding.Unicode;
                if (bom[0] == 0xfe && bom[1] == 0xff)
                    encoding = Encoding.Unicode;
                if (bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xfe && bom[3] == 0xff)
                    encoding = Encoding.Unicode;

                file.Close();
            }
            else
            {
                encoding = Encoding.ASCII;
            }

            return encoding;
        }

        IEnumerable<TextReplaceItem> GetReplaceTokens()
        {
            return new List<TextReplaceItem>(WarmupConfiguration.settings.TextReplaceCollection.Cast<TextReplaceItem>());
        }

        static List<string> GetIgnoredExtensions()
        {
            var extension = new List<string>();
            foreach (IgnoredFileType ignoredFileType in WarmupConfiguration.settings.IgnoredFileTypeCollection)
            {
                extension.Add(string.Format(".{0}", ignoredFileType.Extension));
            }
            return extension;
        }

        void MoveAllFiles(DirectoryInfo point, string name)
        {
            foreach (var file in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string moveTo = file.FullName.Replace(_replacementToken, name);
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

        void MoveAllDirectories(DirectoryInfo dir, string name)
        {
            DirectoryInfo workingDirectory = dir;
            if (workingDirectory.Name.Contains(_replacementToken))
            {
                string newFolderName = dir.Name.Replace(_replacementToken, name);
                string moveTo = Path.Combine(dir.Parent.FullName, newFolderName);

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

        public void MoveToDestination(string target)
        {
            if (string.IsNullOrEmpty(target)) return;
            if (!Directory.Exists(target)) return;
            if (target == FullPath) return;

            DirectoryInfo folder = new DirectoryInfo(FullPath);
            var destination = Path.Combine(target, folder.Name);
            Console.WriteLine(string.Format("move {0} to {1}", FullPath, destination));
            folder.MoveTo(destination);
        }
    }
}