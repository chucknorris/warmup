using System;
using warmup.infrastructure;
using warmup.infrastructure.exporters;
using warmup.infrastructure.settings;
using System.IO;

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

            if (WarmupConfiguration.settings.SourceControlType == SourceControlType.FileSystem)
            {
                if (!Directory.Exists(WarmupConfiguration.settings.SourceControlWarmupLocation))
                {
                    Console.WriteLine("Template directory not found");
                    Environment.Exit(-1);
                }

                if (!Directory.Exists(WarmupConfiguration.settings.SourceControlWarmupLocation + "\\" + templateName))
                {
                    Console.WriteLine("Template not found");
                    Environment.Exit(-1);
                }
            }


            var td = new TargetDir(name);
            IExporter exporter = GetExporter(templateName);
            exporter.Export(WarmupConfiguration.settings.SourceControlWarmupLocation, templateName, td);
            Console.WriteLine("Replacing tokens");
            td.ReplaceTokens(name);
            td.MoveToDestination(target);
        }

        private static IExporter GetExporter(string templateName)
        {
            if (IsNotConfiguredForGitButIsGitHubUrl(templateName)) return new GitHub();

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

        private static bool IsNotConfiguredForGitButIsGitHubUrl(string templateName)
        {
            return WarmupConfiguration.settings.SourceControlType != SourceControlType.Git && templateName.Contains("github.com");
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