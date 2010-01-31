namespace warmup
{
    using System;
    using settings;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //parse out command line
            // warmup web FHLBank.Grouping
            string templateName = args[0];
            string name = args[1];

            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + templateName);
            var td = new TargetDir(name);
            Console.WriteLine("svn exporting to: {0}", td.FullPath);
            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    Svn.Export(baseUri, td);
                    break;
                case SourceControlType.Git:
                    Git.Clone(baseUri, td);
                    break;
            }

            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
        }
    }
}