using System.Configuration;

namespace warmup.infrastructure.settings
{
    public class IgnoredFolderCollection : ConfigurationElementCollection //, IEnumerable<IgnoredFileType>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoredFolder();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoredFolder) element).Folder;
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(IgnoredFolder ignoredFolder)
        {
            return BaseIndexOf(ignoredFolder);
        }

        public void Add(IgnoredFolder ignoredFolder)
        {
            BaseAdd(ignoredFolder);
        }
    }

    public class IgnoredFolder : ConfigurationElement
    {
        [ConfigurationProperty("folder", IsRequired = true, IsKey = true)]
        public string Folder
        {
            get { return (string) this["folder"]; }
            set { this["folder"] = value; }
        }
    }
}