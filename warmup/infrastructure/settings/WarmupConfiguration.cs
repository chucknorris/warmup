using System.Configuration;

namespace warmup.infrastructure.settings
{
    /// <summary>
    ///   Configuration Handler for Warmup
    /// </summary>
    public sealed class WarmupConfiguration : ConfigurationSection
    {
        private static readonly WarmupConfiguration _settings =
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

        [ConfigurationProperty("ignoredFolders", IsDefaultCollection = false)]
        public IgnoredFolderCollection IgnoredFolderCollection
        {
            get { return (IgnoredFolderCollection) base["ignoredFolders"]; }
        }

        /// <summary>
        ///   Gets the text replace collection.
        /// </summary>
        [ConfigurationProperty("textReplaces", IsDefaultCollection = false)]
        public TextReplaceCollection TextReplaceCollection
        {
            get { return (TextReplaceCollection) base["textReplaces"]; }
        }
    }
}