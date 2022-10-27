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

        private void Shop(Card cartToBuy, Bank bank)
        {
            // il reçoit la carte à acheter en argument

            // on retire au joueur le coût de la carte
            coinsAvailable -= cartToBuy.Cost;

            // on rend l'argent à la banque
            bank.CoinsAvailable += cartToBuy.Cost;

            // on ajoute la carte à la main
            cardsAvailable[cartToBuy.Name].Push(cartToBuy);


        }

        private List<Card> DisplayChoice(Dice dice)
        {
            // consulte la valeur actuelle du dé
            // regarde les cartes activables

            List<Card> cartesActivables = new List<Card>();
            
            foreach (var card in CardsAvailable)
            {
               if ( card.Value.PileCards.Peek().ActivationValue == dice.face )
                {
                    cartesActivables.Add(card.Value.PileCards.Peek());
                }
            }

            return cartesActivables;
        }


        private void PlayerChoice(Card carteChoisie)
        {
            
        }
    }
}
