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
            
            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    Console.WriteLine("svn exporting to: {0}", td.FullPath);
                    Svn.Export(baseUri, td);
                    break;
                case SourceControlType.Git:
                    Console.WriteLine("Hardcore git cloning action to: {0}", td.FullPath);
                    Git.Clone(baseUri, td);
                    break;
            }

            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
        }
    }
}