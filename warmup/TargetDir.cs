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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using infrastructure.settings;

    [DebuggerDisplay("{FullPath}")]
    public class TargetDir
    {
        private readonly string _path;
        private readonly IDictionary<string,string> _replacementTokens = new Dictionary<string, string>();
        private readonly string _mainReplacementToken;
        private readonly StringBuilder _directoryMoveTo = new StringBuilder();

        public TargetDir(string path)
        {
            _path = path;
            _mainReplacementToken = WarmupConfiguration.settings.ReplacementToken;
        }

        private void SetReplaceTokens(IDictionary<string, string> tokens)
        {
            foreach (TextReplaceItem token in WarmupConfiguration.settings.TextReplaceCollection)
            {
                tokens.Add(token.Find, token.Replace);
            }
        }

        public string FullPath
        {
            get { return Path.GetFullPath(_path); }
        }

        public void ReplaceTokens(string name)
        {
            // add the main token with the replacement value
            _replacementTokens.Add(_mainReplacementToken,name);
            SetReplaceTokens(_replacementTokens);
           
            var startingPoint = new DirectoryInfo(FullPath);

            //move all directories
            MoveAllDirectories(startingPoint, _replacementTokens);

            startingPoint = new DirectoryInfo(startingPoint.FullName.Replace(_mainReplacementToken, name));

            //move all files
            MoveAllFiles(startingPoint, _replacementTokens);

            //replace file content
            TokenReplacement.ReplaceTokensInTheFiles(startingPoint, _replacementTokens);
        }

        private void MoveAllDirectories(DirectoryInfo dir, IDictionary<string, string> tokens)
        {
            _directoryMoveTo.Clear();

            DirectoryInfo workingDirectory = dir;
            var originalName = workingDirectory.Name;

            _directoryMoveTo.Append(workingDirectory.Name);

            foreach (var token in tokens)
            {
                _directoryMoveTo.Replace(token.Key, token.Value);
            }

            var newName = _directoryMoveTo.ToString();
            if (!originalName.Equals(newName, StringComparison.InvariantCultureIgnoreCase))
            {
                //string newFolderName = dir.Name.Replace(_replacementToken, name);
                string moveTo = Path.Combine(workingDirectory.Parent.FullName, newName);

                try
                {
                    //Console.WriteLine("Moving '{0}' to '{1}'", workingDirectory.FullName, moveTo);
                    workingDirectory.MoveTo(moveTo);
                    workingDirectory = new DirectoryInfo(moveTo);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error trying to move '{0}' to '{1}'", workingDirectory.FullName, moveTo);
                    throw;
                }
            }

            foreach (var info in workingDirectory.GetDirectories())
            {
                MoveAllDirectories(info, tokens);
            }
        }

        private void MoveAllFiles(DirectoryInfo point, IDictionary<string, string> tokens)
        {
            var moveTo = new StringBuilder();

            foreach (var file in point.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var originalName = file.Name;                

                moveTo.Clear();
                moveTo.Append(file.Name);

                foreach (var token in tokens)
                {
                    moveTo.Replace(token.Key, token.Value);
                }

                string newName = moveTo.ToString();
                if (!originalName.Equals(newName,StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        string moveFileTo = Path.Combine(file.DirectoryName, newName);
                        //Console.WriteLine("Moving '{0}' to '{1}'", file.FullName, moveFileTo);
                        file.MoveTo(moveFileTo);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error trying to move '{0}' to '{1}'", file.FullName, moveTo);
                        throw;
                    }
                }
            }
        }
        
        public void MoveToDestination(string target)
        {
            if (string.IsNullOrEmpty(target)) return;
            if (!Directory.Exists(target)) return;
            if (target == FullPath) return;

            DirectoryInfo folder = new DirectoryInfo(FullPath);
            var destination = Path.Combine(target, folder.Name);
            Console.WriteLine(string.Format("move {0} to {1}", FullPath, destination));
            folder.MoveTo(destination);
        }
    }
}