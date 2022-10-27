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

        private void Shop(Card cartToBuy)
        {
            // on lui donne la carte à acheter en argument

            // on retire au joueur le coût de la carte
            //Player.coinsAvailable -= cartToBuy.cost;

            // on rend l'argent à la banque
            //Bank.coinsAvailable += cartToBuy.cost;

            // on ajoute la carte à la main



        }

        private void DisplayChoice()
        {
            //
        }

        private void PlayerChoice()
        {
            //
        }
    }
}
