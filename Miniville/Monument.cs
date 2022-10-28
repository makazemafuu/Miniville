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


        public Monument(string Name, int Type) 

        {
            this.name = Name;
            this.cost = Cost;
            this.IsActive = false;
        }
        public void PassiveEffect()
        {
            // Display passive effect of the Monument cards.
        }
    }
}
