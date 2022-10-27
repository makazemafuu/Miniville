using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    public class Monument : Cards
    {
        bool isActive = false;

        public Monument(int ActivationValue, string Name, int Type, int Cost) : base(ActivationValue, Name, Type, Cost)
        {

        }

        public void PassiveEffect()
        {
            while (!isActive)
            {
                if (ActivationValue == 4)
                {

                }
                if (ActivationValue == 10)
                {

                }
                if (ActivationValue == 16)
                {

                }
                if (ActivationValue == 22)
                {

                }
                isActive = true;
            }
        }
    }
}
