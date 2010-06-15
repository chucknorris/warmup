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

    public class IgnoredFileTypeCollection : ConfigurationElementCollection //, IEnumerable<IgnoredFileType>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoredFileType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoredFileType) element).Extension;
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
            get { return (string) this["ext"]; }
            set { this["ext"] = value; }
        }
    }
}