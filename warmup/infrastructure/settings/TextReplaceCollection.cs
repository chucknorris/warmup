using System.Configuration;

namespace warmup.infrastructure.settings
{
    /// <summary>
    ///   Text replace config collection
    /// </summary>
    public class TextReplaceCollection : ConfigurationElementCollection
    {
        /// <summary>
        ///   When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" /> .
        /// </summary>
        /// <returns> A new <see cref="T:System.Configuration.ConfigurationElement" /> . </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TextReplaceItem();
        }

        /// <summary>
        ///   Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element"> The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for. </param>
        /// <returns> An <see cref="T:System.Object" /> that acts as the key for the specified <see
        ///    cref="T:System.Configuration.ConfigurationElement" /> . </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TextReplaceItem) element).Find;
        }

        /// <summary>
        ///   Clears this instance.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        ///   Indexes the of.
        /// </summary>
        /// <param name="textReplaceItem"> The text replace item. </param>
        /// <returns> </returns>
        public int IndexOf(TextReplaceItem textReplaceItem)
        {
            return BaseIndexOf(textReplaceItem);
        }

        /// <summary>
        ///   Adds the specified text replace item.
        /// </summary>
        /// <param name="textReplaceItem"> The text replace item. </param>
        public void Add(TextReplaceItem textReplaceItem)
        {
            BaseAdd(textReplaceItem);
        }
    }

    /// <summary>
    ///   Text replace item
    /// </summary>
    public class TextReplaceItem : ConfigurationElement
    {
        /// <summary>
        ///   Gets or sets the find.
        /// </summary>
        /// <value> The find. </value>
        [ConfigurationProperty("find", IsRequired = true, IsKey = true)]
        public string Find
        {
            get { return (string) this["find"]; }
            set { this["find"] = value; }
        }

        /// <summary>
        ///   Gets or sets the replace.
        /// </summary>
        /// <value> The replace. </value>
        [ConfigurationProperty("replace", IsRequired = true, IsKey = false)]
        public string Replace
        {
            get { return (string) this["replace"]; }
            set { this["replace"] = value; }
        }
    }
}