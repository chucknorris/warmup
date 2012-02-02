using System;
using System.IO;
using warmup.infrastructure;
using warmup.infrastructure.console;
using warmup.settings;

namespace warmup.commands
{
    [Command("addTemplateFolder")]
    public class AddTemplateFolder : ICommand
    {
        public void Run(string[] args)
        {
            if (args == null || args.Length !=3)
            {
                ShowHelp();
                Environment.Exit(-1);
            }

            var linkName = args[1];
            var targetPath = args[2].Replace("\"",string.Empty);

            var templatesFolder = WarmupConfiguration.settings.SourceControlWarmupLocation;
            if (!Directory.Exists(templatesFolder))
            {
                Directory.CreateDirectory(templatesFolder);
            }

            var linkFolder = Path.Combine(templatesFolder,linkName);
            
            Console.WriteLine("Generating symbolic link \"{0}\" pointing to \"{1}\"",linkFolder,targetPath);
            CommandRunner.Run("cmd.exe", string.Format("/c mklink /d \"{0}\" \"{1}\"",linkFolder,targetPath), waitForExit:true,assertFullPath:false);
        }

        public void ShowHelp()
        {
            CommonHelp.ShowHelp();
            Console.WriteLine("----------");
            Console.WriteLine("usage for addTemplateFolder");
            Console.WriteLine("----------");
            Console.WriteLine("warmup addTemplateFolder templateName \"FullPath\"");
            Console.WriteLine("Example: warmup addTextReplacement SomeTemplate \"C:\\somefolder\\somerepo\\SomeTemplate\"");
            Console.WriteLine("Example: 'SomeTemplate' is the name of the template to add, \"C:\\somefolder\\somerepo\\SomeTemplate\" is the path to the template.");
        }
    }
}