using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Ionic.Zip;

namespace warmup.infrastructure.exporters
{
    public class GitHub : BaseExporter
    {
        public override void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var gitHubZipUrl = GenerateZipDownloadUrl(githubRepoUrl: templateName);

            var pathToZip = DownloadFile(gitHubZipUrl);

            var extractionPath = GenerateTempExtractionPath();

            using (ZipFile zipFile = new ZipFile(pathToZip))
            {
                zipFile.ExtractAll(extractionPath, ExtractExistingFileAction.OverwriteSilently);
            }

            var sourceDirectory = Directory.GetDirectories(extractionPath).First();

            FixLineFeedsInSolutionFile(sourceDirectory);

            CopyDirectory(Path.Combine(pathToZip, sourceDirectory), targetDir.FullPath, GetIgnoredFolders());
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

        private string ConvertLineFeedsToCarriageReturnLineFeed(string s)
        {
            string contents = s;

            contents = contents.Replace("\r\n", "\n");

            contents = contents.Replace("\n", "\r\n");

            return contents;
        }
    }
}