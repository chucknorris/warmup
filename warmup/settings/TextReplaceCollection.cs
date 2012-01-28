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
    /// Text replace config collection
    /// </summary>
    public class TextReplaceCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TextReplaceItem();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TextReplaceItem)element).Find;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="textReplaceItem">The text replace item.</param>
        /// <returns></returns>
        public int IndexOf(TextReplaceItem textReplaceItem)
        {
            return BaseIndexOf(textReplaceItem);
        }

        /// <summary>
        /// Adds the specified text replace item.
        /// </summary>
        /// <param name="textReplaceItem">The text replace item.</param>
        public void Add(TextReplaceItem textReplaceItem)
        {
            BaseAdd(textReplaceItem);
        }
    }

    /// <summary>
    /// Text replace item
    /// </summary>
    public class TextReplaceItem : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the find.
        /// </summary>
        /// <value>
        /// The find.
        /// </value>
        [ConfigurationProperty("find", IsRequired = true, IsKey = true)]
        public string Find
        {
            get { return (string)this["find"]; }
            set { this["find"] = value; }
        }

        /// <summary>
        /// Gets or sets the replace.
        /// </summary>
        /// <value>
        /// The replace.
        /// </value>
        [ConfigurationProperty("replace", IsRequired = true, IsKey = false)]
        public string Replace
        {
            get { return (string)this["replace"]; }
            set { this["replace"] = value; }
        }
    }
}