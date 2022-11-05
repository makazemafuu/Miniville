using NAudio.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    internal class Player : Bank
    {
        private string namePlayer;
        private bool isAI;
        private bool isPlaying = false;
        private Bank theBank;
        
        private List<Monument> monuments = new List<Monument>()
        {
            new Monument("Gare", 4, 3, "Vous pouvez lancez deux dés au lieu d'un."),
            new Monument("Centre Commercial", 10, 3, "Vos établissement Boulangerie, Supérette, Café, Restaurant vous rapportent une pièce de plus."),
            new Monument("Tour Radio", 22, 3, "Une fois par tour, vous pouvez choisir de relancer les dés."),
            new Monument("Parc d'Attractions", 22, 3, "Si votre lancé de dés est un double, vous pouvez rejouer un tour après celui-ci.")
        };

        public List<Monument> Monuments { get { return monuments; } }
        public bool IsAI { get { return isAI; } }
        public string NamePlayer { get { return namePlayer; } }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }
        public Player(string namePlayer, bool isAI, Game game, int money, Bank TheBank) : base(game, money)
        {
            this.namePlayer = namePlayer;
            this.isAI = isAI;
            this.game = game;
            this.coinsAvailable = money;
            this.theBank = TheBank;
            foreach (Pile pile in cardsAvailable.Values)
                pile.PileCards.Clear();
        }
        
        public void ShopIA(int choixPile, List<Card> cards)
        {
            if (choixPile == 42)
                return;
            // il reçoit la carte à acheter en argument
            // on ajoute la carte à la main
            if (cards[choixPile].GetType() != typeof(Monument))
                Trade(theBank, this, "Card", cards[choixPile].Name);
            else
                Monuments[Monuments.IndexOf((Monument)cards[choixPile])].IsActive = true;
            // on retire au joueur le coût de la carte
            // on rend l'argent à la banque
            Trade(this, theBank, "Coin", cards[choixPile].Cost.ToString());
            Console.WriteLine("\n" + NamePlayer + " a acheté " + cards[choixPile].Name + " pour la somme de " + cards[choixPile].Cost.ToString() + " pièce(s).\n\n");
        }

        public List<Card> DisplayChoice()
        {
            List<Card> aled = new List<Card>();
            // regarde les cartes activables
            int i = 0;
            int t = 0;
            while (i < game.Bank.CardsAvailable.Count)
            {
                if (game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Count > 0)
                    if (game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Peek().Cost <= CoinsAvailable)
                    { 
                        Console.WriteLine("{0} : {4} \n Coût : {1} pièce(s). {3} restantes en banque, entrez {2} pour l'acheter.\n", game.Bank.CardsAvailable.ElementAt(i).Key,
                            game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Peek().Cost, t, game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Count, 
                            game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Peek().EffectDescription);
                        aled.Add(game.Bank.CardsAvailable.ElementAt(i).Value.PileCards.Peek());
                        t++;
                    }
                i++;
            }
            int j = 0;
            while (j < Monuments.Count)
            {
                if (!Monuments[j].IsActive)
                    if (Monuments[j].Cost <= CoinsAvailable)
                    {
                        Console.WriteLine("{0} : {3} \n Coût : {1} pièce(s), entrez {2} pour l'acheter.\n", Monuments[j].Name, Monuments[j].Cost, t, Monuments[j].EffectDescription);
                        aled.Add(Monuments[j]);
                        t++;
                    }
                j++;
            }
            Console.WriteLine("Si vous ne souhaitez pas acheter de batiment, entrez '42'");
            return aled;
        }

        public void DisplayMonuments()
        {
            foreach (Monument monument in Monuments)
            {
                if (monument.IsActive)
                    Console.WriteLine("Vous possédez le monument : " + monument.Name + ".");
            }
        }
    }
}
