using System;
using System.Collections.Generic;
using System.Text;

namespace MiniVille_GraphiqueWF
{
    public class Die
    {
        public Random random = new Random();
        private int Face = 1;
        public int face
        {
            get { return Face; }
            set { Face = value; }
        }
        public virtual int Lancer()
        {
            face = 6 ; // pour faire des tests
            //face = random.Next(1, 7);
            return face;
        }
    }
}
 