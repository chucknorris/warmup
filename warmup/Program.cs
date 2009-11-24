namespace warmup
{
    using System;
    using System.Configuration;

    internal class Program
    {
        static void Main(string[] args)
        {
            //parse out command line
            // warmup web FHLBank.Grouping
            string templateName = args[0];
            string name = args[1];

            //svn only
            var baseUri = new Uri(ConfigurationManager.AppSettings["source_control"] + templateName);
            var td = new TargetDir(name);
            Console.WriteLine("svn exporting to: {0}", td.FullPath);
            Svn.Export(baseUri, td);

            Console.WriteLine("replacing tokens");
            td.ReplaceTokens(name);
        }
    }
}