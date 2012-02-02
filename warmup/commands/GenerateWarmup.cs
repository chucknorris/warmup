using System;
using warmup.infrastructure;
using warmup.settings;

namespace warmup.commands
{
    [Command("")]
    public class GenerateWarmup : ICommand
    {
        public void Run(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                ShowHelp();
                Environment.Exit(-1);
            }
            //parse out command line
            // warmup web newprojName
            string templateName = args[0];
            string name = args[1];
            string target = null;
            if (args.Length > 2) target = args[2];

            var td = new TargetDir(name);
            IExporter exporter = GetExporter();
            exporter.Export(WarmupConfiguration.settings.SourceControlWarmupLocation, templateName, td);
            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
            td.MoveToDestination(target);
        }

        static IExporter GetExporter()
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

        public void ShowHelp()
        {
            CommonHelp.ShowHelp();
            Console.WriteLine("----------");
            Console.WriteLine("usage for generate (default)");
            Console.WriteLine("----------");
            Console.WriteLine("warmup templateFolderName replacementName [targetDirectoryIfDifferentThanReplacementName]");
            Console.WriteLine("Example: warmup base Bob");
            Console.WriteLine("Example: 'base' is a subfolder in your warmup template that has a warmup template in it. 'Bob' is what you want to use instead of the token '__NAME__'.");
        }
    }
}