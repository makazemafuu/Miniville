using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    internal class Card
    {
        private string name = "";
        private int type;

        public Card(string name, int type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return name; } }
        public int Type { get { return type; } set { this.type = value; } }
    }
}
