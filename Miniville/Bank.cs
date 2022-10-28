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
		private Pile pile = new Pile();

        public Bank(Game Game, int money)
		{
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

		public void DisplayRessources()
		{
			Console.WriteLine("There is {0} coins remaining in the Bank", CoinsAvailable);
			foreach (var card in cardsAvailable)
				Console.WriteLine("There is {0} cards of the type {1} remaining of the type in the Bank", card.Value.PileCards.Count, card.Key);
		}

		public void Trade(Bank from, Bank to, string type, string value)
		{
			if (type == "Card")
				to.CardsAvailable[value].PileCards.Push(from.CardsAvailable[value].PileCards.Pop());
			else if (type == "Coin")
			{
				to.CoinsAvailable += int.Parse(value);
				from.CoinsAvailable -= int.Parse(value);
			}
		}
    }
}
