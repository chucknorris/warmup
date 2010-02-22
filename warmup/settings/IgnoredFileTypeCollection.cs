


using System.Collections.Generic;

namespace warmup.settings
{

    using System;
    using System.Configuration;
    public class IgnoredFileTypeCollection : ConfigurationElementCollection, IEnumerable<IgnoredFileType>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoredFileType();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoredFileType)element).Extension;
        }
        public void Clear()
        {
            BaseClear();
        }
        public int IndexOf(IgnoredFileType ignoredFileType)
        {
            return BaseIndexOf(ignoredFileType);
        }

        public void Add(IgnoredFileType ignoredFileType)
        {
            BaseAdd(ignoredFileType);
        }
    }
    public class IgnoredFileType : ConfigurationElement
    {
        [ConfigurationProperty("ext", IsRequired = true, IsKey = true)]
        public string Extension
        {
            get
            {
                return (string)this["ext"];
            }
            set
            {
                this["ext"] = value;
            }
        }

    }


}
