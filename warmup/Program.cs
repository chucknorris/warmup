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
            Svn.Export(baseUri, td);
            td.ReplaceTokens(name);
        }
    }
}