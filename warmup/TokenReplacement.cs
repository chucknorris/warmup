using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using warmup.infrastructure.settings;

namespace warmup
{
    public class TokenReplacement
    {
        private static List<string> GetIgnoredExtensions()
        {
            var extension = new List<string>();
            foreach (IgnoredFileType ignoredFileType in WarmupConfiguration.settings.IgnoredFileTypeCollection)
            {
                extension.Add(string.Format(".{0}", ignoredFileType.Extension));
            }
            return extension;
        }

        public static void ReplaceTokensInTheFiles(DirectoryInfo directory, IDictionary<string, string> tokens)
        {
            List<string> ignoredExtensions = GetIgnoredExtensions();

            foreach (var info in directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (ignoredExtensions.Contains(info.Extension, StringComparer.InvariantCultureIgnoreCase)) continue;
                //skip the .git directory
                if (new[] { "\\.git\\" }.Contains(info.FullName)) continue;
                // skip readonly and hidden files
                if (info.IsReadOnly || (info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;

                ReplaceTokenInFile(info, tokens);
            }
        }

        public static void ReplaceTokenInFile(FileInfo info, IDictionary<string, string> tokens)
        {
            var fileContents = new StringBuilder(File.ReadAllText(info.FullName));

            foreach (var token in tokens) fileContents.Replace(token.Key, token.Value);

            var originalFileEncoding = GetFileEncoding(info.FullName);

            File.WriteAllText(info.FullName, fileContents.ToString(), originalFileEncoding);
        }

        public static Encoding GetFileEncoding(string fileName)
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

    }
}
