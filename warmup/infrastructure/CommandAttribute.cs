using System;

namespace warmup.infrastructure
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandAttribute(string name)
         {
             Name = name;
         }
    }
}