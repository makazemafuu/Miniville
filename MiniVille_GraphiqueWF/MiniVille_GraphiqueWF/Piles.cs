using System;
using System.Collections.Generic;
using System.Text;

namespace MiniVille_GraphiqueWF
{
    public class Piles
    {
        public Stack<Cards> PileCartes;
        public Piles()
        {
            PileCartes = new Stack<Cards>();
        }
    }
}
