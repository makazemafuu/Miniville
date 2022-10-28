using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    public class Monument : Card
    {
        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Monument((int, int) ActivationValue, string Name, int Type, int Cost) : base(ActivationValue, Name, Type, Cost)
        {

        }
        public void PassiveEffect()
        {
            // Display passive effect of the Monument cards.
        }
    }
}
