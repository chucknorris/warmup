using System;
using System.Configuration;
using warmup.infrastructure;
using warmup.settings;

namespace warmup.commands
{
    [Command("addTextReplacement")]
    public class AddTextReplacement : ICommand
    {
        public void Run(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                ShowHelp();
                Environment.Exit(-1);
            }


            var find = args[1];
            var replace = args[2];

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            WarmupConfiguration warmupConfig = config.GetSection("warmup") as WarmupConfiguration;

            if (warmupConfig != null)
            {
                warmupConfig.SectionInformation.ForceSave = true;

                bool itemFound = false;

                foreach (TextReplaceItem replaceItem in warmupConfig.TextReplaceCollection)
                {
                    if (replaceItem.Find.ToLower() == find.ToLower())
                    {
                        Console.WriteLine("Replacing '{0}' value of '{1}' with '{2}'.", find,replaceItem.Replace, replace);
                        replaceItem.Replace = replace;
                        itemFound = true;
                    }
                }

                if (!itemFound)
                {
                    Console.WriteLine("Adding '{0}' with a replacement of '{1}' to the configuration.",find,replace.Replace("\"",string.Empty));
                    warmupConfig.TextReplaceCollection.Add(new TextReplaceItem { Find = find, Replace = replace });
                }

               // Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.Save(ConfigurationSaveMode.Full);
            }

        }

        public void ShowHelp()
        {
            CommonHelp.ShowHelp();
            Console.WriteLine("----------");
            Console.WriteLine("usage for addTextReplacement");
            Console.WriteLine("----------");
            Console.WriteLine("warmup addTextReplacement findName replacementName");
            Console.WriteLine("Example: warmup addTextReplacement __COMPANY__ \"somewheres, inc\"");
            Console.WriteLine("Example: '__COMPANY__' is the token to search for, \"somwheres, inc\" is the replacement text.");
        }
    }
}