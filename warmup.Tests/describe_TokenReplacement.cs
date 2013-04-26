using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using System.IO;

namespace warmup.Tests
{
    class describe_TokenReplacement : nspec
    {
        string token = "__NAME__";

        string newValue = "Replace";

        string processedFile = "";

        void file_encodings()
        {
            beforeEach = () => File.SetAttributes(@"BomTestFiles\DynamicBlog.slnfile", FileAttributes.ReadOnly);

            it["UTF8 encoding is retained after token replacement"] = () =>
            {
                string slnFile = @"BomTestFiles\DynamicBlog.slnfile";

                string processedFile = ReplaceTokens(slnFile, token, newValue);

                EncodingFor(processedFile).should_be(Encoding.UTF8);
            };

            it["ASCII encoding is retained after token replacement"] = () =>
            {
                string ymlFile = @"BomTestFiles\dev.yml";

                string processedFile = ReplaceTokens(ymlFile, token, newValue);

                EncodingFor(processedFile).should_be(Encoding.ASCII);
            };
        }

        void replacing_tokens()
        {
            before = () => File.SetAttributes(@"BomTestFiles\DynamicBlog.slnfile", FileAttributes.ReadOnly);

            it["tokens in file are replaced with new value"] = () =>
            {
                string slnFile = @"BomTestFiles\DynamicBlog.slnfile";

                string processedFile = ReplaceTokens(slnFile, token, newValue);

                Text(slnFile).Replace(token, newValue).should_be(Text(processedFile));
            };
        }

        string ReplaceTokens(string file, string token, string newValue)
        {
            string processedFile = file + ".clone";

            File.Copy(file, processedFile, overwrite: true);

            TokenReplacement.ReplaceTokenInFile(new FileInfo(processedFile), new Dictionary<string, string> { { token, newValue } });

            return processedFile;
        }

        string Text(string path)
        {
            return File.ReadAllText(path);
        }

        Encoding EncodingFor(string path)
        {
            return TokenReplacement.GetFileEncoding(path);
        }
    }
}
