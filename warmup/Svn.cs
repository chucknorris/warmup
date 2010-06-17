// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace warmup
{
    using System;
    using System.Diagnostics;
    using settings;

    public class Svn :
        IExporter
    {
        public static void SvnExport(Uri sourceLocation, TargetDir target)
        {
            var psi = new ProcessStartInfo("svn",
                                           string.Format("export {0} {1}", sourceLocation, target.FullPath));

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            //todo: better error handling
            Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);
            string output, error = "";
            using (Process p = Process.Start(psi))
            {
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
            }

            Console.WriteLine(output);
            Console.WriteLine(error);
        }

        public void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + templateName);
            Console.WriteLine("svn exporting to: {0}", targetDir.FullPath);
            SvnExport(baseUri, targetDir);
        }
    }
}