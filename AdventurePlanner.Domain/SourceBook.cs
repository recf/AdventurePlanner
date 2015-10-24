using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurePlanner.Domain
{
    public class SourceBook
    {
        public SourceBook(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public string Identifier { get; private set; }

        public string Name { get; private set; }
    }
}
