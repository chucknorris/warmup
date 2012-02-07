using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Ionic.Zip;

namespace warmup
{
    public class GitHub : IExporter
    {
        public void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var gitHubZipUrl = GenerateZipDownloadUrl(githubRepoUrl: templateName);

            var pathToZip = DownloadFile(gitHubZipUrl);

            var extractionPath = GenerateTempExtractionPath();

            using(ZipFile zipFile = new ZipFile(pathToZip))
            {
                zipFile.ExtractAll(extractionPath, ExtractExistingFileAction.OverwriteSilently);
            }

            var sourceDirectory = Directory.GetDirectories(extractionPath).First();

            FixLineFeedsInSolutionFile(sourceDirectory);

            Folder.CopyDirectory(Path.Combine(pathToZip, sourceDirectory), targetDir.FullPath);
        }

        public string DownloadFile(string url)
        {
            var client = new WebClient();

            var zipUrl = url;

            var tempFile = Path.GetTempFileName();

            client.DownloadFile(zipUrl, tempFile);

            return tempFile;
        }

        public string GenerateZipDownloadUrl(string githubRepoUrl)
        {
            if (!githubRepoUrl.EndsWith("/")) githubRepoUrl += "/";

            return githubRepoUrl + "zipball/master";
        }

        public string GenerateTempExtractionPath()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        private void FixLineFeedsInSolutionFile(string sourceDirectory)
        {
            var solutionFile = Directory.GetFiles(sourceDirectory, "*.sln").FirstOrDefault();

            if (solutionFile == null) return;

            var content = File.ReadAllText(solutionFile, Encoding.UTF8);

            content = ConvertLineFeedsToCarriageReturnLineFeed(content);

            File.WriteAllText(solutionFile, content, Encoding.UTF8);
        }

        string ConvertLineFeedsToCarriageReturnLineFeed(string s)
        {
            string contents = s;
        
            contents = contents.Replace("\r\n", "\n");
        
            contents = contents.Replace("\n", "\r\n");
        
            return contents;
        }
    }
}
