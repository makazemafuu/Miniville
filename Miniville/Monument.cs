using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    internal class Monument : Card
    {
        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }



        public Monument(string Name, int Cost, int Type, string Description) 

        {
            this.name = Name;
            this.cost = Cost;
            this.IsActive = false;
            this.effectDescription = Description;
        }
    }
}
