using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Miniville
{
	internal class Bank 
	{
		protected int coinsAvailable;
		protected Game game;
        protected Dictionary<string, Pile> cardsAvailable;
		private Pile pile;

        public Bank(Game Game, int money)
		{
			pile = new Pile();
			this.coinsAvailable = money;
			this.game = Game;
			cardsAvailable = pile.InitPile(game);
		}

        public void DisplayCards()
		{
			foreach (var card in cardsAvailable)
				Console.WriteLine(card.Value);
		}

		public Dictionary<String, Pile> CardsAvailable
		{
			get { return this.cardsAvailable; }
			set { this.cardsAvailable = value; }
		}

		public int CoinsAvailable
		{
			get { return this.coinsAvailable; }
			set { this.coinsAvailable = value; }
		}

		public void DisplayRessources(string from)
		{
			Console.WriteLine("{1} dispose de {0} pièces.", CoinsAvailable, from);
			foreach (var card in cardsAvailable)
			{
				Console.WriteLine("Il y a {0} cartes {1} restantes dans la réserve de {2}.", card.Value.PileCards.Count, card.Key, from);
            }
		}

        public void DisplayCardsOtherPLayer(string from)
        {
            foreach (var card in cardsAvailable)
            {
				if (card.Value.PileCards.Peek().Type != 3)
					Console.WriteLine("{2} a {0} cartes {1}.", card.Value.PileCards.Count, card.Key, from);
            }
        }
		public void ChooseCardOtherPlayer(string from)
        {
			int i = 0;
            foreach (var card in cardsAvailable)
            {
				if (card.Value.PileCards.Peek().Type != 3)
				{
					Console.WriteLine("Il y a {0} cartes {1} restantes dans la réserve de {2}, entrez \"{3}\" si vous voulez choisir celle là.", card.Value.PileCards.Count, card.Key, from, i);
					i++;
				}
            }
        }

        public void DisplayYourCards()
        {
            int i = 0;
            foreach (var card in cardsAvailable)
            {
                if (card.Value.PileCards.Peek().Type != 3)
				{
                    Console.WriteLine("Vous disposez de {0} carte(s) {1} , entrez \"{3}\" si vous voulez donner celle là", card.Value.PileCards.Count, card.Key, i);
					i++;
				}
			}
        }

		public void DisplayMoney(string from)
		{
			Console.WriteLine(from + "a {0} pièces, voulez-vous le dépouiller ?", CoinsAvailable);
		}

        public void Trade(Bank from, Bank to, string type, string value)
		{
			if (type == "Card")
			{
				from.CardsAvailable[value].PileCards.Peek().Owner = (Player)to;
				to.CardsAvailable[value].PileCards.Push(from.CardsAvailable[value].PileCards.Pop());
			}
			else if (type == "Coin")
			{
				to.CoinsAvailable = to.CoinsAvailable + int.Parse(value);
				from.CoinsAvailable = from.CoinsAvailable - int.Parse(value);
			}
		}
    }
}
