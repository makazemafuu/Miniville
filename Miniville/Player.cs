﻿using NAudio.Codecs;
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
            new Monument("Gare", 4),
            new Monument("Centre Commercial", 10),
            new Monument("Tour Radio", 22),
            new Monument("Parc d'Attractions", 16)
        };

        public List<Monument> Monuments { get { return monuments; } }
        public bool IsAI { get { return isAI; } }
        public string NamePlayer { get { return namePlayer; } }
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

        public void Shop(int choixPile)
        {
            // il reçoit la carte à acheter en argument
            // on ajoute la carte à la main
            Trade(theBank, this, "Card", theBank.CardsAvailable.ElementAt(choixPile).Key);
            // on retire au joueur le coût de la carte
            // on rend l'argent à la banque
            Trade(this, theBank, "Coin", theBank.CardsAvailable.ElementAt(choixPile).Value.PileCards.Peek().Cost.ToString());    
        }

        public List<Card> DisplayChoice()
        {
            // consulte la valeur actuelle du dé
            // regarde les cartes activables

            List<Card> cartesActivables = new List<Card>();

            foreach (var card in CardsAvailable)
            {
                if (card.Value.PileCards.Peek().ActivationValue.Item1 == game.Dice.Face || card.Value.PileCards.Peek().ActivationValue.Item2 == game.Dice.Face)
                {
                    cartesActivables.Add(card.Value.PileCards.Peek());
                }
            }

            return cartesActivables;
        }
    }
}
