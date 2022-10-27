using System;
using System.Xml.Schema;

namespace Miniville
{
   public class Dice
    {
        public string nbFaces;
        private Random random = new Random();
        private int face = 1;

        public Dice() => nbFaces = "6";
        public Dice(string nbFaces)
        {
            this.nbFaces = nbFaces;
        }

        public int Face
        {
            get { return face; }
            private set { face = value; }
        }

        public virtual int Roll()
        {
            return Face = random.Next(1, int.Parse(nbFaces) + 1);
        }

        public override string ToString()
        {
            string toString = string.Format("Le dé possède {0} faces, {1} est la face actuelle du dé.", nbFaces, face);
            return toString;
        }
    }
}