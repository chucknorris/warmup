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

            var td = new TargetDir(name);
            var exporter = GetExporter();
            exporter.Export(WarmupConfiguration.settings.SourceControlWarmupLocation, templateName, td);
            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
        }

        private static IExporter GetExporter()
        {
            switch (WarmupConfiguration.settings.SourceControlType)
            {
                case SourceControlType.Subversion:
                    return new Svn();
                case SourceControlType.Git:
                    return new Git();
                case SourceControlType.FileSystem:
                    return new Folder();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}