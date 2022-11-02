using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniVille_GraphiqueWF
{
    public class Game
    {
        public List<Player> playerList = new List<Player>();
        public Die die;
        public int NombreDe = 1, NombreDeIA = 1, scoreDes, tourJoueur = 1, MonnaieDisponible = 999;
        public bool DieThrowed = false;
        public List<Piles> CartesDisponibles = new List<Piles>(); 
        public Random random = new Random();


        public Game(int nbJoueur = 2)
        {
            for(int i = 0; i < nbJoueur; i++)
            {
                playerList.Add(new Player());
            }
            die = new Die();
            NombreDe = 1;
            foreach (NomCarte i in Enum.GetValues(typeof(NomCarte)))
            {
                Piles pile = new Piles();
                for(int j = 0; j < 6; j++)
                {
                    pile.PileCartes.Push(new Cards(i));
                }
                CartesDisponibles.Add(pile);
            }
        }


    }
}
