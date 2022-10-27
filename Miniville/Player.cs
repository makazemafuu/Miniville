using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    internal class Player : Bank
    {
        private string namePlayer;
        private bool isAI;
        private bool isPlaying;

        private List<Monument> monuments = new List<Monument>()
        {
            new Monument(),
            new Monument(),
            new Monument(),
            new Monument()
        };
        public bool IsAI { get { return isAI; } }
        public string NamePlayer { get { return namePlayer; } }
        public Player(string namePlayer, bool isAI)
        {
            this.namePlayer = namePlayer;
            this.isAI = isAI;

        }

        public void Shop(int cartToBuy)
        {
            // on lui donne la carte à acheter en argument

            // on retire au joueur le coût de la carte
            //Player.coinsAvailable -= cartToBuy.cost;

            // on rend l'argent à la banque
            //Bank.coinsAvailable += cartToBuy.cost;

            // on ajoute la carte à la main



        }

        public void DisplayChoice()
        {
            //
        }

        public void PlayerChoice()
        {
            //
        }
    }
}
