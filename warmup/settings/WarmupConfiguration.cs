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
namespace warmup.settings
{
    using System.Configuration;

    /// <summary>
    ///   Configuration Handler for Warmup
    /// </summary>
    public sealed class WarmupConfiguration : ConfigurationSection
    {
        static readonly WarmupConfiguration _settings =
            ConfigurationManager.GetSection("warmup") as WarmupConfiguration;

        /// <summary>
        ///   Settings section
        /// </summary>
        public static WarmupConfiguration settings
        {
            get { return _settings; }
        }

        /// <summary>
        ///   The top level location for warmup templates
        /// </summary>
        [ConfigurationProperty("sourceControlWarmupLocation", IsRequired = true)]
        public string SourceControlWarmupLocation
        {
            get { return (string) this["sourceControlWarmupLocation"]; }
        }

        /// <summary>
        ///   The token to replace in the warmup templates. Not required, default value is "__NAME__"
        /// </summary>
        [ConfigurationProperty("replacementToken", IsRequired = false, DefaultValue = "__NAME__")]
        public string ReplacementToken
        {
            get { return (string) this["replacementToken"]; }
        }

        /// <summary>
        ///   What type of source control are we using?
        /// </summary>
        [ConfigurationProperty("sourceControlType", IsRequired = false, DefaultValue = "svn")]
        public string SourceControl
        {
            get { return (string) this["sourceControlType"]; }
        }

        public SourceControlType SourceControlType
        {
            get
            {
                if (SourceControl.Contains("git"))
                {
                    return SourceControlType.Git;
                }
                if (SourceControl.ToLower().Contains("file"))
                {
                    return SourceControlType.FileSystem;
                }

                return SourceControlType.Subversion;
            }
        }

        [ConfigurationProperty("ignoredExtensions", IsDefaultCollection = false)]
        public IgnoredFileTypeCollection IgnoredFileTypeCollection
        {
            get { return (IgnoredFileTypeCollection) base["ignoredExtensions"]; }
        }
    }
}